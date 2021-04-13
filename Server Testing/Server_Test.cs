using System;
using Xunit;
using WebSocket4Net;
using SERVER_IDGLYPHS;
using Creatore_archivio_pcto;
using Creatore_archivio_pcto_Percorso;
using System.Collections.Generic;
using Newtonsoft.Json;
using SERVER_PERCORSO;

namespace Server_Testing
{
    public class Server1_Test
    {
        [Fact]
        public void MessageIDFermataInsistente()
        {
            //Arrange
            string result = default;
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            var codici_fermate = new Dictionary<string, int>();
            //Act
            result=IDGLYPHS.getmessage("asd",percorsi, codici_fermate);
            //Assert
            Assert.Equal("ERRORE: Fermata inesistente", result);
        }

        [Fact]
        public void MessageIDFermataNonTrovataNeiPercorsi()
        {
            //Arrange
            string result = default;
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            var codici_fermate = new Dictionary<string, int>();
            codici_fermate["asd"] = 1;
            //Act
            result = IDGLYPHS.getmessage("asd", percorsi, codici_fermate);
            //Assert
            Assert.Equal("ERRORE: Fermata non trovata", result);
        }
        [Fact]
        public void MessaggeIDSuccess()
        {
            //Arrange
            string result = default;
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            var codici_fermate = new Dictionary<string, int>();
            percorsi.Add(new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }));
            percorsi.Add(new Percorso("Milano-Brescia", new List<int> { 0, 5, 7 }, new List<int> { 7, 5, 0 }));
            percorsi.Add(new Percorso("Telgate-Bergamo", new List<int> { 3, 4, 5 }, new List<int> { 5, 4, 3 }));
            percorsi.Add(new Percorso("Bergamo-Brescia", new List<int> { 0, 7, 2 }, new List<int> { 2, 7, 0 }));
            percorsi.Add(new Percorso("Como-Bergamo", new List<int> { 4, 0, 6 }, new List<int> { 6, 0, 4 }));
            percorsi.Add(new Percorso("Como-Brescia", new List<int> { 3, 0, 6 }, new List<int> { 6, 7, 3 }));
            codici_fermate.Add("das", 3);
            codici_fermate.Add("vdf", 2);
            codici_fermate.Add("qwe", 6);
            codici_fermate.Add("fas", 4);
            codici_fermate.Add("ers", 7);
            codici_fermate.Add("vfd", 5);
            codici_fermate.Add("pkl", 0);
            var expectedmessage = new mex();
            expectedmessage.Percorsi.Add(new Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }));
            expectedmessage.Percorsi.Add(new Percorso("Como-Bergamo", new List<int> { 4, 0, 6 }, new List<int> { 6, 0, 4 }));
            expectedmessage.Percorsi.Add(new Percorso("Como-Brescia", new List<int> { 3, 0, 6 }, new List<int> { 6, 7, 3 }));
            
            expectedmessage.codfermata = 6;
            //Act
            result = IDGLYPHS.getmessage("qwe", percorsi, codici_fermate);
            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedmessage), result);
        }
        [Fact]
        public void CaricaPercorsiFileNonTrovato()
        {
            //Arrange
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            string path = default;

            //Act
            var result = IDGLYPHS.CaricaPercorsi(ref percorsi, path);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void CaricaPercorsiFileNonConforme()
        {
            //Arrange
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            string path = "CodGlifi";

            //Act
            var result = IDGLYPHS.CaricaPercorsi(ref percorsi, path);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void CaricaPercorsiFileConforme()
        {
            //Arrange
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            string path = "Percorsi";

            //Act
            var result = IDGLYPHS.CaricaPercorsi(ref percorsi, path);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void CaricaGlifiFileNonTrovato()
        {
            //Arrange
            var codici_fermate = new Dictionary<string, int>();
            string path = default;

            //Act
            var result = IDGLYPHS.CaricaCOD_Glyphs(ref codici_fermate, path);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void CaricaGlifiFileNonConforme()
        {
            //Arrange
            var codici_fermate = new Dictionary<string, int>();
            string path = "Percorsi";

            //Act
            var result = IDGLYPHS.CaricaCOD_Glyphs(ref codici_fermate, path);
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void CaricaGlifiFileConforme()
        {
            //Arrange
            var codici_fermate = new Dictionary<string, int>();
            string path = "CodGlifi";

            //Act
            var result = IDGLYPHS.CaricaCOD_Glyphs(ref codici_fermate, path);
            //Assert
            Assert.True(result);
        }
    }
    public class Server2_Test
    {
        [Fact]
        public void MessageIDFermataInsistente()
        {
            //Arrange
            string result = default;
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            var codici_fermate = new Dictionary<string, int>();
            //Act
            result = IDGLYPHS.getmessage("asd", percorsi, codici_fermate);
            //Assert
            Assert.Equal("ERRORE: Fermata inesistente", result);
        }

        [Fact]
        public void MessageIDFermataNonTrovataNeiPercorsi()
        {
            //Arrange
            string result = default;
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            var codici_fermate = new Dictionary<string, int>();
            codici_fermate["asd"] = 1;
            //Act
            result = IDGLYPHS.getmessage("asd", percorsi, codici_fermate);
            //Assert
            Assert.Equal("ERRORE: Fermata non trovata", result);
        }
        [Fact]
        public void MessaggeIDSuccess()
        {
            //Arrange
            string result = default;
            var percorsi = new List<Creatore_archivio_pcto.Percorso>();
            var codici_fermate = new Dictionary<string, int>();
            percorsi.Add(new Creatore_archivio_pcto.Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }));
            percorsi.Add(new Creatore_archivio_pcto.Percorso("Milano-Brescia", new List<int> { 0, 5, 7 }, new List<int> { 7, 5, 0 }));
            percorsi.Add(new Creatore_archivio_pcto.Percorso("Telgate-Bergamo", new List<int> { 3, 4, 5 }, new List<int> { 5, 4, 3 }));
            percorsi.Add(new Creatore_archivio_pcto.Percorso("Bergamo-Brescia", new List<int> { 0, 7, 2 }, new List<int> { 2, 7, 0 }));
            percorsi.Add(new Creatore_archivio_pcto.Percorso("Como-Bergamo", new List<int> { 4, 0, 6 }, new List<int> { 6, 0, 4 }));
            percorsi.Add(new Creatore_archivio_pcto.Percorso("Como-Brescia", new List<int> { 3, 0, 6 }, new List<int> { 6, 7, 3 }));
            codici_fermate.Add("das", 3);
            codici_fermate.Add("vdf", 2);
            codici_fermate.Add("qwe", 6);
            codici_fermate.Add("fas", 4);
            codici_fermate.Add("ers", 7);
            codici_fermate.Add("vfd", 5);
            codici_fermate.Add("pkl", 0);
            var expectedmessage = new mex();
            expectedmessage.Percorsi.Add(new Creatore_archivio_pcto.Percorso("Milano-Bergamo", new List<int> { 3, 2, 6 }, new List<int> { 6, 2, 3 }));
            expectedmessage.Percorsi.Add(new Creatore_archivio_pcto.Percorso("Como-Bergamo", new List<int> { 4, 0, 6 }, new List<int> { 6, 0, 4 }));
            expectedmessage.Percorsi.Add(new Creatore_archivio_pcto.Percorso("Como-Brescia", new List<int> { 3, 0, 6 }, new List<int> { 6, 7, 3 }));

            expectedmessage.codfermata = 6;
            //Act
            result = IDGLYPHS.getmessage("qwe", percorsi, codici_fermate);
            //Assert
            Assert.Equal(JsonConvert.SerializeObject(expectedmessage), result);
        }
        [Fact]
        public void CaricaBusFileNonTrovato()
        {
            //Arrange
            var bus = new List<Creatore_archivio_pcto_Percorso.Percorso>();
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
            var bus = new List<Creatore_archivio_pcto_Percorso.Bus>();
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
            var bus = new List<Creatore_archivio_pcto_Percorso.Bus>();
            string path = "Percorsi";

            //Act
            var result = PERCORSO.CaricaBus(ref bus, path);
            //Assert
            Assert.True(result);
        }

    }
}
