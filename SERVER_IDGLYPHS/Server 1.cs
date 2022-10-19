using AForgeFunctions;
using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Bus_Percorsi;
namespace SERVER_IDGLYPHS
{

    public class IDGLYPHS
    {
        static string indirizzo = "ws://127.0.0.1:8080";
        static Dictionary<string, string> CODGlyphs = new Dictionary<string, string>();
        static List<Percorso> Percorsi = new List<Percorso>();
        static List<Fermata> Fermate = new List<Fermata>();
        static void Main(string[] args)
        {

            if (!CaricaCOD_Glyphs(ref CODGlyphs, "CodGlifi.json"))
            {
                Console.WriteLine("ERRORE: Codice glifi non caricato");
                Console.ReadKey();
                return;
            }

            if (!CaricaPercorsi(ref Percorsi, "Percorsi.json"))
            {
                Console.WriteLine("ERRORE: Percorsi non caricati");
                Console.ReadKey();
                return;
            }
            var websocketServer = new WebSocketServer(indirizzo);
            Console.WriteLine("--------------------Server ID-Glifo--------------------");
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
                connection.OnMessage = Json => //si fa presente che si può anche inviare in bytes
                {
                    try
                    {
                        var request = JsonConvert.DeserializeObject<Mex.ServerIMG.Request>(Json);
                        Bitmap bmp = default;
                        bmp = (Bitmap)Image.FromStream(new MemoryStream(Convert.FromBase64String(request.img)));
                        if (!Funzioni.FindG(bmp))
                        {
                            throw new Exception("Glifo non trovato");
                        }
                        else
                        {
                            connection.Send(getmessage(Funzioni.FindGlyphName(bmp).ToString(), Percorsi, CODGlyphs));
                        }
                    }
                    catch (Exception ex)
                    {
                        connection.Send(JsonConvert.SerializeObject(new Mex.ServerIMG.Response { Error = new List<string> { ex.Message }, Status = false })); ;
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
        public static bool CaricaCOD_Glyphs(ref Dictionary<string, string> listainput, string path)
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
                listainput = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
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
        public static string getmessage(string cod, List<Percorso> percorsi, Dictionary<string, string> codiceglifi)
        {
            string codicefermata = "";
            if (!(codiceglifi.TryGetValue(cod, out codicefermata)))
            {
                throw new Exception("Fermata inesistente");
            }

            string message;
            Mex.ServerIMG.Response NRmessage = new Mex.ServerIMG.Response();
            NRmessage.fermata = Fermate.Where(x => x.Id == codicefermata).First();
            NRmessage.ArriviProb = new List<Fermata>();

            foreach (var percorso in percorsi.Where(x=>x.elefermateritorno.Contains(NRmessage.fermata)|| x.elefermateandata.Contains(NRmessage.fermata)))
            {

                NRmessage.ArriviProb.AddRange(percorso.elefermateandata.Where(x => !NRmessage.ArriviProb.Contains(NRmessage.fermata) && percorso.elefermateandata.IndexOf(x)> percorso.elefermateandata.IndexOf(NRmessage.fermata)));
                NRmessage.ArriviProb.AddRange(percorso.elefermateritorno.Where(x => !NRmessage.ArriviProb.Contains(NRmessage.fermata) && percorso.elefermateritorno.IndexOf(x) > percorso.elefermateritorno.IndexOf(NRmessage.fermata)));


            }
            if (NRmessage.ArriviProb.Count == 0)
                throw new Exception("Fermata non trovata");

            NRmessage.Status =true ;

            return JsonConvert.SerializeObject(NRmessage);
        }
    }
}
