using System;
using Fleck;
using Moq;
namespace SERVER_IDGLYPHS
{
    interface IAforge
    {
        public string Getid();
    }
    class IDGLYPHS
    {
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
                    connection.Send(Aforge.Object.Getid());
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
    }
}
