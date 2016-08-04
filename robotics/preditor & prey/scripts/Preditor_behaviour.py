#!/usr/bin/env python
# -*- coding: utf-8 -*-

import rospy                                  # The ROS python bindings
from geometry_msgs.msg import Twist           # The cmd_vel message type
from sensor_msgs.msg import Image             # The message type of the image
from sensor_msgs.msg import LaserScan  # The message type of the laser scan
from kobuki_msgs.msg import BumperEvent
from cv_bridge import CvBridge, CvBridgeError # OpenCV ROS functions
import cv2                                    # OpenCV functions
import numpy                                  # Matlab like functions to work on image
import time

class Preditor():

    def __init__(self, name):
        
        #rospy.loginfo("Starting node %s" % name)
        self.bridge = CvBridge()
        self.timer = time.time()
        self.bumped = False
        self.bumpstate =  0
        self.min_distance = 0 
        self.checkstat = True
        self.donerun = True
        self.issafe = True       #- spin around at loaction till box seen
        self.turnsmallest = 0.0        
        self.iswall = False     #- if see on left : turn 140 deg and run away else :turn -140 deg, set run away to true
        self.isrun = False       #-start timer, if running away for X seconds stop rotate 180          # Creatingan OpenCV bridge object used to create an OpenCV image from the ROS image
        cv2.namedWindow("Image window", 1)  # Opening a window to show the image
        cv2.startWindowThread()

        #"/camera/rgb/image_color"
        #"/turtlebot_1/camera/rgb/image_raw"
        self.image_sub = rospy.Subscriber(  # Creating a subscriber listening to the kinect image topic
           "/camera/rgb/image_color", # The topic to which it should listened
           #"/turtlebot_2/camera/rgb/image_raw",
            Image,                          # The data type of the topic
            callback=self.image_callback,   # The callback function that is triggered when a new message arrives
            queue_size=1                    # Disregard every message but the latest
        )
        self.laser_sub = rospy.Subscriber(  # Creating a subscriber listening to the laser scans
            '/scan',                        # The topic to which it should listend
            #'/turtlebot_2/scan',
            LaserScan,                      # The data type of the topic
            callback=self.laser_callback,   # The callback function that is triggered when a new message arrives
            queue_size=1                    # Disregard every message but the latest
        )
        
        self.cmd_vel_pub = rospy.Publisher( # The same as previously
            "/cmd_vel",                     # The topic to which it should publish
            #"/turtlebot_2/cmd_vel",
            Twist,                          # The data type of the topic
            queue_size=1                    # Explicitly set to prevent a warining in ROS
        )
        
        self.Bumper_sub = rospy.Subscriber(  # Creating a subscriber listening to the Bumper
            '/mobile_base/events/bumper',                       # The topic to which it should listend
            #'/turtlebot_2/turtlebot_2_kobuki_safety_controller/events/bumper',
            BumperEvent,                      # The data type of the topic
            callback=self.Bumper_callback,   # The callback function that is triggered when a new message arrives
            queue_size=1                    # Disregard every message but the latest
        )


    def Bumper_callback(self, bumper):     
        if bumper.bumper > 0:
            self.bumped = True
        self.bumpstate = bumper.state
        
    def image_callback(self, img):
        #rospy.loginfo("Received image of size: %i x %i" % (img.width,img.height))  # Just making sure we received something
        rate = rospy.Rate(10)
        
        try:
            cv_image = self.bridge.imgmsg_to_cv2(img, "bgr8")  # Convert to OpenCV image 
        except CvBridgeError, e:
            print e
        #chance the below colour thing to the hvs 
        hsv_img = cv2.cvtColor(cv_image, cv2.COLOR_BGR2HSV) #get image
        

        hsv_thresh = cv2.inRange(hsv_img,
                                 numpy.array((50, 65, 0)),#0,220,0
                                 numpy.array((100, 255, 255)))# get  from range-150,255,255

                               
        kernel = numpy.ones((5,5),numpy.uint8) #make structure
        hsv_thresh = cv2.morphologyEx(hsv_thresh, cv2.MORPH_OPEN, kernel)# Erosion then Dilation
        
        cv2.imshow("Image window", hsv_thresh)    
        
        hsv_thresh/=255 # divides whole image by 225
        twist_msg = Twist()
        wholeintensity = numpy.mean(hsv_thresh)
        leftim = numpy.mean(hsv_thresh[:, 0:215])#/wholeintensity
        midim = numpy.mean(hsv_thresh[:, 215:425])#/wholeintensity
        rightim = numpy.mean(hsv_thresh[:, 425:640])#/wholeintensity      

        
        wholeintensity1 = numpy.sum(hsv_thresh) 
        
        if self.checkstat == True:
        #check if seen
            if wholeintensity1 > 160:#200:#100:#600:
                self.robotseen =True
            else:
                self.robotseen = False
          

        if self.robotseen and self.min_distance <=1.2: # charging 
             if self.checkstat:
                 self.checkstat = False
             print 'in'
             twist_msg.linear.x = 0.3
             twist_msg.angular.z = 0
                
        elif self.robotseen and self.min_distance > 1.2: # move towards opponent
            #print 'out'
            self.robotseen =True
            if not self.iswall:
                print 'out'
                twist_msg.linear.x = 0.5
                if midim > leftim and midim > rightim:
                    twist_msg.angular.z = 0
                elif leftim > rightim:
                    twist_msg.angular.z = numpy.pi/8
                else:
                    twist_msg.angular.z = (numpy.pi/8)*-1
            else:
                twist_msg.linear.x = 0.0
                if leftim > rightim:
                    twist_msg.angular.z = (numpy.pi/8)*-1
                else:
                    twist_msg.angular.z =  numpy.pi/8       
        else:
            print'not seen'
            if not self.iswall:
                twist_msg.linear.x = 0.5
                twist_msg.angular.z = 0 
            else:
                twist_msg.linear.x = 0.0
                twist_msg.angular.z = self.turnsmallest
                
            
        if self.bumpstate:
            twist_msg.linear.x = 0.0
            twist_msg.angular.z = 0
            print 'bumper hit'
            
             
        cv2.imshow("Image window", hsv_thresh)  # Showing the image
        self.cmd_vel_pub.publish(twist_msg)     # Publishig the twist message
        rate.sleep()                         # Sleep to ensure rate of 10hz 
        
    def laser_callback(self, msg):
        ranges = msg.ranges                 # Getting the range values of the laser rays
        leftside = numpy.mean(numpy.nanmin(ranges[0:320])) #get left side
        rightside = numpy.mean(numpy.nanmin(ranges[321:640]))#get right side
     
        if (leftside - rightside) >=-0.20 and (leftside - rightside) <=0.20: # it nether turn 180
            self.turnsmallest = numpy.pi
        elif leftside < rightside:                                           #  turn right 
            self.turnsmallest = numpy.pi/2
        else:                                                                   #turn left
            self.turnsmallest = (numpy.pi/2)*-1     
        
        
        self.min_distance = numpy.nanmin(ranges)  # Using numpy's nanmin function to find the minimal value and ignore nan values
        if self.min_distance < 0.95:                    # If the robot is close, then
            self.iswall = True 
        else:
            self.iswall = False
if __name__ == '__main__':
    rospy.init_node("Preditor")     # Create a node of name braitenberg
    b = Preditor(rospy.get_name())  # Create an instance of above class
    rospy.spin()                       # Function to keep the node running until terminated via Ctrl+C
