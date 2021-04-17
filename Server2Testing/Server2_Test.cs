using System;
using System.Collections.Generic;
using Creatore_archivio_pcto;
using SERVER_PERCORSO;
using Xunit;
using System.IO;
using Newtonsoft.Json;

namespace Server2Testing
{
    public class Server2_Test
    {
        [Fact]
        public void MessageBusDatiMancanti()
        {
            //Arrange
            var result = new List<Bus>();
            var percorsi = new List<Percorso>();
            var busses = new List<Bus>();
            //Act
            try 
            {
               result= PERCORSO.getmessage(percorsi, busses, 4);
            }
            catch (Exception ex)
            {
                Assert.Equal("Dati non disponibili",ex.Message);
            }
           
            
         }
        [Fact]
        public void MessageBusPercorsoNonTrovato()
        {
            //Arrange
            var result = new List<Bus>();
            var percorsi = new List<Percorso>();
            percorsi.Add(new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }));
            var busses = new List<Bus>();
            busses.Add(null);
            //Act
            try
            {
                result = PERCORSO.getmessage(percorsi, busses, 4);
            }
            catch (Exception ex)
            {
                Assert.Equal("Non sono stati trovati percorsi conformi", ex.Message);
            }
            //Assert
            
        }
        [Fact]
        public void MessageBusSucces()
        {
            //Arrange
            var result = new List<Bus>();
            var percorsi = new List<Percorso>();
            var busses = new List<Bus>();
            percorsi.Add(new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }));
            percorsi.Add(new Percorso("Milano-Brescia", new List<int> { 0, 5, 7 }, new List<int> { 7, 5, 0 }));

            busses.Add(new Bus("5A", percorsi[0]));
            busses.Add(new Bus("5B", percorsi[1]));
        

            var expectedmessage = new List<Bus>();
            expectedmessage.Add(new Bus("5B", percorsi[1]));

            
            //Act
            result = PERCORSO.getmessage(percorsi, busses, 5);
            //Assert
            Assert.Equal(expectedmessage.ToString(), result.ToString());
        }
        [Fact]
        public void CaricaBusFileNonTrovato()
        {
            //Arrange
            var bus = new List<Bus>();
            string path = default;

            //Act
            var result = PERCORSO.CaricaBus(ref bus, path);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void CaricaBusFileNonConforme()
        {
            //Arrange
            var bus = new List<Bus>();
            string path = "CodGlifi";

            //Act
            var result = PERCORSO.CaricaBus(ref bus, path);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void CaricaBusFileConforme()
        {
            //Arrange
            var bus = new List<Bus>();
            string path = "Bus";

            //Act
            var result = PERCORSO.CaricaBus(ref bus, path);
            //Assert
            Assert.True(result);
        }

    }
}
