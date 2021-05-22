using Newtonsoft.Json;
using System;
using System.Threading;
using WebSocket4Net;

namespace ClientLibrary
{
    public class Client_Glifo_1 : IDisposable
    {
        private WebSocket Client;
        private bool _response;
        /// <summary>
        /// Notify when the resonse has arrived
        /// </summary>
        public bool response { get {return _response; } }
        private int _timeout;
        private byte[] _immage;
        public byte[] immage { get {return _immage; }
            set
            {
                _immage = value;
                Client.Send(_immage,0,_immage.Length);
            }
        
        
        }
        private bool stato;

        private string _json;
        /// <summary>
        /// Json contenente classe "mex"
        /// </summary>
        public string json
        {
            get
            {
                return _json;
            }
        }

        //Funzioni
        private void MessaggioRicevuto(object a, MessageReceivedEventArgs b)
        {
            if(b.Message != "Glifo non trovato")
            {
                _json = b.Message;
                _response = true;
            }
            

        }

        private void Initialize(string indirizzo)
        {
            _response = false;
            this.Client = new WebSocket(indirizzo);

            this.Client.MessageReceived += MessaggioRicevuto;

            this.Client.Closed += (a, b) =>
            {
                if (this.stato)
                    throw new Exception("Connessione improvvisamente interrotta");
            };

            this.Client.Open();

            int timeout = 0;
            while (this.Client.State == WebSocketState.Connecting && timeout < this._timeout)
            {
                Thread.Sleep(1);
                timeout++;
            }

            if (this.Client.State != WebSocketState.Open)
                throw new Exception("Connessione fallita");

            //Bool.Tostring() restituisce con la maiuscola, True, False
            

        }
        /// <summary>
        /// Inizializza la connessione e inizia la ricezione dati
        /// </summary>
        /// <param name="indirizzo">Indirizzo ip del server web</param>
        /// <param name="timeout">Tempo di attesa dell connessione, in alternativa eccezione</param>
        /// <param name="immagine">Immagine del glifo</param>
        public Client_Glifo_1(string indirizzo, int timeout = 1000)
        {
            this.stato = true;
            if (timeout <= 0)
                throw new Exception("Valore timeout non valido");
            _timeout = timeout;
            Initialize(indirizzo);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool isdisposed = false;

        protected virtual void Dispose(bool disposing)
        {

            if (isdisposed)
                return;
            else
            {
                this.stato = false;
                this.Client.Dispose();
                isdisposed = true;
            }
        }
        ~Client_Glifo_1()
        {
            Dispose(false);
        }


    }

    public class Client_Percorso_2 : IDisposable
    {
        private WebSocket Client;
        private bool _response;
        /// <summary>
        /// Notify when the resonse has arrived
        /// </summary>

        public bool response { get { return _response; } }
        private int _timeout;

        private bool stato;

        private string _json;
        /// <summary>
        /// Json contenente List<Bus>
        /// </summary>
        public string json
        {
            get
            {
                return _json;
            }
        }

        //Funzioni
        private void MessaggioRicevuto(object a, MessageReceivedEventArgs b)
        {
            _json = b.Message;
            _response = true;

        }

        private void Initialize(string indirizzo, mexdestinazione messaggio)
        {
            this.Client = new WebSocket(indirizzo);

            this.Client.MessageReceived += MessaggioRicevuto;

            this.Client.Closed += (a, b) =>
            {
                if (this.stato)
                    throw new Exception("Connessione improvvisamente interrotta");
            };

            this.Client.Open();

            int timeout = 0;
            while (this.Client.State == WebSocketState.Connecting && timeout < this._timeout)
            {
                Thread.Sleep(1);
                timeout++;
            }

            if (this.Client.State != WebSocketState.Open)
                throw new Exception("Connessione fallita");

            //Bool.Tostring() restituisce con la maiuscola, True, False
            this.Client.Send(JsonConvert.SerializeObject(messaggio));

        }
        /// <summary>
        /// Inizializza la connessione e inizia la ricezione dati
        /// </summary>
        /// <param name="indirizzo">Indirizzo ip del server web</param>
        /// <param name="timeout">Tempo di attesa dell connessione, in alternativa eccezione</param>
        /// <param name="messaggio">Messaggio contenente la destinazione</param>
        public Client_Percorso_2(string indirizzo, mexdestinazione messaggio, int timeout = 1000)
        {
            this.stato = true;
            if (timeout <= 0)
                throw new Exception("Valore timeout non valido");
            _timeout = timeout;

            Initialize(indirizzo, messaggio);

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool isdisposed = false;

        protected virtual void Dispose(bool disposing)
        {

            if (isdisposed)
                return;
            else
            {
                this.stato = false;
                this.Client.Dispose();
                isdisposed = true;
            }
        }
        ~Client_Percorso_2()
        {
            Dispose(false);
        }
    }

    public class Client_Bus_3 : IDisposable
    {
        private WebSocket Client;
        private bool _response;
        /// <summary>
        /// Notify when the resonse has arrived
        /// </summary>

        public bool response { get { return _response; } }
        private int _timeout;

        private bool stato;

        private string _CodiceBus;
        /// <summary>
        /// Codice del pullman desiderato
        /// </summary>
        public string CodiceBus
        {
            get
            {
                return _CodiceBus;
            }
        }

        //Funzioni
        private void MessaggioRicevuto(object a, MessageReceivedEventArgs b)
        {
            _CodiceBus = b.Message;
            _response = true;
        }

        private void Initialize(string indirizzo, string messaggio)
        {
            this.Client = new WebSocket(indirizzo);

            this.Client.MessageReceived += MessaggioRicevuto;

            this.Client.Closed += (a, b) =>
            {
                if (this.stato)
                    throw new Exception("Connessione improvvisamente interrotta");
            };

            this.Client.Open();

            int timeout = 0;
            while (this.Client.State == WebSocketState.Connecting && timeout < this._timeout)
            {
                Thread.Sleep(1);
                timeout++;
            }

            if (this.Client.State != WebSocketState.Open)
                throw new Exception("Connessione fallita");

            //Bool.Tostring() restituisce con la maiuscola, True, False

            this.Client.Send(messaggio);

        }
        /// <summary>
        /// Inizializza la connessione e inizia la ricezione dati
        /// </summary>
        /// <param name="indirizzo">Indirizzo ip del server web</param>
        /// <param name="timeout">Tempo di attesa dell connessione, in alternativa eccezione</param> 
        /// <param name="messaggio">Messaggio contenente i bus disponibili</param>
        public Client_Bus_3(string indirizzo, string messaggio, int timeout = 10000)
        {
            this.stato = true;
            if (timeout <= 0)
                throw new Exception("Valore timeout non valido");
            _timeout = timeout;

            Initialize(indirizzo, messaggio);

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool isdisposed = false;

        protected virtual void Dispose(bool disposing)
        {

            if (isdisposed)
                return;
            else
            {
                this.stato = false;
                this.Client.Dispose();
                isdisposed = true;
            }
        }
        ~Client_Bus_3()
        {
            Dispose(false);
        }
    }

    public class Client_Immagine_4 : IDisposable
    {
        private WebSocket Client;
        private bool _response;
        /// <summary>
        /// Notify when the resonse has arrived
        /// </summary>

        public bool response { get { return _response; } }
        private bool _newImage;
        public bool newImage
        {
            get
            {
                this._newImage = false;
                return _newImage;
            }


        }


        private int _timeout;

        private bool stato;

        private byte[] _immagine;
        /// <summary>
        /// Immagine della mappa
        ///  bmp = (Bitmap)Image.FromStream(new MemoryStream(bytes));
        /// </summary>
        public byte[] immagine
        {
            get
            {
                return _immagine;
            }
        }

        //Funzioni
        private void MessaggioRicevuto(object a, DataReceivedEventArgs b)
        {
            _immagine = b.Data;
            _newImage = true;

        }

        private void Initialize(string indirizzo, string messaggio)
        {
            this.Client = new WebSocket(indirizzo);
            this._newImage = false;
            this.Client.DataReceived += MessaggioRicevuto;
            // this.Client.MessageReceived += MessaggioRicevuto;

            this.Client.Closed += (a, b) =>
            {
                if (this.stato)
                    throw new Exception("Connessione improvvisamente interrotta");
            };

            this.Client.Open();

            int timeout = 0;
            while (this.Client.State == WebSocketState.Connecting && timeout < this._timeout)
            {
                Thread.Sleep(1);
                timeout++;
            }

            if (this.Client.State != WebSocketState.Open)
                throw new Exception("Connessione fallita");

            //Bool.Tostring() restituisce con la maiuscola, True, False

            this.Client.Send(messaggio);

        }
        /// <summary>
        /// Inizializza la connessione e inizia la ricezione dati
        /// </summary>
        /// <param name="indirizzo">Indirizzo ip del server web</param>
        /// <param name="timeout">Tempo di attesa dell connessione, in alternativa eccezione</param>
        /// <param name="messaggio">Messaggio contenente codice del pullman</param>
        public Client_Immagine_4(string indirizzo, string messaggio, int timeout = 10000)
        {
            this.stato = true;
            if (timeout <= 0)
                throw new Exception("Valore timeout non valido");
            _timeout = timeout;

            Initialize(indirizzo, messaggio);

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool isdisposed = false;

        protected virtual void Dispose(bool disposing)
        {

            if (isdisposed)
                return;
            else
            {
                this.stato = false;
                this.Client.Dispose();
                isdisposed = true;
            }
        }
        ~Client_Immagine_4()
        {
            Dispose(false);
        }
    }
}
