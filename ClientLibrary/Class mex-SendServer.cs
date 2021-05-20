using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
