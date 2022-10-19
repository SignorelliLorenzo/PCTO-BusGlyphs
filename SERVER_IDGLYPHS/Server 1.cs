
using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bus_Percorsi;
using ZXing;
using ZXing.Common;
using ZXing.CoreCompat.System.Drawing;
using System.Drawing;

namespace SERVER_IDGLYPHS
{

    public class IDGLYPHS
    {
        static string indirizzo = "ws://127.0.0.1:8080";
        static Dictionary<string, string> CODGlyphs = new Dictionary<string, string>();
        static List<Percorso> Percorsi = new List<Percorso>();
        static List<Fermata> Fermate = new List<Fermata>();
        private static string FindQrCodeInImage(Bitmap bmp)
        {
            //decode the bitmap and try to find a qr code
            var source = new BitmapLuminanceSource(bmp);
            var bitmap = new BinaryBitmap(new HybridBinarizer(source));
            var result = new MultiFormatReader().decode(bitmap);


            //no qr code found in bitmap
            if (result == null)
            {


                throw new Exception("Glifo non trovato");
            }

            //return the found qr code text
            return result.Text;
        }
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
            if (!CaricaFermate(ref Fermate, "Fermate.json"))
            {
                Console.WriteLine("ERRORE: Fermate non caricate");
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
                        System.Drawing.Bitmap bmp = default;
                        bmp = (Bitmap)Image.FromStream(new MemoryStream(Convert.FromBase64String(request.img)));
                        var result=FindQrCodeInImage(bmp);
                        connection.Send(getmessage(result, Percorsi, CODGlyphs));
                        
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
        public static bool CaricaFermate(ref List<Fermata> Fermate, string path)
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
                Fermate = JsonConvert.DeserializeObject<List<Fermata>>(jsonString);
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

            Mex.ServerIMG.Response NRmessage = new Mex.ServerIMG.Response();
            NRmessage.fermata = Fermate.Where(x => x.Id == codicefermata).First();
            NRmessage.ArriviProb = new List<Fermata>();

            foreach (var percorso in percorsi.Where(x=>x.elefermateritorno.Contains(NRmessage.fermata, new SameId()) || x.elefermateandata.Contains(NRmessage.fermata, new SameId())))
            {

                NRmessage.ArriviProb.AddRange(percorso.elefermateandata.Where(x => NRmessage.ArriviProb.Contains(NRmessage.fermata, new SameId()) && percorso.elefermateandata.IndexOf(x)> percorso.elefermateandata.FindIndex(x=>x.Id==NRmessage.fermata.Id)));
                NRmessage.ArriviProb.AddRange(percorso.elefermateritorno.Where(x => !NRmessage.ArriviProb.Contains(NRmessage.fermata, new SameId()) && percorso.elefermateritorno.IndexOf(x) > percorso.elefermateritorno.FindIndex(x => x.Id == NRmessage.fermata.Id)));


            }
            if (NRmessage.ArriviProb.Count == 0)
                throw new Exception("Fermata non trovata");

            NRmessage.Status =true ;

            return JsonConvert.SerializeObject(NRmessage);
        }
    }
}
