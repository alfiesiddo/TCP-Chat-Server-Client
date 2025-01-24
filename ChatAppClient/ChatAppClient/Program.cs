using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net.Sockets; 


namespace ChatAppClient
{
    internal class Program
    {
        static TcpClient client;
        static NetworkStream stream;
        static Thread listenerThread;
        static string targetAddress = "";
        static string username = "user";
        static void Main(string[] args)
        {
            SetUp();
            try
            {
                // Connect to the server (replace with the server's IP if necessary)
                TcpClient client = new TcpClient(targetAddress, 5000); // Server's IP or localhost (for local machine)
                NetworkStream stream = client.GetStream();

                // Start a thread to listen for incoming messages from the server
                Thread listenerThread = new System.Threading.Thread(() => ListenForMessages(stream));
                listenerThread.Start();

                // Send messages to the server
                while (true)
                {
                    string message = Console.ReadLine();

                    if (string.IsNullOrEmpty(message)) continue;

                    if (message.ToLower() == "leave") break; // Exit the loop on 'leave'

                    // Send the message to the server
                    byte[] data = Encoding.ASCII.GetBytes(username + ": " + message);
                    stream.Write(data, 0, data.Length);
                }

                // Close the connection when done
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void ListenForMessages(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        // If no data is read, the server may have disconnected
                        Console.WriteLine("Server disconnected.");
                        break;
                    }

                    // Convert the received data to a string and display it
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error receiving message: " + ex.Message);
                    break;
                }
            }
        }
        static void SetUp()
        {
            Console.WriteLine("enter a username!");
            username = Console.ReadLine();
            Console.WriteLine("enter Server IP address!");
            targetAddress = Console.ReadLine();
            Console.WriteLine("You're trying to connect to " + targetAddress);
        }
    }

   static void StopRepeat

    //MAKE IT SO THAT IF A STRING COMES THROUGH THAT MATCHES USERNAME, DO NOT DISPLAY IT, THIS IS BECAUSE USER SEES TWICE
   
}
