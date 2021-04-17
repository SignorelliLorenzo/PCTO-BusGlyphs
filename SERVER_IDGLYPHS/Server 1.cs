﻿using System;
using Fleck;
using Moq;
using System.Linq;
using System.Collections.Generic;
using Creatore_archivio_pcto;
using Newtonsoft.Json;
using System.IO;

namespace SERVER_IDGLYPHS
{
    interface IAforge
    {
        public string Getid();
    }
    public class mex
    {
        public int codfermata { get; set; }
        public List<Percorso> Percorsi = new List<Percorso>();
    }


    public class IDGLYPHS
    {
        static Dictionary<string,int>  CODGlyphs=new Dictionary<string, int>();
        static List<Percorso> Percorsi = new List<Percorso>();

        static void Main(string[] args)
        {
            //var file = new StreamWriter("CodGlifi");
            //Percorsi.Add(new Percorso("Milano-Bergamo",new List<int> { 3,2,6} , new List<int> { 6,2,3}));
            //Percorsi.Add(new Percorso("Milano-Brescia", new List<int> { 0, 5, 7 }, new List<int> { 7, 5, 0 }));
            //Percorsi.Add(new Percorso("Telgate-Bergamo", new List<int> { 3, 4, 5 }, new List<int> { 5, 4, 3 }));
            //Percorsi.Add(new Percorso("Bergamo-Brescia", new List<int> { 0, 7, 2}, new List<int> { 2, 7, 0 }));
            //Percorsi.Add(new Percorso("Como-Bergamo", new List<int> { 4, 0, 6 }, new List<int> { 6, 0, 4 }));
            //Percorsi.Add(new Percorso("Como-Brescia", new List<int> { 3, 0, 6 }, new List<int> { 6, 7, 3 }));
            //CODGlyphs.Add("das", 3);
            //CODGlyphs.Add("vdf", 2);
            //CODGlyphs.Add("qwe", 6);
            //CODGlyphs.Add("fas", 4);
            //CODGlyphs.Add("ers", 7);
            //CODGlyphs.Add("vfd", 5);
            //CODGlyphs.Add("pkl", 0);
            //string jason = JsonConvert.SerializeObject(CODGlyphs);
            //file.WriteLine(jason);
            //file.Close();
            //return;
            if (!CaricaCOD_Glyphs(ref CODGlyphs, "CodGlifi"))
            {
                Console.WriteLine("ERRORE: Codice glifi non caricato");
                return;
            }
                
            if(!CaricaPercorsi(ref Percorsi, "Percorsi"))
            {
                Console.WriteLine("ERRORE: Percorsi non caricati");
                return;
            }
            var Aforge = new Mock<IAforge>();
            
            var websocketServer = new WebSocketServer("ws://127.0.0.1:8181");
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
                connection.OnBinary = bytes =>
                {
                    Aforge.Setup(x => x.Getid()).Returns("ABC");
                    

                    connection.Send(getmessage(Aforge.Object.Getid(),Percorsi,CODGlyphs));
                    
                };

                connection.OnError = exception =>
                {
                    Console.WriteLine($"OnError {exception.Message}");
                };

            });
            string f = default;
            while (f != "quit")
            {
                f = Console.ReadLine();

            }

        }


        public static bool CaricaCOD_Glyphs(ref Dictionary<string, int> listainput, string path)
        {
            if(!File.Exists(path))
            {
                return false;
            }
            StreamReader file = new StreamReader(path);
            string jsonString = file.ReadToEnd();
            file.Close();
            try
            {
                listainput = JsonConvert.DeserializeObject<Dictionary<string,int>>(jsonString);  
            }
                catch 
            {
                
                return false;
            }
            return true;
        }
        public static bool CaricaPercorsi(ref List<Percorso> percorsi, string path)
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
                percorsi = JsonConvert.DeserializeObject<List<Percorso>>(jsonString);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string getmessage(string cod, List<Percorso> percorsi, Dictionary<string, int> codiceglifi)
        {
            int codicefermata = -1;
            if(!(codiceglifi.TryGetValue(cod,out codicefermata)))
            {
                return "ERRORE: Fermata inesistente";
            }
           
            string message;
            mex NRmessage = new mex();
            NRmessage.codfermata = codicefermata;
            NRmessage.Percorsi=percorsi.Where(x=> x.elefermateandata.Contains(codicefermata) || x.elefermateritorno.Contains(codicefermata)).ToList();
            if (NRmessage.Percorsi.Count==0)
                return "ERRORE: Fermata non trovata";

            message = JsonConvert.SerializeObject(NRmessage);
            
            return message;
        }
    }
}
