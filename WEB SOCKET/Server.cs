using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using Fleck;

namespace WEBSOCKET_SERVER
{
    class Server
    {
        static void Main(string[] args)
        {
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
                connection.OnMessage = message =>
                {
                    if (message == "True")
                        connection.Send("Pacchetto");

                };

                connection.OnError = exception =>
                  Console.WriteLine($"OnError {exception.Message}");
                connection.OnPing = bytes =>
                  Console.WriteLine("OnPing");
                connection.OnPong = bytes =>
                {
                    Console.WriteLine("OnPong");
                    connection.Close();
                };
            });
            string f = default;
            while (f != "quit")
            {
                f = Console.ReadLine();

            }
        }
    }
}
