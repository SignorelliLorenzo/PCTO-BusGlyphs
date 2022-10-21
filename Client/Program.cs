// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using WebSocket4Net;
using Newtonsoft.Json;
using System.Device.Location;
namespace CLientBus
{
    public static class Program
    {
        static void Errore(string ex)
        {
            Console.WriteLine("------------------------------------------ERRORE--------------------------------------------");
            Console.WriteLine(ex);
            int x = 0;
            while(x< (32 - ex.Length) / 2)
            {
               
            }
            Console.Write(ex);
            Console.WriteLine("--------------------------------------------------------------------------------------------");
        }
        static void Main(string[] args)
        {
            string IdBus = "2000";
            int _timeout = 2000;
            bool statoClientMappa=false;
            bool statoClientStato = false;
            WebSocket ClientMappa = new WebSocket("ws://127.0.0.1:8383");
            WebSocket ClientStato = new WebSocket("ws://127.0.0.1:8383");
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            GeoCoordinate coord = watcher.Position.Location;
            ClientMappa.Closed += (a, b) =>
            {
                if (statoClientMappa)
                    Errore("Connessione improvvisamente interrotta");
            };

            ClientMappa.Open();

            int timeout = 0;
            while (ClientMappa.State == WebSocketState.Connecting && timeout < _timeout)
            {
                Thread.Sleep(1);
                timeout++;
            }

            if (ClientMappa.State != WebSocketState.Open)
                throw new Exception("Connessione fallita per ClientMappa");

            statoClientMappa = true;
            ClientStato.Closed += (a, b) =>
            {
                if (statoClientStato)
                    Errore("Connessione improvvisamente interrotta");
            };

            ClientStato.Open();
            while (ClientStato.State == WebSocketState.Connecting && timeout < _timeout)
            {
                Thread.Sleep(1);
                timeout++;
            }

            if (ClientStato.State != WebSocketState.Open)
                throw new Exception("Connessione fallita per ClientMappa");
            statoClientStato = true;
            while(true)
            {
                Thread.Sleep(2000);
                ClientMappa.Send("gps%"+IdBus+"%"+JsonConvert.DeserializeObject<Coordinate>())
            }
        }
    }
}
