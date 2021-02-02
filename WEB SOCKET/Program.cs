﻿using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_SOCKET
{
    class Program
    {
        static void Main(string[] args)
        {

            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
            websocketServer.Start(connection =>
            {

                connection.OnOpen = () =>
                {
                    Console.WriteLine("CLIENT CONNESSO");
                    connection.Send($"CONNESSO");
                };
                connection.OnClose = () =>
                  Console.WriteLine("CLIENT DISCONNESSO");
                connection.OnMessage = message =>
                {
                    Console.WriteLine($"OnMessage {message}");
                    if (message == "Richiesta")
                    {
                        connection.Send("c");
                    }
                };

                connection.OnError = exception =>
                  Console.WriteLine($"OnError {exception.Message}");
                connection.OnPing = bytes =>
                  Console.WriteLine("OnPing");
                connection.OnPong = bytes =>
                {
                    Console.WriteLine("OnPong");
                    connection.Close();
                };


            });
            string f = default;
            while (f != "quit")
            {
                f = Console.ReadLine();

            }
        }
    }
}