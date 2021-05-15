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
        public static void Pulisci(Dictionary<string, BusState> MyDyctionary)
        {
            foreach(BusState item in MyDyctionary.Values)
            {
                item.Dispose();
                
            }
            MyDyctionary.Clear();
        }
        //OnGPSMessage
        //**************************************
        //**************************************
        //**************************************
        //**************************************
        [Fact]
        public void OnGPSMessage_NotGPSMessage()
        {           
            List<Bus> BusList = new List<Bus>();
            Dictionary<string, BusState> CoordinateDyctionary = new Dictionary<string, BusState>();
            string message = "";
            var risposta = ServerBus.OnGpsMessage(ref CoordinateDyctionary, message, BusList);
            Assert.False(risposta);
            Pulisci(CoordinateDyctionary);
        }
        [Fact]
        public void OnGPSMessage_NewBus()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("beta",new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            Dictionary<string, BusState> CoordinateDyctionary = new Dictionary<string, BusState>();

            var json = JsonConvert.SerializeObject(new Coordinate(1,2)); 
            string message = "gps%beta%"+json;
            var risposta = ServerBus.OnGpsMessage(ref CoordinateDyctionary, message, BusList);
            Assert.True(risposta);
            Assert.Single(CoordinateDyctionary);
            Pulisci(CoordinateDyctionary);
        }
        [Fact]
        public void OnGPSMessage_SameBus()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("alfa", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            Dictionary<string, BusState> CoordinateDyctionary = new Dictionary<string, BusState>();
            CoordinateDyctionary["alfa"] = new BusState("alfa", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3));

            var json = JsonConvert.SerializeObject(new Coordinate(1, 2));
            string message = "gps%alfa%" + json;
            var risposta = ServerBus.OnGpsMessage(ref CoordinateDyctionary, message, BusList);
            Assert.True(risposta);
            Assert.Single(CoordinateDyctionary);
            Assert.Equal(new Coordinate(1,2).x,CoordinateDyctionary["alfa"].currentposition.x);
            Assert.Equal(new Coordinate(1, 2).y, CoordinateDyctionary["alfa"].currentposition.y);
            Pulisci(CoordinateDyctionary);
        }

        [Fact]
        public void OnGpsMessage_InvalidFormat()
        {
            List<Bus> BusList = new List<Bus>();
            BusList.Add(new Bus("gamma", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            Dictionary<string, BusState> CoordinateDyctionary = new Dictionary<string, BusState>();

            var json = JsonConvert.SerializeObject(new Coordinate(1, 2));
            string message = "gps%gamma%error" + json;
            try
            {
                var risposta = ServerBus.OnGpsMessage(ref CoordinateDyctionary, message, BusList);
            }
            catch(Exception ex)
            {
                Assert.Equal("Invalid message format", ex.Message);
            }

            Pulisci(CoordinateDyctionary);
        }
        //OnStandardMessage
        //**************************************
        //**************************************
        //**************************************
        [Fact]
        public void OnStandardMessage_InvalidFormatFermata()
        {
            Dictionary<string, BusState> pullmancoordinate = new Dictionary<string, BusState>();
            string message = "";
            try
            {
                ServerBus.OnStandardMessage(message, ref pullmancoordinate);
            }
            catch (Exception ex)
            {
                Assert.Equal("Invalid message format", ex.Message);
            }
            Pulisci(pullmancoordinate);
        }
        [Fact]
        public void OnStandardMessage_InvalidFormatJson()
        {
            Dictionary<string, BusState> pullmancoordinate = new Dictionary<string, BusState>();
            string message = "1";
            try
            {
                ServerBus.OnStandardMessage(message, ref pullmancoordinate);
            }
            catch(Exception ex)
            {
                Assert.Equal("Invalid message format", ex.Message);
            }
            Pulisci(pullmancoordinate);
        }
        [Fact]
        public void OnStandardMessage_AndataBusFound()
        {
            Dictionary<string, BusState> CoordinateDyctionary = new Dictionary<string, BusState>();
            CoordinateDyctionary["alfa"] = new BusState("alfa", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),3,true);
            CoordinateDyctionary["beta"] = new BusState("beta", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),2,false);
            CoordinateDyctionary["gamma"] = new BusState("gamma", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),6,false);
            CoordinateDyctionary["zeta"] = new BusState("zeta", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }), new Coordinate(2, 3),2,true);



            List<Bus> elebus = new List<Bus>();
            elebus.Add(new Bus("alfa", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            elebus.Add(new Bus("zeta", new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 })));
            var json = JsonConvert.SerializeObject(elebus);
            string messaggio = "6%" + json;

            string codice = ServerBus.OnStandardMessage(messaggio, ref CoordinateDyctionary);
            Assert.Equal("zeta", codice);
            Pulisci(CoordinateDyctionary);
        }
        [Fact]
        public void OnStandardMessage_RitornoBusFound()
        {

            //Pulisci(CoordinateDyctionary);
        }
        [Fact]
        public void OnStandardMessage_BusNotFound()
        {

            //Pulisci(CoordinateDyctionary);
        }
    }
}
