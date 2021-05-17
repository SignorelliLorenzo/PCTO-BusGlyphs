using System;
using Xunit;
using System.Collections.Generic;
using SERVER_BUS;
using Creatore_archivio_pcto;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Server3Testing
{
    public class Server3_Test
    {
      
        //OnGPSMessage
        //**************************************
        //**************************************
        //**************************************
        //**************************************
        [Fact]
        public void OnGPSMessage_NotGPSMessage()
        {           
            List<Bus> BusList = new List<Bus>();
            ConcurrentDictionary<string, BusState> CoordinateDyctionary = new ConcurrentDictionary<string, BusState>();
            string message = "";
            var risposta = ServerBus.OnGpsMessage( CoordinateDyctionary, message, BusList);
            Assert.False(risposta);
        }
        [Fact]
        public void OnGPSMessage_NewBus()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("beta",new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            ConcurrentDictionary<string, BusState> CoordinateDyctionary = new ConcurrentDictionary<string, BusState>();

            var json = JsonConvert.SerializeObject(new Coordinate(1,2)); 
            string message = "gps%beta%"+json;
            var risposta = ServerBus.OnGpsMessage( CoordinateDyctionary, message, BusList);
            Assert.True(risposta);
            Assert.Single(CoordinateDyctionary);
        }
        [Fact]
        public void OnGPSMessage_SameBus()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("gamma", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            ConcurrentDictionary<string, BusState> CoordinateDyctionary = new ConcurrentDictionary<string, BusState>();
            CoordinateDyctionary["gamma"] = new BusState("gamma", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3));

            var json = JsonConvert.SerializeObject(new Coordinate(1, 2));
            string message = "gps%gamma%" + json;
            var risposta = ServerBus.OnGpsMessage( CoordinateDyctionary, message, BusList);
            Assert.True(risposta);
            Assert.Single(CoordinateDyctionary);
            Assert.Equal(new Coordinate(1,2).x,CoordinateDyctionary["gamma"].currentposition.x);
            Assert.Equal(new Coordinate(1, 2).y, CoordinateDyctionary["gamma"].currentposition.y);
        }

        [Fact]
        public void OnGpsMessage_InvalidFormat()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("zeta", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            ConcurrentDictionary<string, BusState> CoordinateDyctionary = new ConcurrentDictionary<string, BusState>();

            var json = JsonConvert.SerializeObject(new Coordinate(1, 2));
            string message = "gps%zeta%error" + json;
            try
            {
                var risposta = ServerBus.OnGpsMessage(CoordinateDyctionary, message, BusList);
            }
            catch(Exception ex)
            {
                Assert.Equal("Invalid message format", ex.Message);
            }

        }
        //OnStandardMessage
        //**************************************
        //**************************************
        //**************************************
        [Fact]
        public void OnStandardMessage_InvalidFormatFermata()
        {
            ConcurrentDictionary<string, BusState> pullmancoordinate = new ConcurrentDictionary<string, BusState>();
            string message = "";
            try
            {
                ServerBus.OnStandardMessage(message, pullmancoordinate);
            }
            catch (Exception ex)
            {
                Assert.Equal("Invalid message format", ex.Message);
            }
        }
        [Fact]
        public void OnStandardMessage_InvalidFormatJson()
        {
            ConcurrentDictionary<string, BusState> pullmancoordinate = new ConcurrentDictionary<string, BusState>();
            string message = "1";
            try
            {
                ServerBus.OnStandardMessage(message,  pullmancoordinate);
            }
            catch(Exception ex)
            {
                Assert.Equal("Invalid message format", ex.Message);
            }
        }
        [Fact]
        public void OnStandardMessage_AndataBusFound()
        {
            ConcurrentDictionary<string, BusState> CoordinateDyctionary = new ConcurrentDictionary<string, BusState>();
            CoordinateDyctionary["A1"] = new BusState("A1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),3,true);
            CoordinateDyctionary["B2"] = new BusState("B2", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),2,false);
            CoordinateDyctionary["C2"] = new BusState("C2", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),6,false);
            CoordinateDyctionary["D3"] = new BusState("D3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),2,true);



            List<Bus> elebus = new List<Bus>();
            elebus.Add(new Bus("A1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            elebus.Add(new Bus("D3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            var json = JsonConvert.SerializeObject(elebus);
            string messaggio = "6%" + json;

            string codice = ServerBus.OnStandardMessage(messaggio,  CoordinateDyctionary);
            Assert.Equal("D3", codice);
        }
        [Fact]
        public void OnStandardMessage_RitornoBusFound()
        {

            ConcurrentDictionary<string, BusState> CoordinateDyctionary = new ConcurrentDictionary<string, BusState>();
            CoordinateDyctionary["A2"] = new BusState("A2", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 3, true);
            CoordinateDyctionary["B1"] = new BusState("B1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 2, false);
            CoordinateDyctionary["C1"] = new BusState("C1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 6, false);
            CoordinateDyctionary["D2"] = new BusState("D2", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 2, true);



            List<Bus> elebus = new List<Bus>();
            elebus.Add(new Bus("B1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            elebus.Add(new Bus("C1", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            var json = JsonConvert.SerializeObject(elebus);
            string messaggio = "3%" + json;

            string codice = ServerBus.OnStandardMessage(messaggio, CoordinateDyctionary);
            Assert.Equal("B1", codice);
        }
        [Fact]
        public void OnStandardMessage_BusNotFound()
        {
            ConcurrentDictionary<string, BusState> CoordinateDyctionary = new ConcurrentDictionary<string, BusState>();
            CoordinateDyctionary["A3"] = new BusState("A3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 3, true);
            CoordinateDyctionary["B3"] = new BusState("B3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 2, false);
            CoordinateDyctionary["C3"] = new BusState("C3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 6, false);
            CoordinateDyctionary["D5"] = new BusState("D5", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 2, true);



            List<Bus> elebus = new List<Bus>();
            elebus.Add(new Bus("B3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            
            var json = JsonConvert.SerializeObject(elebus);
            string messaggio = "9%" + json;

            string codice = ServerBus.OnStandardMessage(messaggio, CoordinateDyctionary);
            Assert.Equal("Nessun pullman disponibile in tempi brevi", codice);

        }
        //ClassBusState
        //**************************************
        //**************************************
        //**************************************
        [Fact]
        public void CoordinateChanged_NextStopChanged()
        {
            BusState TestBus= new BusState("F5", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 3, true);
            TestBus.currentposition= new Coordinate(45.6969260, 9.6690978);
            Assert.Equal(2, TestBus.LastStop);
            Assert.Equal(6,TestBus.NextStop);
        }
        [Fact]
        public void CoordinateChanged_NextStopChanged_ButStillInRange()
        {
            BusState TestBus = new BusState("F9", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 3, true);
            TestBus.currentposition = new Coordinate(45.6969260, 9.6690978);
            Assert.Equal(2, TestBus.LastStop);
            TestBus.currentposition = new Coordinate(45.6969261, 9.6690979);
            Assert.Equal(2, TestBus.LastStop);
        }
        [Fact]
        public void CoordinateChanged_NextStopNotChanged()
        {
            BusState TestBus = new BusState("F3", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3), 3, true);
            TestBus.currentposition = new Coordinate(45, 9);
            Assert.Equal(3, TestBus.LastStop);
        }
    }
}
