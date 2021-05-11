using System;
using Xunit;
using System.Collections.Generic;
using SERVER_BUS;
using Creatore_archivio_pcto;

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
    }
}
