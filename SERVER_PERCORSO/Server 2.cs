using System;
using Fleck;
using Creatore_archivio_pcto_Percorso;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Messages;
using System.Linq;

namespace SERVER_PERCORSO
{
 
   
    public class PERCORSO
    {
        
        static void Main(string[] args)
        {
            
            List<Bus> Bus = new List<Bus>(); 

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
                    mexdestinazione message = JsonConvert.DeserializeObject<mexdestinazione>(msg);
                    
                    connection.Send(JsonConvert.SerializeObject(getmessage(message.Percorsi, Bus , message.Destinazione)));
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
        
        public static List<Bus> getmessage(List<Percorso> Percorsi,List<Bus> Bus, int Destinazione)
        {
            List<Percorso> Percorsigiusti = new List<Percorso>();
            Percorsigiusti=Percorsi.Where(x => x.elefermateandata.Contains(Destinazione) || x.elefermateritorno.Contains(Destinazione)).ToList();
            return Bus.Where(x => Percorsigiusti.Contains(x.percorso)).ToList();
        }
        public static bool CaricaBus(ref List<Bus> bus, string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            StreamReader file = new StreamReader(path);
            string jsonString = file.ReadToEnd();
            file.Close();
            try
            {
                bus = JsonConvert.DeserializeObject<List<Bus>>(jsonString);
            }
            catch
            {

                return false;
            }
            return true;
        }
        

    }
}
