using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

class ChatServer
{
    static TcpListener server;
    static List<TcpClient> clients = new List<TcpClient>(); // List to keep track of all connected clients

    public static void Main()
    {
        // Start the server
        server = new TcpListener(IPAddress.Any, 5000); // Listen on all network interfaces
        server.Start();
        ServerMessage("Server started...");

        while (true)
        {
            try
            {
                // Accept a new client connection
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);
                
                ServerMessage(BroadcastMessage("New user entered the chat!"));

                // Handle this new client in a separate thread
                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
            catch (Exception ex)
            {
                ServerErrorMessage("Error accepting client: " + ex.Message);
            }
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while (true)
            {
                // Read the message from the client
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    // Client disconnected
                    ServerMessage(BroadcastMessage("A user disconnected."));
                    break;
                }

                // Convert the message to a string and display it
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine(message);

                // Broadcast the message to all clients
                BroadcastMessage(message);
            }
        }
        catch (Exception ex)
        {
            ServerErrorMessage("Error with client:" + ex.Message);
        }
        finally
        {
            // Clean up
            stream.Close();
            client.Close();
        }
    }

    static string BroadcastMessage(string message)
    {
        // Send the message to all connected clients
        foreach (var client in clients)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                ServerErrorMessage("Error sending message to client: " + ex.Message);
            }
        }
        return message;
    }

    static void ServerMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
    static void ServerErrorMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
