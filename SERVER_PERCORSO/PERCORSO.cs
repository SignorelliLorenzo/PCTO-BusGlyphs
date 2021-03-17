using System;
using Fleck;
using Creatore_archivio_pcto;
using System.Collections.Generic;

namespace SERVER_PERCORSO
{
    class PERCORSO
    {
        
        static void Main(string[] args)
        {
            List<Percorso> Percorsi = new List<Percorso>();


            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
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
                connection.OnMessage = msg =>
                {
                    int Attuale = int.Parse(msg.Split(";")[0]);
                    int Destinazione = int.Parse(msg.Split(";")[1]);

                };

                connection.OnError = exception =>
                {
                    Console.WriteLine($"OnError {exception.Message}");
                };

            });
            string f = default;
            while (f != "quit")
            {
                f = Console.ReadLine();

            }

        }
        public static List<Bus> getmessage(int attuale,int destinazione, List<Percorso> percorsi, List<Bus> bus)
        {
            int codicefermata = -1;
            
            foreach (var item in percorsi)
            {
                

            }
            message = JsonConvert.SerializeObject(NRmessage);
            return message;
        }
    }
}
