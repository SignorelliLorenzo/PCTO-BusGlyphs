using System;
using System.Collections.Generic;

namespace Creatore_archivio_pcto
{
    public class Bus : IDisposable
    {
        private static List<string> elecodici = new List<string>();
        private string _codice;
        public string codice
        {
            get
            {
                return _codice;
            }
        }
        private Percorso _percorso;
        public Percorso percorso
        {
            get
            {
                return _percorso;
            }
            set
            {
                if (value == null)
                    throw new Exception("Percorso nullo");
                _percorso = value;
            }
        }
        bool stato = default;
        public Bus(string codice, Percorso percorso)
        {
            if (String.IsNullOrEmpty(codice) || elecodici.Contains(codice))
                throw new Exception("codice nullo o già usato");
            this._codice = codice;
            this.percorso = percorso;
            elecodici.Add(codice);
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!disposed)
            {
                elecodici.RemoveAll(p => p == this.codice);
                disposed = true;
            }
        }
        ~Bus()
        {
            Dispose(false);
        }

        public override string ToString()
        {
            return $"{this.codice} - {this.percorso.ToString()}";
        }
    }
    public class Percorso : IDisposable
    {
        private static List<string> elenomi = new List<string>();
        public List<int> elefermateandata = new List<int>();
        public List<int> elefermateritorno = new List<int>();

        private string _nome;
        public string nome
        {
            get
            {
                return _nome;
            }
        }

        public Percorso(string nome, List<int> eleandata, List<int> eleritorno)
        {
            if (String.IsNullOrEmpty(nome) || elenomi.Contains(nome))
                throw new Exception("Nome nullo o già usato");
            _nome = nome;
            this.elefermateandata = eleandata;
            this.elefermateritorno = eleritorno;
            //elenomi.Add(nome);
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!disposed)
            {
                elenomi.RemoveAll(p => p == this.nome);
                disposed = true;
            }
        }
        ~Percorso()
        {
            Dispose(false);
        }

        public override string ToString()
        {
            string andata = "";
            foreach (int item in this.elefermateandata)
            {
                andata = andata + item.ToString();
            }
            return $"{this.nome} - {andata}";
        }
    }

}
