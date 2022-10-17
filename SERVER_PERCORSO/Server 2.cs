using Bus_Percorsi;
using Fleck;
using Mex;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SERVER_PERCORSO
{
    public class PERCORSO
    {
        static string indirizzo = "ws://192.168.1.126:8181";
        static List<Bus> Bus = new List<Bus>();
        static List<Percorso> Percorsi = new List<Percorso>();
        static void Main(string[] args)
        {
   
           
            Console.WriteLine("--------------------Server Percorso--------------------");
            
            if (!CaricaBus(ref Bus, "BusList.json"))
            {
                Console.WriteLine("ERRORE: Non sono riuscito a caricare i bus");
                Console.ReadKey();
                return;
            }
            if (!CaricaPercorsi(ref Percorsi, "Percorsi.json"))
            {
                Console.WriteLine("ERRORE: Percorsi non caricati");
                Console.ReadKey();
                return;
            }

            var websocketServer = new WebSocketServer(indirizzo);
            
            websocketServer.Start(connection =>
            {

                connection.OnOpen = () =>
                {
                    Console.WriteLine("CLIENT CONNESSO");
                    
                };
                connection.OnClose = () =>
                {
                    Console.WriteLine("CLIENT DISCONNESSO");
                };
                connection.OnMessage = msg =>
                {

                    try
                    {
                        var message = JsonConvert.DeserializeObject<ServerBuses.Request>(msg);
                        var json = JsonConvert.SerializeObject(getmessage(message.Arrivo, message.Partenza));
                        connection.Send(json);
                    }
                    catch (Exception ex)
                    {
                        connection.Send(JsonConvert.SerializeObject(new Mex.ServerBuses.Response { Error = new List<string> { ex.Message }, Status = false })); ;
                        Console.WriteLine("\n--------Errore--------\n" + ex.Message + "\n--------Errore--------\n");
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
        public static ServerBuses.Response getmessage(Fermata Arr,Fermata Part)
        {
            
            
            var PercorsiAndataPossibili = Percorsi.Where(x=>x.elefermateandata.Contains(Part) && x.elefermateandata.GetRange(x.elefermateandata.IndexOf(Part),x.elefermateandata.Count()- x.elefermateandata.IndexOf(Part)).Contains(Arr));
            var PercorsiRitornoPossibili= Percorsi.Where(x => x.elefermateritorno.Contains(Part) && x.elefermateritorno.GetRange(x.elefermateritorno.IndexOf(Part), x.elefermateritorno.Count()  - x.elefermateritorno.IndexOf(Part)).Contains(Arr));
            if (PercorsiAndataPossibili.Count() == 0 && PercorsiRitornoPossibili.Count()==0)
            {
                throw new Exception("Non sono stati trovati percorsi conformi");
            }
            List<Bus> BusGiusti = new List<Bus>();
            BusGiusti = Bus.Where(x => PercorsiAndataPossibili.Contains(x.percorso) || PercorsiRitornoPossibili.Contains(x.percorso)).ToList();
            //non si tiene conto di andata e ritorno perchè non si sa il pullman dove sia
            //si da per scontato inoltre che un pullman faccia sempre lo stesso percorso normalmente (in caso si va a cambare il suo percorso)


            return new ServerBuses.Response { buses=BusGiusti,Status=true};
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
        public static bool CaricaPercorsi(ref List<Percorso> percorsi, string path)
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
