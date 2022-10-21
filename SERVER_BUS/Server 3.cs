using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bus_Percorsi;
using Mex;
namespace SERVER_BUS
{
    public class ServerBus
    {
        static string indirizzo = "ws://127.0.0.1:8282";
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
            var bus = Bus.Find(x => x.Id == "1000");
            coordinatepullman["1000"] = new BusState(bus.Id, bus.percorso, new Coordinate(45.6964538, 9.6686629), bus.percorso.elefermateandata[0], true);
            bus = Bus.Find(x => x.Id == "1001");
            coordinatepullman["1001"] = new BusState(bus.Id, bus.percorso, new Coordinate(45.69334538, 9.6677629), bus.percorso.elefermateandata[5], true);
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
                            var Response = OnStandardMessage(message, coordinatepullman);
                            //var bus = coordinatepullman.Where(p => p.Value.BusName == risposta).First().Value;
                            //var json = JsonConvert.SerializeObject(bus);
                            connection.Send(JsonConvert.SerializeObject(Response));
                        }
                        else
                        {
                            connection.Send("true");
                        }
                    }
                    catch (Exception ex)
                    {
                        connection.Send(JsonConvert.SerializeObject(new ServerNearBus.Response { Status = false, Error = new List<string> { ex.Message } }));
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
        public static ServerNearBus.Response OnStandardMessage(string message, IDictionary<string, BusState> pullmancoordinate)
        {
           
            Fermata fermataattuale = null;
            List<Bus> elebus = default;
            try
            {
                var Request = JsonConvert.DeserializeObject<ServerNearBus.Request>(message);
                fermataattuale =BusState.ArchivioCoordinate_Fermate.Find(x=>x.Id==Request.Attuale.Id);
                elebus = Request.buses.Where(x=> pullmancoordinate.Keys.Contains(x.Id)).ToList();
            }
            catch
            {
                throw new Exception("Invalid message format");
            }
            int x = 0;

            //bool startconta = false;
            //int distanza = 0;
            //string codicefinale = default;
            int distanza = -1;
            int res=0;
            Bus result = null;
            foreach (Bus bus in elebus.Where(x=>x.Andata== pullmancoordinate[x.Id].andata))
            {
                res=BusDistance(fermataattuale, pullmancoordinate[bus.Id]);
                if((distanza==-1||res<distanza)&&(res != -1))
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
                throw new Exception( "Nessun pullman disponibile in tempi brevi");
            }

            return new ServerNearBus.Response { bus=result,Status=true};
        }
        public static int BusDistance(Fermata fermata, BusState State)
        {
            Fermata[] elefermate;
            int x = 0;
            if(State.andata)
            {
                elefermate = State.BusPath.elefermateandata.Where(x=>State.BusPath.elefermateandata.FindIndex(z=>z.Id==State.NextStop.Id)<=State.BusPath.elefermateandata.FindIndex(z => z.Id == x.Id)).ToArray();
                while(x<elefermate.Count())
                {
                    if(elefermate[x].Id==fermata.Id)
                    {
                        return x;
                    }
                    x++;
                }
            }
            else
            {
                elefermate = State.BusPath.elefermateritorno.Where(x => State.BusPath.elefermateritorno.FindIndex(z => z.Id == State.NextStop.Id) <= State.BusPath.elefermateritorno.FindIndex(z => z.Id == x.Id)).ToArray();
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
