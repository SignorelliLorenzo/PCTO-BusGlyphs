using Bus_Percorsi;
using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Mex;

namespace SERVER_IMMAGINEMAPPA
{

    public class CoordinateMappa
    {
        private decimal _x;
        public decimal x
        {
            get
            {
                return _x;
            }
            set
            {
                if (value < -90 || value > 90)
                {
                    throw new Exception("Coordinata x non valida");
                }
                _x = value;
            }
        }
        private decimal _y;
        public decimal y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value < -180 || value > 180)
                {
                    throw new Exception("Coordinata y non valida");
                }
                _y = value;
            }
        }
        public CoordinateMappa(Coordinate coordinate)
        {
            this.x = Convert.ToDecimal(coordinate.x);
            this.y = Convert.ToDecimal(coordinate.y);
        }
    }
    public class Coordinate
    {
        public double x;
        public double y;
        public Coordinate(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class ImmagineMappa
    {
       

        static string indirizzo = "ws://127.0.0.1:8383";
        public static byte[] CreaImmagine(Coordinate coordinate)
        {
            byte[] mappa = default;
            try
            {
                CoordinateMappa coordinatemappa = new CoordinateMappa(coordinate);

                //Elaborazione

                string latitudine = coordinatemappa.x.ToString().Replace(",",".");
                string longitudine = coordinatemappa.y.ToString().Replace(",", ".");
                
                string zoom = "15";
                string larghezza = "600";
                string altezza = "350";
                string chiave = "e3a22e33b51049bf804743dcb4a093fc";
                //string url = @"http://maps.googleapis.com/maps/api/staticmap?center=" + latitudine + "," + longitudine + "&zoom=" + zoom + "&size=" + larghezza + "x" + altezza + "&maptype=roadmap&markers=color:red%7Clabel:%7C" + latitudine + "," + longitudine + "&sensor=false&key=" + chiave;
                string url = $"https://maps.geoapify.com/v1/staticmap?style=maptiler-3d&width={larghezza}&height={altezza}&center=lonlat:{longitudine},{latitudine}&zoom={zoom}&marker=lonlat:{longitudine},{latitudine};type:material;color:%23bb3f73;size:medium;icon:bus-alt;icontype:awesome&apiKey={chiave}";
                using (WebClient wc = new WebClient())
                {
                    mappa = wc.DownloadData(url);
                }
                return mappa;
            }
            catch 
            {
                return mappa;
            }

        }
        static void Main(string[] args)
        {
            object a = new object();
            var websocketServer = new WebSocketServer(indirizzo);
            Dictionary<string, Coordinate> coordinatepullman = new Dictionary<string, Coordinate>();
            coordinatepullman["1000"] = new Coordinate(45.6964538, 9.6686629);
            coordinatepullman["1001"] = new Coordinate(45.696926, 9.6690978);
            coordinatepullman["C1"] = new Coordinate(45.6975549, 9.6667936);
            coordinatepullman["D1"] = new Coordinate(45.7008704, 9.6655066);
            coordinatepullman["E1"] = new Coordinate(45.7032898, 9.6775359);

            Console.WriteLine("--------------------Server Immagine-Mappa--------------------");
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
                        if (message.StartsWith("gps%"))
                        {

                            var risposta = message.Split("%");
                            coordinatepullman[risposta[1]] = JsonConvert.DeserializeObject<Coordinate>(risposta[2]);
                            connection.Send("true");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        connection.Send("!%-ERRORE: " + ex.Message);
                        Console.WriteLine("\n--------Errore--------\n" + ex.Message + "\n--------Errore--------\n");
                    }
                    Coordinate coordinate;
                    byte[] immagine;
                    int z = 0;
                    while (connection.IsAvailable)
                    {
                        z++;
                        coordinate = coordinatepullman[JsonConvert.DeserializeObject<ServerPosition.Request>(message).bus.Id];
                        coordinate.x = coordinate.x + z * (0.0000000001);
                        coordinate.y=coordinate.y+ z * (0.0000000001);
                        immagine = CreaImmagine(coordinate);
                            if (immagine == default)
                            {
                            connection.Send(JsonConvert.SerializeObject(new ServerPosition.Response { Status = false, Error = new List<string> { "Errore nella richiesta della mappa" } }));
                            }
                            else
                            {
                                connection.Send(JsonConvert.SerializeObject(new ServerPosition.Response { Status=true,img=Convert.ToBase64String(immagine)}));
                            }

                            Thread.Sleep(3000);

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
