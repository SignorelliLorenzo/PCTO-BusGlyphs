using System;
using Fleck;

namespace SERVER_IMMAGINEMAPPA
{
    class ImmagineMappa
    {
        static void Main(string[] args)
        {
            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
            Console.WriteLine("Server Immagine-Mappa");
            websocketServer.Start(connection =>
            {

                connection.OnOpen = () =>
                {
                    Console.WriteLine("CLIENT CONNESSO");
                    connection.Send($"CONNESSO");
                };

                connection.OnClose = () =>
                {
                    Console.WriteLine("CLIENT DISCONNESSO");
                };
                connection.OnMessage = message =>
                {
                    if (message == "True")
                        connection.Send("Immagine mappa");

                };

                connection.OnError = exception =>
                {
                    Console.WriteLine($"OnError {exception.Message}");
                };

            });

            string fine = default;
            while (fine != "quit")
            {
                Console.WriteLine("Scrivere quit per uscire");
                fine = Console.ReadLine();
                fine = fine.Trim().ToLower();
            }
        }
    }
}
