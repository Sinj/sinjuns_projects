/*
 * Writer class 
 * Writes/send commands to the server
 */
package TMS;

import java.io.IOException;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * @authors Holly, Sinjun, Adam
 */
public class Writer implements Runnable {

    Socket newSocket = null;
    PrintWriter write = null;

    Writer(Socket s) {//constructor
        newSocket = s;
        System.out.println("In write constructor");
    }

    @Override
    public void run() {
        try {//writes command to the server
            System.out.println("trying to write");
            write = new PrintWriter(newSocket.getOutputStream(), true);
            if (newSocket != null && write != null) {
                System.out.println("Neither writer null");
                write.println("HELO");
                write.println("HELP");
                write.println("REGI");
                write.println("BEGI");
            }
        } catch (UnknownHostException e) {
            System.err.println("Don't know about host: hostname");
        } catch (IOException e) {
            System.err.println("Couldn't get I/O for the connection to: hostname - Output");
        }
    }
}
