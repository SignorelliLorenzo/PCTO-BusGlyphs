using System;
using Fleck;
using Moq;
using System.Linq;
using System.Collections.Generic;
using Creatore_archivio_pcto;
using Newtonsoft.Json;

namespace SERVER_IDGLYPHS
{
    interface IAforge
    {
        public string Getid();
    }
    public class mex
    {
        public int codfermata { get; set; }
        public List<string> Percorsi = new List<string>();
        private string _direzione;
        public string direzione
        {
            get { return direzione; }
            set{
                if (value != "ANDATA" && value != "RITORNO" && value != "ENTRAMBE")
                {
                    throw new Exception("Inserire uno stato valido");
                }
                _direzione = value;
            }

        }
        
    }


    class IDGLYPHS
    {
        static Dictionary<string,int>  CODGlyphs=new Dictionary<string, int>();
        static List<Percorso> Percorsi = new List<Percorso>();
        static void Main(string[] args)
        {
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

                    connection.Send(getmessage(Aforge.Object.Getid()));
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

        public static string getmessage(string cod)
        {
            int codicefermata = -1;
            if(!(CODGlyphs.TryGetValue(cod,out codicefermata)))
            {
                throw new Exception("Fermata inesistente");
            }
           
            string message;
            mex NRmessage = new mex();
            NRmessage.codfermata = codicefermata;
            foreach (var item in Percorsi)
            {
                foreach(var v in item.elefermateandata)
                {
                    if(v==codicefermata)
                    {
                        NRmessage.Percorsi.Add(item.nome);
                        NRmessage.direzione = "ANDATA";
                        break;
                    }
                }
                foreach (var v in item.elefermateritorno)
                {
                    if (v == codicefermata)
                    {
                        if(NRmessage.direzione== "ANDATA")
                        {
                            NRmessage.direzione = "ENTRAMBE";
                            break;
                        }
                        NRmessage.Percorsi.Add(item.nome);
                        NRmessage.direzione = "RITORNO";
                        break;
                    }
                }

            }
            message = JsonConvert.SerializeObject(NRmessage);
            return message;
        }
    }
}
