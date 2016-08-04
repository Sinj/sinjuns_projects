#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os
import mary_tts.msg
import time
import math
import rospy
import random
import actionlib
import json
import numpy as np
import flir_pantilt_d46.msg
import move_base_msgs.msg 
from std_msgs.msg import String
from geometry_msgs.msg import PoseStamped 
#from strands_gazing.msg import GazeAtPoseAction, GazeAtPoseGoal

#sub to "/human/transformed" when in sim
#sub to "/people_tracker/pose"  when on robot

sqrt = 0.0  # hold the sqrt distance of the human from the robot
hstate = 0  # hold the humans current state, i.e. following, still or moving away.
sim = False #in simulation
def makegoal(x,y,z,w): #makes move goal & return it 
    target = move_base_msgs.msg.MoveBaseGoal()
    target.target_pose.header.frame_id = "/map"
    target.target_pose.pose.position.x = x
    target.target_pose.pose.position.y = y
    target.target_pose.pose.orientation.z = z
    target.target_pose.pose.orientation.w = w 
    return target     

def wherehuman(data):
    #get the euclidean distance the humans is away from  the robot and store it  
    global sqrt
    toBeSqrt = (data.pose.position.x * data.pose.position.x) + (data.pose.position.y * data.pose.position.y)   
    sqrt = math.sqrt(toBeSqrt)
  

def boo (qtc_msg):
    global hstate 
    data = json.loads(qtc_msg.data)
    if data[len(data)-1][0] == '0':
        #print "Human standing still"
        hstate = 0
    elif data[len(data)-1] == '+':
        #print"human running away!!!"
        hstate = 1
    else:
        #print "human following"
        hstate = 2

def speak(client, text):
    global sim
    if sim == True:
        print speak
    else:
        client.send_goal_and_wait(mary_tts.msg.maryttsGoal(text=text))
    
def robot_simple():
    rospy.init_node('robot_simple', anonymous=True)
    rospy.Subscriber("/online_qtc_creator/QTC", String, boo)
    
    r = rospy.Rate(0.3) # .3hz    
    
    global hstate 
    global sqrt 
    
    global sim  #in simulation
    
    end = False #guide end flag
    k=0# The index for cords
    speakfile = rospy.get_param("~speak")
    cordsfile = rospy.get_param("~cords")
    robotstate = 0 #intial state of the robot   
    
    maxdistance = 4.5  # stores max distance the human can be from the robot
    encourgedis = 3  # distance the robot will start to try and encouge them to keep up
    mindistance = 0.5  # stores min distance the human can be from the robot  
    
    encourgetime = 3 # how long to wait to encourge agian
    beforewaittime = 3 #how long to count down before robot stops moving
    waitbeforending = 10 #how long the robot will wait for before ending guide
    spothuman = 8
    
    starttimer = False # check if should start timer
    waitforhuman = False # check if waiting for human
    losthuman = False # check if lost human
    firststop = True#check if first pass though wait while   
    
    #below is the end cords for the robot to go to.

    
    counter = 0# used to catch errors with human state
    countmax = 2# max amount of counts needed for it not to be an error
            #make client
    print"making move clients"
    baseClient = actionlib.SimpleActionClient('move_base',move_base_msgs.msg.MoveBaseAction)
    baseClient.wait_for_server()
    
    #~~~~~~~~~~~~~~~
    dlist = []
    dlist.append('========== New participant ===============')
    startimer = time.time()
    stats_hnec = 0 #human needs encourgement counter
    stats_rsfhc = 0#robot stoped for human counter
    stats_rlhc = 0 #robot lost human counter
    idint = random.randint(1,200) # random an id for this test.
    dlist.append('~~~ ID num = {} ~~~'.format(idint))
    #~~~~~~~~~~~~~~~    
    
    if sim == False:
        rospy.Subscriber("/people_tracker/pose", PoseStamped, wherehuman)        
        
        print"making speaking clients"       #Make speaking client
        maryclient = actionlib.SimpleActionClient('speak',mary_tts.msg.maryttsAction)    
        maryclient.wait_for_server()        
    
        #make PTU client
        ptuclient = actionlib.SimpleActionClient('SetPTUState',flir_pantilt_d46.msg.PtuGotoAction)    
        ptuclient.wait_for_server()
        
        #face to foward PTU goal
        ptuforward = flir_pantilt_d46.msg.PtuGotoGoal()
        ptuforward.pan = 0
        ptuforward.pan_vel = 60
        
        #face to backwards PTU goal
        ptubwrd = flir_pantilt_d46.msg.PtuGotoGoal()
        ptubwrd.pan = -180
        ptubwrd.pan_vel = 60
	ptubwrd.tilt = 10
        ptubwrd.tilt_vel = 60        
        #ptuclient.send_goal_and_wait(ptuforward) #rotate ptu face foward
        #ptuclient.send_goal_and_wait(ptubwrd) #rotate ptu face bkward

        endx = -2.0
        endy = 0.0
        endz = 1.0
        endw = 0.1
    else:
        rospy.Subscriber("/human/transformed", PoseStamped, wherehuman)
        endx = 1.749
        endy = -3.893
        endz = 0.999
        endw = -0.038   
        
    
    # Below is the open and read file- first as float, second as string
    print "reading cords from {}".format(cordsfile)
    cord=[]
    for line in open(cordsfile):
       cord.append(np.array([float(val) for val in line.rstrip('\n').split(' ') if val != '']))
       
    print "reading speaking text from {}".format(speakfile)           
    with open(speakfile, "r") as f:
        speakarray = map(str.rstrip, f)
    
    ptuclient.send_goal(ptubwrd)
    speak(maryclient,speakarray[0]) #move to starting area    
    #move to starting area.
    print'Going to start at coords: X:{} Y:{} Z:{} W:{} and there are {} waypoints in total'.format(cord[0],cord[1],cord[2],cord[3], (len(cord)/4))
    baseClient.send_goal_and_wait(makegoal(cord[k],cord[k+1],
                                                       cord[k+2],cord[k+3]))
    
    speak(maryclient,speakarray[1]) #greet user               
    speak(maryclient,speakarray[2]) #ask them to move behind robot             
    speak(maryclient,speakarray[random.randint(3,4)])
                                                                                                               
    k = k +4 #move index to next set of cords
    print'starting route at coords: X:{} Y:{} Z:{} W:{} and there are {} waypoints on route'.format(cord[0],cord[1],cord[2],cord[3], (len(cord)/4-1))
    
    encoutimer = time.time()# make timer for encourgement
    while not rospy.is_shutdown():
        if end == False:        
            if robotstate <3:
#               print 'bp'
#               print waitforhuman, 'current wait'
#               print sqrt, ' current human distance'
#               print hstate, ' current human state'              
#               print robotstate, ' current robots state.'
               #~~~~~~~~~~~~~
              # dlist.append('user distance standing {}'.format(sqrt))
               #~~~~~~~~~~~~~

               baseClient.send_goal(makegoal(cord[k],cord[k+1],
                                       cord[k+2],cord[k+3]))   
                                       
               spothumtimer = time.time()# timer for spotting the human
               while robotstate < 3 and not rospy.is_shutdown():
                   #~~~~~~~~~~~~~
	          # dlist.append('user distance movving {}'.format(sqrt))
              	   #~~~~~~~~~~~~~

                   robotstate = baseClient.get_state()#get the current state of robot                              
                   if sqrt > encourgedis and sqrt < maxdistance: # start encourging when over X distance
                       spothumtimer = time.time()#reset both timer and sqrt                       
                       sqrt = 0.0 
                       if ((encoutimer+encourgetime) < time.time()): #check if can encourge 
                           print 'il encourge you to keep up'
                           starttimer = False
                           speak(maryclient,speakarray[random.randint(5,7)]) #gives encourgement               
                           encoutimer = time.time()# reset timer
                           #~~~~~~~
	                   dlist.append('user distance standing {}'.format(sqrt))
                           stats_hnec = stats_hnec +1
                           #~~~~~~~
                   elif sqrt > maxdistance: # start stopping when the user is far away
                       spothumtimer = time.time()#reset both timer and sqrt
                       sqrt = 0.0 
                       if starttimer == False:
                            waittimer = time.time() #start a timer for when the robot should stop moving forward
                            starttimer = True
                            print 'human is to far - timer started'
                       elif starttimer == True and ((waittimer+beforewaittime) < time.time()):
                           #print "starttimer {:.2f} seconds ".format(((encoutimer+encourgetime) - time.time()))
                           waitforhuman = True
                           speak(maryclient,speakarray[random.randint(8,9)])
                           #~~~~~~~
                           stats_rsfhc = stats_rsfhc +1
                           #~~~~~~~
                       else:
                           print "seconds before stopping to wait human: {:.2f} seconds ".format(((waittimer+beforewaittime) - time.time()))                   
                           time.sleep(1)
                   elif sqrt > mindistance and sqrt < encourgedis:
                       spothumtimer = time.time()#reset both timer and sqrt
                       sqrt = 0.0
                       #print 'reset'
                   elif (spothumtimer+spothuman) < time.time():
                       waitforhuman = True
                       losthuman = True#needs 2 do somthing with this
                       speak(maryclient,speakarray[random.randint(10,11)])
                       #~~~~~~~
                       stats_rlhc = stats_rlhc+1
                       #~~~~~~~
                   else:
                       if ((spothumtimer+spothuman) - time.time()) < 5:
                           print "seeking human for {:.2f} seconds ".format(((spothumtimer+spothuman) - time.time()))
                       
                   endtimer = time.time()
                   #sqrt = 0.0
                   while waitforhuman and robotstate < 3 and not rospy.is_shutdown():
                       if firststop:
                           speak(maryclient,speakarray[random.randint(17,18)])
                           firststop = False
                       baseClient.cancel_all_goals()
                       starttimer = False
                       if ((endtimer+waitbeforending) < time.time()):                          
                           print 'ending the guide as human not returing'
                           if losthuman:
                               speak(maryclient,speakarray[16])# inform user of ending guide due no human
                               #~~~~~~
                               dlist.append('human not spotted within time limit')
                               #~~~~~~
                           else:
                               speak(maryclient,speakarray[12])# inform user of ending guide due to them not returing closer
                               #~~~~~~
                               dlist.append('human didnt move closer within time limit')
                               #~~~~~~
                           end = True
                           robotstate = 3                          
                       else:
                           print "seconds before ending: {:.2f} seconds ".format(((endtimer+waitbeforending) - time.time()))
                           time.sleep(1)
                           print hstate
                       if not end:    
                           if hstate == 2 and sqrt < (maxdistance):
                               print'human is moving towards me'
                               robotstate = 3
                               speak(maryclient,speakarray[21])
                               counter = 0
                           elif hstate == 2 and sqrt > maxdistance:
                               if ((encoutimer+(encourgetime-2)) < time.time()):
                                   speak(maryclient,speakarray[random.randint(22,23)])                                   
                           elif hstate == 1 and sqrt > maxdistance:
                               if counter == countmax:
                                   print'human is moving away end guide'
                                   speak(maryclient,speakarray[15])
                                   robotstate = 2
                                   end = True
                               else:
                                   counter =+1                                   
                           elif hstate == 0:
                               if ((encoutimer+encourgetime) < time.time()):
                                   speak(maryclient,speakarray[random.randint(13,14)])
                                   print 'give encourgement for them to come to the robot'
                                   encoutimer = time.time()
                               
                                      
            if robotstate == 3:
                robotstate = 2
                firststop = True
                if len(cord)-1 > k+4 and waitforhuman == False:
                    speak(maryclient,speakarray[((k/4)+24)])
                    k = k +4 #move index to next set of cords
                    print'next goal coords: X:{} Y:{} Z{}: W:{}'.format(cord[k],cord[k+1],cord[k+2],cord[k+3])
                    print'moving to waypoint {}/{}'.format(((k+4)/4),(len(cord)/4))
                elif waitforhuman:
                    print 'change waithuman flag'
                    waitforhuman = False
                else:
                    speak(maryclient,speakarray[19])
                    print'the guiding has finished'
                    end = True
                    #~~~~~~
                    dlist.append('no more waypoints- end')
                    #~~~~~~                                                            
        if end:
            if robotstate <3:
                print'moving robot back to start'
                speak(maryclient,speakarray[20])
                baseClient.send_goal_and_wait(makegoal(endx,endy,endz,endw))
                robotstate = baseClient.get_state() #get the current state of robot
            if robotstate == 3:              
                 ptuclient.send_goal(ptuforward)
                 #~~~~~
		 dlist.append('number of times encuragued {}'.format(stats_hnec))
                 dlist.append('number of stops {}'.format(stats_rsfhc))
                 dlist.append('number of lost{}'.format(stats_rlhc))
                 dlist.append('guide end, took {:.2f} seconds'.format(startimer - time.time()))
                 dlist.append('guide end, took {:.2f} seconds'.format((startimer - time.time())/60))                 
                 dlist.append('=========== end ======== \n')
                 with open(rospy.get_param("~out") + '/Method1_simple_stats2.txt','a') as File:
                     for item in dlist:
                         print>>File, item
                 #~~~~~
                 print'program end'
                 speak(maryclient,speakarray[24])
                 rospy.signal_shutdown("program end")
                 
        r.sleep()
    
if __name__ == '__main__':
    try:
        robot_simple()
    except rospy.ROSInterruptException: pass
