﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Percorsi
{
    public class SameId : EqualityComparer<HasId>
    {
        public override bool Equals(HasId x, HasId y)
        {
            return x.Id == y.Id;
        }

        public override int GetHashCode([DisallowNull] HasId obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    public interface HasId
    {
        public string Id { get; }
    }
    public class Bus : IDisposable, HasId
    {
        private static List<string> Ids = new List<string>();
        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
        }
        public string Nome { get; set; }
        private Percorso _percorso;
        public Percorso percorso
        {
            get
            {
                return _percorso;
            }
            set
            {

                _percorso = value;
            }
        }
        public bool Andata { get; set; }
        public Bus(string Id, Percorso percorso, string nome)
        {
            if (Ids.Contains(Id) || String.IsNullOrEmpty(Id))
            {
                throw new Exception("Id must be unique and not null");
            }
            this.Nome = nome;
            this._id = Id;
            this.percorso = percorso;
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
                Ids.RemoveAll(p => p == this.Id);
                disposed = true;
            }
        }
        ~Bus()
        {
            Dispose(false);
        }

        public override string ToString()
        {
            return $"{this.Id} - {this.percorso.ToString()}";
        }

    }
    public class Percorso : IDisposable, HasId
    {
        private static List<string> elenomi = new List<string>();
        public List<Fermata> elefermateandata = new List<Fermata>();
        public List<Fermata> elefermateritorno = new List<Fermata>();
        public string Id
        {
            get
            {
                return _nome;
            }
        }
        private string _nome;
        public string nome
        {
            get
            {
                return _nome;
            }
        }

        public Percorso(string nome, List<Fermata> eleandata, List<Fermata> eleritorno)
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

            return this.nome;
        }
    }
    public class Fermata : IDisposable, HasId
    {
        private static List<string> Ids = new List<string>();
        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
        }
        public class GPS
        {
            public double x { get; set; }
            public double y { get; set; }
        }
        public Fermata(string Id, double x, double y)
        {
            if (Ids.Contains(Id) || String.IsNullOrEmpty(Id))
            {
                throw new Exception("Id must be unique and not null");
            }
            GPS Coordinates = new GPS();
            Coordinates.x = x;
            Coordinates.y = y;
            this._id = Id;
        }
        public GPS Coordinates { get; set; }
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
                Ids.RemoveAll(p => p == this.Id);
                disposed = true;
            }
        }
        ~Fermata()
        {
            Dispose(false);
        }

        public override string ToString()
        {
            return $"{this.Id}";
        }
    }
}