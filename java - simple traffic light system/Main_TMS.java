/*
 * Main - sets up the socket and threads, runs the threads
 */
package TMS;

import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * @authors Holly, Sinjun, Adam
 */

public class Main_TMS {

    public static void main(String[] args) {
        Socket socket = null;
        try {
            socket = new Socket("192.168.0.208", 5000);//make socket 
            Writer write = new Writer(socket);// make writer with socket
            Reader read = new Reader(socket);//make read with scocket
            Thread tWrite = new Thread(write);//make thread
            Thread tRead = new Thread(read);
            System.out.println("Starting write thread");
            tWrite.start();//start thread
            System.out.println("Starting read thread");
            tRead.start();//start thread
        } catch (UnknownHostException e) {
            System.out.println("Unknown Host");
            System.err.println(e.getMessage());
        } catch (IOException e) {
            System.out.println("IO Exception");
            System.err.println(e.getMessage());
        }
    }
}
