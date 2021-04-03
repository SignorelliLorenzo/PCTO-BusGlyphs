using System;
using Fleck;
using Creatore_archivio_pcto;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Messages;
using System.Linq;

namespace SERVER_PERCORSO
{
 
   
    class PERCORSO
    {
        
        static void Main(string[] args)
        {
            List<Percorso> Percorsi = new List<Percorso>();
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
        public static bool CaricaCOD_Glyphs(ref Dictionary<string, int> listainput, string path)
        {
            StreamReader file = new StreamReader(path);
            string jsonString = file.ReadToEnd();
            file.Close();
            try
            {
                listainput = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString);
            }
            catch
            {

                return false;
            }
            return true;
        }
        public static bool CaricaPercorsi(ref List<Percorso> percorsi, string path)
        {
            StreamReader file = new StreamReader(path);
            string jsonString = file.ReadToEnd();
            file.Close();
            try
            {
                percorsi = JsonConvert.DeserializeObject<List<Percorso>>(jsonString);
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}
