﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Fleck;
using Newtonsoft.Json;

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
            }
        }
        public CoordinateMappa(Coordinate coordinate)
        {
            this.x = (decimal)coordinate.x;
            this.y = (decimal)coordinate.y;
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
    class ImmagineMappa
    {      
        public static byte[] CreaImmagine(Coordinate coordinate)
        {
            CoordinateMappa coordinatemappa = new CoordinateMappa(coordinate);

            //Elaborazione
            byte[] mappa;
            string latitudine = coordinatemappa.x.ToString();
            string longitudine = coordinatemappa.y.ToString();
            string zoom = "12";
            string larghezza = "600";
            string altezza = "350";
            string chiave = "AIzaSyDnc6Fone6eXBZ4y8w7IC-hSvjMPQuRfwY";
            string url = @"http://maps.googleapis.com/maps/api/staticmap?center=" + latitudine + "," + longitudine + "&zoom=" + zoom + "&size=" + larghezza + "x" + altezza + "&maptype=roadmap&markers=color:red%7Clabel:%7C" + latitudine + "," + longitudine + "&sensor=false&key=" + chiave;

            using (WebClient wc = new WebClient())
            {
                mappa = wc.DownloadData(url);
            }
            //
            return mappa;
        }
        static void Main(string[] args)
        {          
            object a = new object();
            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
            Dictionary<string, Coordinate> coordinatepullman = new Dictionary<string, Coordinate>();
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
                    if(message.StartsWith("gps%"))
                    {
                        var risposta = message.Split("%");
                        coordinatepullman[risposta[1]] = JsonConvert.DeserializeObject<Coordinate>(risposta[2]);
                        return;
                    }
                    Coordinate coordinate = coordinatepullman[message];
                    var immagine=CreaImmagine(coordinate);
                    connection.Send(immagine);
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