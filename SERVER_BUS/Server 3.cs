﻿using System;
using Fleck;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Creatore_archivio_pcto;

namespace SERVER_BUS
{
    public class ServerBus
    {


        static void Main(string[] args)
        {
            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
            Console.WriteLine("Server Bus");
            Dictionary<string, BusState> coordinatepullman = new Dictionary<string, BusState>();
            List<Bus> Bus = new List<Bus>();
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
                    if (!OnGpsMessage(ref coordinatepullman, message, Bus))
                    {
                        var codicebus = OnStandardMessage(message, ref coordinatepullman);
                        //var bus = coordinatepullman.Where(p => p.Value.BusName == risposta).First().Value;
                        //var json = JsonConvert.SerializeObject(bus);
                        connection.Send(codicebus);
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
        public static bool OnGpsMessage(ref Dictionary<string, BusState> CoordianteDyctionary, string message, List<Bus> BusList)
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

                    }
                }
                return true;

            }
            return false;
        }
        public static string OnStandardMessage(string message,ref Dictionary<string,BusState> pullmancoordinate)
        {
            var infos = message.Split("%");

            var fermataattuale = int.Parse(infos[0]);
            var elebus = JsonConvert.DeserializeObject<List<Bus>>(infos[1]);
            int x = 0;
            
            bool startconta = false;
            int distanza = 0;
            string codicefinale = default;
            foreach(Bus bus in elebus)
            {
                var attuale = pullmancoordinate.Where(b => b.Key == bus.codice).First().Value;
                x = 0;
                startconta = false;
                if(attuale.andata)
                {
                    foreach(var item in attuale.BusPath.elefermateandata)
                    {
                        if(item == attuale.LastStop)
                        {
                            startconta = true;
                        }
                        if(startconta)
                        {
                            x++;
                            if(item == fermataattuale)
                            {
                                if(x<distanza || distanza == 0)
                                {
                                    distanza = x;
                                    codicefinale = attuale.BusName;
                                }
                                break;
                            }
                        }
                    }
                }
                else if(!attuale.andata)
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

            if(distanza == 0)
            {
                return "Nessun pullman disponibile in tempi brevi";
            }

            return codicefinale;
        }
    }
}
