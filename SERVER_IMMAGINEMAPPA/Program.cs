using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Fleck;

namespace SERVER_IMMAGINEMAPPA
{
    public class TabellaImmagineMappa
    {
        private string _codice;
        public string codice
        {
            get
            {
                return _codice;
            }
        }

        private decimal _x;
        public decimal x
        {
            get
            {
                return _x;
            }
            set
            {
                if(value<-90||value>90)
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
    }
    class ImmagineMappa
    {
        static List<TabellaImmagineMappa> lista = new List<TabellaImmagineMappa>();
        public static string CreaImmagine(string messaggio)
        {  
            var bus=lista.FirstOrDefault(l => l.codice == messaggio);
            string immagine = default;
            decimal x = bus.x;
            decimal y = bus.y;
            //Elaborazione
            return immagine;
        }
        static void Main(string[] args)
        {
            List<TabellaImmagineMappa> lista = new List<TabellaImmagineMappa>();
            object a = new object();
            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
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
                    var immagine=CreaImmagine(message);
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
