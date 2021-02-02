using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.WebSockets;
using System.Threading;



namespace GlyphsBus.WEB_SOCKET_CLIENT
{
    class Client:IDisposable
    {
        private string _coordinate;
        public string coordinate
        {
            get
            {
                return _coordinate;
            }
        }
        private void Funzione(string indirizzo)
        {
            //Crea la connessione
            var websocketClient = new ClientWebSocket();
            var cancellationToken = new CancellationTokenSource();


            var connection = websocketClient.ConnectAsync(
              new Uri("ws://127.0.0.1:8181"),
              cancellationToken.Token);

            //Codice eseguito se la connessione ha avuto successo
            connection.ContinueWith(async tsk =>
            {
                var buffer = new byte[128];

                //Richieste periodiche
                while (true)
                {
                    Thread.Sleep(3000);
                    Array.Clear(buffer, 0, 127);
                    await websocketClient.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes("Richiesta")),
                    WebSocketMessageType.Text,
                    true,
                    cancellationToken.Token);

                    await websocketClient.ReceiveAsync(
                    new ArraySegment<byte>(buffer), cancellationToken.Token);


                    _coordinate = Encoding.UTF8.GetString(buffer);
                }

            });
        }
        public Client(string indirizzo)
        {
            Funzione(indirizzo);
        }


        public virtual void Dispose()
        {
            //// Chiude la connessione
            //websocketClient.CloseAsync(
            //   WebSocketCloseStatus.NormalClosure,
            //   String.Empty,
            //   cancellationToken.Token);

            //cancellationToken.Cancel();
        }

    }
}
