using System;
using Xunit;
using System.Collections.Generic;
using SERVER_BUS;
using Creatore_archivio_pcto;
using Newtonsoft.Json;

namespace Server3Testing
{
    public class Server3_Test
    {
        [Fact]
        public void OnGPSMessage_NotGPSMessage()
        {
            
            List<Bus> BusList = new List<Bus>();
            Dictionary<string, BusState> CoordinateDyctionary = new Dictionary<string, BusState>();
            string message = "";
            var risposta = ServerBus.OnGpsMessage(ref CoordinateDyctionary, message, BusList);
            Assert.False(risposta);
        }
        [Fact]
        public void OnGPSMessage_NewBus()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("beta",new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            Dictionary<string, BusState> CoordinateDyctionarya = new Dictionary<string, BusState>();

            var json = JsonConvert.SerializeObject(new Coordinate(1,2)); 
            string message = "gps%beta%"+json;
            var risposta = ServerBus.OnGpsMessage(ref CoordinateDyctionarya, message, BusList);
            Assert.True(risposta);
            Assert.Single(CoordinateDyctionarya);
        }
        [Fact]
        public void OnGPSMessage_SameBus()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("alfa", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            Dictionary<string, BusState> CoordinateDyctionaryb = new Dictionary<string, BusState>();
            CoordinateDyctionaryb["alfa"] = new BusState("alfa", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3));

            var json = JsonConvert.SerializeObject(new Coordinate(1, 2));
            string message = "gps%alfa%" + json;
            var risposta = ServerBus.OnGpsMessage(ref CoordinateDyctionaryb, message, BusList);
            Assert.True(risposta);
            Assert.Single(CoordinateDyctionaryb);
            Assert.Equal(new Coordinate(1,2).x,CoordinateDyctionaryb["alfa"].currentposition.x);
            Assert.Equal(new Coordinate(1, 2).y, CoordinateDyctionaryb["alfa"].currentposition.y);
        }
    }
}
