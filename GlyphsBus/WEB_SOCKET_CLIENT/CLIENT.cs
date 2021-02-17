using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using WebSocket4Net;

namespace WEBSOCKET_CLIENT
{
    public class WebSocket_Client : IDisposable
    {
        private WebSocket Client;

        private int _timeout;

        private bool stato;

        private string _coordinate;
        /// <summary>
        /// Valore coordinate in tempo reale
        /// </summary>
        public string coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        //Funzioni
        private void MessaggioRicevuto(object a, MessageReceivedEventArgs b)
        {
            if (this.stato == true)
            {
                this.Client.Send(stato.ToString());
                this._coordinate = b.Message;
            }
            else if (this.stato == false)
                this.Client.Send(stato.ToString());
        }

        private void Initialize(string indirizzo)
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
            this.Client.Send(stato.ToString());

        }
        /// <summary>
        /// Inizializza la connessione e inizia la ricezione dati
        /// </summary>
        /// <param name="indirizzo">Indirizzo ip del server web</param>
        /// <param name="timeout">Tempo di attesa dell connessione, in alternativa eccezione</param>
        public WebSocket_Client(string indirizzo, int timeout = 1000)
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
        ~WebSocket_Client()
        {
            Dispose(false);
        }


    }
}
