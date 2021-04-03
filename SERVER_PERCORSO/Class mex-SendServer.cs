using System;
using System.Collections.Generic;
using System.Text;
using Creatore_archivio_pcto;
using System.Linq;

namespace Messages
{
    public class mex
    {
        public int codfermata { get; set; }
        public List<Percorso> Percorsi = new List<Percorso>();
        private string _direzione;
    }
    class mexdestinazione : mex
    {
        public int Destinazione { get; set; }
        public mexdestinazione(int destinazione,mex messaggio)
        {
            base.Percorsi = messaggio.Percorsi.ToList();
            base.codfermata = messaggio.codfermata;
            this.Destinazione = destinazione;
        }
          

    }
}
