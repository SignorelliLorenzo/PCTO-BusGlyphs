using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bus_Percorsi;
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
            //coordinatepullman["A1"] = new BusState("A1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6, 0 }, new List<int> { 0, 6, 2, 3 }), new Coordinate(45.6964538, 9.6686629), 3, true);
            //coordinatepullman["B1"] = new BusState("B1", new Percorso("Milano-Brescia", new List<int> { 3, 7, 5, 2 }, new List<int> { 2, 5, 7, 3 }), new Coordinate(45.696926, 9.6690978), 2, false);
            //coordinatepullman["C1"] = new BusState("C1", new Percorso("Como-Bergamo", new List<int> { 7, 6, 5, 3 }, new List<int> { 3, 5, 6, 7 }), new Coordinate(45.6975549, 9.6667936), 6, false);
            //coordinatepullman["D1"] = new BusState("D1", new Percorso("Como-Bergamo", new List<int> { 7, 6, 5, 3 }, new List<int> { 3, 5, 6, 7 }), new Coordinate(45.7008704, 9.6655066), 7, true);
            //coordinatepullman["E1"] = new BusState("E1", new Percorso("Telgate-Bergamo", new List<int> { 2, 5, 6, 0 }, new List<int> { 0, 6, 5, 2 }), new Coordinate(45.7032898, 9.6775359), 0, true);


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
                        else
                        {
                            connection.Send("true");
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
                        var k = new BusState(BusList.Where(x => x.Id == Infos[1]).FirstOrDefault().Id, BusList.Where(x => x.Id == Infos[1]).FirstOrDefault().percorso, JsonConvert.DeserializeObject<Coordinate>(Infos[2]));
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
            Fermata fermataattuale = null;
            List<Bus> elebus = default;
            try
            {
                fermataattuale =BusState.ArchivioCoordinate_Fermate.Find(x=>x.Id==infos[0]);
                elebus = JsonConvert.DeserializeObject<List<Bus>>(infos[1]);
            }
            catch
            {
                return "Invalid message format";
            }
            int x = 0;

            //bool startconta = false;
            //int distanza = 0;
            //string codicefinale = default;
            int distanza = -1;
            int res=0;
            Bus result = null;
            foreach (Bus bus in elebus)
            {
                res=BusDistance(fermataattuale, pullmancoordinate[bus.Id]);
                if(res!=-1&&res<distanza)
                {
                    distanza = res;
                    result = bus;
                }

                #region OLD
                //var attuale = pullmancoordinate.Where(b => b.Key == bus.Id).First().Value;
                //x = 0;
                //startconta = false;
                //if (attuale.andata)
                //{
                //    foreach (var item in attuale.BusPath.elefermateandata)
                //    {
                //        if (item == attuale.LastStop)
                //        {
                //            startconta = true;
                //        }
                //        if (startconta)
                //        {
                //            x++;
                //            if (item == fermataattuale)
                //            {
                //                if (x < distanza || distanza == 0)
                //                {
                //                    distanza = x;
                //                    codicefinale = attuale.BusName;
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}
                //else if (!attuale.andata)
                //{
                //    foreach (var item in attuale.BusPath.elefermateritorno)
                //    {
                //        if (item == attuale.LastStop)
                //        {
                //            startconta = true;
                //        }
                //        if (startconta)
                //        {
                //            x++;
                //            if (item == fermataattuale)
                //            {
                //                if (x < distanza || distanza == 0)
                //                {
                //                    distanza = x;
                //                    codicefinale = attuale.BusName;
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}
                #endregion
            }

            if (result == null)
            {
                return "Nessun pullman disponibile in tempi brevi";
            }

            return result.Id;
        }
        public static int BusDistance(Fermata fermata, BusState State)
        {
            Fermata[] elefermate;
            int x = 0;
            if(State.andata)
            {
                elefermate = State.BusPath.elefermateandata.GetRange(State.BusPath.elefermateandata.IndexOf(State.NextStop), State.BusPath.elefermateandata.Count()- State.BusPath.elefermateandata.IndexOf(State.NextStop)).ToArray();
                while(x<elefermate.Count())
                {
                    if(elefermate[x]==fermata)
                    {
                        return x;
                    }
                    x++;
                }
            }
            else
            {
                elefermate = State.BusPath.elefermateritorno.GetRange(State.BusPath.elefermateritorno.IndexOf(State.NextStop), State.BusPath.elefermateandata.Count() - State.BusPath.elefermateandata.IndexOf(State.NextStop)).ToArray();
                while (x < elefermate.Count())
                {
                    if (elefermate[x] == fermata)
                    {
                        return x;
                    }
                    x++;
                }
            }
            return -1;
            
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
