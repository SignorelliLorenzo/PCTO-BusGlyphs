﻿using System;
using Fleck;
using Creatore_archivio_pcto;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Messages;
using System.Linq;

namespace SERVER_PERCORSO
{
    public class PERCORSO
    {
        static string indirizzo = "ws://127.0.0.1:8181";
        static void Main(string[] args)
        {
            
            List<Bus> Bus = new List<Bus>();
            if(!CaricaBus(ref Bus, "BusList.json"))
            {
                Console.WriteLine("ERRORE: Non sono riuscito a caricare i bus");
                return;
            }
            var websocketServer = new WebSocketServer(indirizzo);
            Console.WriteLine("--------------------Server Percorso--------------------");
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
                connection.OnMessage = msg =>
                {
                    
                    try
                    {
                        mexdestinazione message = JsonConvert.DeserializeObject<mexdestinazione>(msg);
                        //connection.Send(JsonConvert.SerializeObject(getmessage(message.Percorsi, Bus, message.Destinazione)));
                        var json = JsonConvert.SerializeObject(getmessage(message.Percorsi, Bus, message.Destinazione));
                        json = $"{message.codfermata}%{json}";
                        connection.Send(json);
                    }
                    catch (Exception ex)
                    {
                        connection.Send("!%-ERRORE: " + ex.Message);
                        Console.WriteLine("/n--------Errore--------/n" + ex.Message + "/n--------Errore--------/n");
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
        public static List<Bus> getmessage(List<Percorso> Percorsi,List<Bus> Bus, int Destinazione)
        {
            if(Percorsi.Count==0 || Bus.Count==0)
            {
                throw new Exception("Dati non disponibili");
            }
            List<Percorso> Percorsigiusti = new List<Percorso>();
            Percorsigiusti=Percorsi.Where(x => x.elefermateandata.Contains(Destinazione) || x.elefermateritorno.Contains(Destinazione)).ToList();
            if(Percorsigiusti.Count==0)
            {
                throw new Exception("Non sono stati trovati percorsi conformi");
            }
            return Bus.Where(x => Percorsigiusti.Contains(x.percorso)).ToList();
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
