using Creatore_archivio_pcto;
using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SERVER_BUS
{
    public class ServerBus
    {
        static string indirizzo = "ws://192.168.1.126:8282";
        static void Main(string[] args)
        {
            Dictionary<string, BusState> coordinatepullman = new Dictionary<string, BusState>();
            List<Bus> Bus = new List<Bus>();
            Console.WriteLine("--------------------Server Bus--------------------");
            if (!CaricaBus(ref Bus, "BusList.json"))
            {
                Console.WriteLine("ERRORE: Non sono riuscito a caricare i bus");
                Console.ReadKey();
                return;
            }
            var websocketServer = new WebSocketServer(indirizzo);
            coordinatepullman["A1"] = new BusState("A1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 3, true);
            coordinatepullman["B2"] = new BusState("B2", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 2, false);
            coordinatepullman["C2"] = new BusState("C2", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 6, false);
            coordinatepullman["D3"] = new BusState("D3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 2, true);
            coordinatepullman["B1"] = new BusState("B1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 2, true);


            //{
            //    new Bus("A1",new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })),
            //    new Bus("A2",new Percorso("Milano-Brescia", new List<int> { 0, 5, 7 }, new List<int> { 7, 5, 0 })),
            //    new Bus("B1",new Percorso("Milano-Bergamo", new List<int> { 3, 5, 6 }, new List<int> { 6, 5, 3 })),
            //    new Bus("C1",new Percorso("Telgate-Bergamo", new List<int> { 3, 0, 5 }, new List<int> { 5, 0, 3 })),
            //    new Bus("D1",new Percorso("Telgate-Bergamo", new List<int> { 3, 0, 5 }, new List<int> { 5, 0, 3 })),
            //};
            //StreamWriter miofile = new StreamWriter("BusList.json");
            //miofile.Write(JsonConvert.SerializeObject(Bus,Formatting.Indented));
            //miofile.Close();


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
                connection.OnMessage = message =>
                {

                    try
                    {
                        if (!OnGpsMessage(coordinatepullman, message, Bus))
                        {
                            var codicebus = OnStandardMessage(message, coordinatepullman);
                            //var bus = coordinatepullman.Where(p => p.Value.BusName == risposta).First().Value;
                            //var json = JsonConvert.SerializeObject(bus);
                            connection.Send(codicebus);
                        }
                    }
                    catch (Exception ex)
                    {
                        connection.Send("!%-ERRORE: " + ex.Message);
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
        public static bool OnGpsMessage(IDictionary<string, BusState> CoordianteDyctionary, string message, List<Bus> BusList)
        {
            if (message.StartsWith("gps%"))
            {
                var Infos = message.Split("%");
                if (CoordianteDyctionary.ContainsKey(Infos[1]))
                {
                    CoordianteDyctionary[Infos[1]].currentposition = JsonConvert.DeserializeObject<Coordinate>(Infos[2]);

                }
                else
                {
                    try
                    {
                        var k = new BusState(Infos[1], BusList.Where(x => x.codice == Infos[1]).FirstOrDefault().percorso, JsonConvert.DeserializeObject<Coordinate>(Infos[2]));
                        CoordianteDyctionary[k.BusName] = k;
                    }
                    catch
                    {
                        throw new Exception("Invalid message format");
                    }
                }
                return true;

            }
            return false;
        }
        public static string OnStandardMessage(string message, IDictionary<string, BusState> pullmancoordinate)
        {
            var infos = message.Split("%");
            int fermataattuale = default;
            List<Bus> elebus = default;
            try
            {
                fermataattuale = int.Parse(infos[0]);
                elebus = JsonConvert.DeserializeObject<List<Bus>>(infos[1]);
            }
            catch
            {
                return "Invalid message format";
            }
            int x = 0;

            bool startconta = false;
            int distanza = 0;
            string codicefinale = default;
            foreach (Bus bus in elebus)
            {
                var attuale = pullmancoordinate.Where(b => b.Key == bus.codice).First().Value;
                x = 0;
                startconta = false;
                if (attuale.andata)
                {
                    foreach (var item in attuale.BusPath.elefermateandata)
                    {
                        if (item == attuale.LastStop)
                        {
                            startconta = true;
                        }
                        if (startconta)
                        {
                            x++;
                            if (item == fermataattuale)
                            {
                                if (x < distanza || distanza == 0)
                                {
                                    distanza = x;
                                    codicefinale = attuale.BusName;
                                }
                                break;
                            }
                        }
                    }
                }
                else if (!attuale.andata)
                {
                    foreach (var item in attuale.BusPath.elefermateritorno)
                    {
                        if (item == attuale.LastStop)
                        {
                            startconta = true;
                        }
                        if (startconta)
                        {
                            x++;
                            if (item == fermataattuale)
                            {
                                if (x < distanza || distanza == 0)
                                {
                                    distanza = x;
                                    codicefinale = attuale.BusName;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            if (distanza == 0)
            {
                return "Nessun pullman disponibile in tempi brevi";
            }

            return codicefinale;
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
