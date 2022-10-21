using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using WebSocket4Net;
using System.Threading;
using Newtonsoft.Json;
using SERVER_BUS;

namespace ClientBus
{
    public static class Program
    {
        static void Errore(string ex)
        {
            Console.WriteLine("------------------------------------------ERRORE--------------------------------------------");
            Console.WriteLine(ex);
            int x = 0;
            while (x < (32 - ex.Length) / 2)
            {

            }
            Console.Write(ex);
            Console.WriteLine("--------------------------------------------------------------------------------------------");
        }
        static void Main(string[] args)
        {
            string IdBus = "1000";
            int _timeout = 2000;
            bool statoClientMappa = false;
            bool statoClientStato = false;
            WebSocket ClientMappa = new WebSocket("ws://127.0.0.1:8383");
            WebSocket ClientStato = new WebSocket("ws://127.0.0.1:8282");
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            Console.ReadKey();
            // Do not suppress prompt, and wait 1000 milliseconds to start.
            var result=watcher.TryStart(false, TimeSpan.FromMilliseconds(3000));
            if(!result)
            {
                throw new Exception();
            }
            while (watcher.Status != GeoPositionStatus.Ready) ;
            GeoCoordinate coord = default;
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
            while (true)
            {
                Thread.Sleep(2000);
                while (coord==null || coord.IsUnknown)
                {
                    coord = watcher.Position.Location;
                }
                ClientMappa.Send("gps%" + IdBus + "%" + JsonConvert.SerializeObject(new Coordinate(coord.Latitude, coord.Longitude)));
                coord = null;
            }
        }
    }
}
