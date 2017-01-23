using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PresenceTCPServer
{
    class Server
    {
        private readonly Func<string, string> _commandHandler;
        private readonly TcpListener _tcpListener;
        private readonly Thread _listenThread;
        private readonly List<TcpClient> _clients; 

        public Server(Func<string,string> commandHandler)
        {
            _clients=new List<TcpClient>();
            _commandHandler = commandHandler;
            _tcpListener = new TcpListener(IPAddress.Any, 3036);
            _listenThread = new Thread(ListenForClients);
            _listenThread.Start();

        }

        public void SendToAllClients(string message)
        {
            message += "\n";

            var toRemove = new List<TcpClient>();
            lock (_clients)
            {
                foreach (var client in _clients)
                {
                    if (client.Connected)
                    {
                        var clientStream = client.GetStream();
                        var encoder = new ASCIIEncoding();
                        var buffer = encoder.GetBytes(message);

                        clientStream.Write(buffer, 0, buffer.Length);
                        clientStream.Flush();
                    }
                    else
                    {
                        toRemove.Add(client);
                    }
                }
                foreach (var client in toRemove)
                    _clients.Remove(client);
            }
        }

        private void ListenForClients()
        {
            _tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                var client = _tcpListener.AcceptTcpClient();
                _clients.Add(client);
                //create a thread to handle communication 
                //with connected client
                var clientThread = new Thread(HandleClientComm);
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            Console.WriteLine("connected");
            var tcpClient = (TcpClient)client;
            var clientStream = tcpClient.GetStream();

            ReceivedCommand(tcpClient, "connected");
            var message = new byte[4096];
            var command = "";
            while (true)
            {
                var bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                var encoder = new ASCIIEncoding();
                command += encoder.GetString(message, 0, bytesRead);
                Console.WriteLine(command);
                if (command[command.Length - 1] == '\n')
                {
                    ReceivedCommand(tcpClient, command.Trim());
                    command = "";
                }
            }

            tcpClient.Close();
        }

        private void ReceivedCommand(TcpClient client, string command)
        {
            Console.WriteLine("Received command {0}", command);

            var toSend = "";

            if (_commandHandler != null)
            {
                toSend = _commandHandler(command);
            }

            if (toSend != "")
            {
                toSend += "\n";
                var clientStream = client.GetStream();
                var encoder = new ASCIIEncoding();
                var buffer = encoder.GetBytes(toSend);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            }
        }
    }
}