using SERVER_IMMAGINEMAPPA;
using Xunit;

namespace Server4Testing
{
    public class Server4_Test
    {
        [Fact]
        public void CreaImmagine_DatiErrati()
        {
            Coordinate coordinate = new Coordinate(100, 90);
            var risposta = ImmagineMappa.CreaImmagine(coordinate);
            byte[] messaggio_aspettato = default;
            Assert.Equal(risposta, messaggio_aspettato);
        }
    }
}
