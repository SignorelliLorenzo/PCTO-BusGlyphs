using System;
using System.Collections.Generic;
using System.Text;
using Creatore_archivio_pcto;

namespace SERVER_PERCORSO
{
    public class mex
    {
        public int codfermata { get; set; }
        public List<Percorso> Percorsi = new List<Percorso>();
        private string _direzione;
        public string direzione
        {
            get { return direzione; }
            set
            {
                if (value != "ANDATA" && value != "RITORNO" && value != "ENTRAMBE")
                {
                    throw new Exception("Inserire uno stato valido");
                }
                _direzione = value;
            }

        }



    }
    class SENDSERVER :mex
    {
           public int Destinazione { get; set; }
       

    }
}
