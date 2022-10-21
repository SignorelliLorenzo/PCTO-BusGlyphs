using System.Collections.Generic;
using System.Linq;

namespace ClientLibrary
{

    public class mex
    {
        public int codfermata { get; set; }
        public List<Percorso> Percorsi = new List<Percorso>();

    }
    public class mexdestinazione : mex
    {

        public int Destinazione { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinazione">Destinazione desiderata</param>
        /// <param name="messaggio">Messsaggio ricevuto dal server</param>
        public mexdestinazione(int destinazione, mex messaggio)
        {
            base.Percorsi = messaggio.Percorsi.ToList();
            base.codfermata = messaggio.codfermata;
            this.Destinazione = destinazione;
        }
        public mexdestinazione()
        { }

    }

}
