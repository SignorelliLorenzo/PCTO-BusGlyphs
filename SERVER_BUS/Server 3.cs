using System;
using Fleck;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Creatore_archivio_pcto;

namespace SERVER_BUS
{
    class ServerBus
    {
       
        
        static void Main(string[] args)
        {
            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
            Console.WriteLine("Server Bus");
            Dictionary<string, Coordinate> coordinatepullman = new Dictionary<string, Coordinate>();

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
                    if (message.StartsWith("gps%"))
                    {
                        var risposta = message.Split("%");
                        coordinatepullman[risposta[1]] = JsonConvert.DeserializeObject<Coordinate>(risposta[2]);
                        return;
                    }

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
