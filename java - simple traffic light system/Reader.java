/*
 * This is the reader class
 * It reads from the server, splits the data and outputs the currect stats of the lights
 */
package TMS;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.List;

/**
 * @authors Holly-Marie, Sinjun, Adam
 */
public class Reader implements Runnable {

    Socket newSocket = null;
    BufferedReader in = null;
    List<String> lane1out = new ArrayList<>();
    List<String> lane2out = new ArrayList<>();
    List<String> lane3out = new ArrayList<>();
    List<String> lane4out = new ArrayList<>();
    List<String> ped1 = new ArrayList<>();
    List<String> ped2 = new ArrayList<>();
    List<String> ped3 = new ArrayList<>();
    List<String> ped4 = new ArrayList<>();
    long time = System.currentTimeMillis();//get miliseconds
    int selector = 1;//used to deteremine what line is green

    Reader(Socket s) {//constructor
        newSocket = s;

    }

    @Override
    @SuppressWarnings("empty-statement")
    public void run() {
        try {
            in = new BufferedReader(new InputStreamReader(newSocket.getInputStream()));
        } catch (IOException e) {
            System.err.println("Couldn't get I/O for the connection to: hostname - Input");
        }
        //check if there is any data to read
        if (newSocket != null && in != null) {
            try {
                String responseLine;
                while ((responseLine = in.readLine()) != null) {
                    //below handles car detection and indexing to relevant arrays
                    if (responseLine.contains(":") && responseLine.contains(";")) {//split the string to get the cars
                        String[] carStream = responseLine.split(";");
                        for (String carStream1 : carStream) {
                            switch (carStream1.charAt(0)) {//check the first character, then store cars by lane
                                case '1':
                                    lane1out.add(carStream1);
                                    break;
                                case '2':
                                    lane2out.add(carStream1);
                                    break;
                                case '3':
                                    lane3out.add(carStream1);
                                    break;
                                case '4':
                                    lane4out.add(carStream1);
                            }
                        }
                    } //below handles pedestrians detection and indexing to relevant arrays
                    else if (responseLine.contains(";")) {//split the string to get pedestrians
                        String[] pedStream = responseLine.split(";");
                        for (String pedStream1 : pedStream) {
                            switch (pedStream1.charAt(0)) {//store the pedestrians at crossing
                                case '1':
                                    ped1.add(pedStream1);
                                    break;
                                case '2':
                                    ped2.add(pedStream1);
                                    break;
                                case '3':
                                    ped3.add(pedStream1);
                                    break;
                                case '4':
                                    ped4.add(pedStream1);
                            }
                        }
                    }
                    //below outputs the state of the lights
                    System.out.print("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");//our bad way of clearing some space on the console
                    System.out.println("Current Event:");
                    switch (selector) {//based on the selectior which light is green
                        case 1:// all light excpt light 1 are red
                            lane1out.clear();

                            System.out.println("Exit 1 lights are green. Cars can go.");
                            break;
                        case 2:// all light excpt light 2 are red
                            lane2out.clear();

                            System.out.println("Exit 2 lights are green. Cars can go.");
                            break;
                        case 3:// all light excpt light 3 are red
                            lane3out.clear();

                            System.out.println("Exit 3 lights are green. Cars can go.");
                            break;
                        case 4:// all light excpt light 4 are red
                            lane4out.clear();

                            System.out.println("Exit 4 lights are green. Cars can go.");
                            break;
                        case 5://all lights go read giving pedestrians chance to cross
                            ped1.clear();
                            ped2.clear();
                            ped3.clear();
                            ped4.clear();

                            System.out.println("All exits are red. Pedestrians are crossing.");
                            break;
                    }

                    if ((time + 5000) < System.currentTimeMillis()) {//checks if 5 seconds have passed

                        time = System.currentTimeMillis();//get the new time
                        if ((ped1.size() + ped2.size() + ped3.size() + ped4.size()) > 10) {//set all light to read if more then 10 pedestrians waiting
                            selector = 5;
                        } else {//manges the light selection, 
                            selector++;

                            if (selector > 4) {
                                selector = 1;

                            }
                        };
                    }
                    //outputs information about the number of cars and pedestrians
                    System.out.println(" ");
                    System.out.println("Cars at Exit 1: " + lane1out.size());
                    System.out.println("Cars at Exit 2: " + lane2out.size());
                    System.out.println("Cars at Exit 3: " + lane3out.size());
                    System.out.println("Cars at Exit 4: " + lane4out.size());
                    System.out.println("Total Cars Waiting: " + (lane1out.size() + lane2out.size() + lane3out.size() + lane4out.size()));
                    System.out.println(" ");
                    System.out.println("Peds at Exit 1: " + ped1.size());
                    System.out.println("Peds at Exit 2: " + ped2.size());
                    System.out.println("Peds at Exit 3: " + ped3.size());
                    System.out.println("Peds at Exit 4: " + ped4.size());
                    System.out.println("Total Pedestrians Waiting: " + (ped1.size() + ped2.size() + ped3.size() + ped4.size()));
                }

                in.close();
            } catch (UnknownHostException e) {
                System.err.println("Trying to connect to unknown host: " + e);
            } catch (IOException e) {
                System.err.println("IOException: " + e);
            }
        }
    }
}
