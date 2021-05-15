using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Creatore_archivio_pcto;
using System.Timers;

namespace SERVER_BUS
{
    public class BusState : IDisposable
    {
        
        private Timer lastcall = new Timer();
        private bool _andata;
        public bool andata
        {
            get { return _andata; }
        }
        static private List<string> BusNames = new List<string>();
        private bool firsttime = true;
        private int _LastStop;
        public int LastStop { get { return _LastStop; } }
        private int _NextStop;
        public int NextStop { get { return _NextStop; } }
        private Dictionary<int, Coordinate> ArchivioCoordinate_Fermate = new Dictionary<int, Coordinate>();
        public Percorso BusPath { get; }
        public string BusName { get; }
        private Coordinate _currentposition;
        public Coordinate currentposition {
            get { return _currentposition; }
            set
            {
                _currentposition = value;
                if (!firsttime)
                {
                    CoordinateChanged();
                }
                else
                {
                    firsttime = false;
                }
            }

        }
        double range = 0.2;
        bool samecoordinate = false;
        private bool disposedValue;

        /// <summary>
        /// Operation for when the coordinate changes
        /// </summary>
        private void CoordinateChanged()
        {
            if(ArchivioCoordinate_Fermate.Where(x=> x.Value.x<(_currentposition.x+range) && x.Value.x > (_currentposition.x - range) && x.Value.y < (_currentposition.y + range) && x.Value.y > (_currentposition.y - range)).Count() == 0 && ArchivioCoordinate_Fermate.Where(x => x.Value.x < (_currentposition.x + range) && x.Value.x > (_currentposition.x - range) && x.Value.y < (_currentposition.y + range) && x.Value.y > (_currentposition.y - range)).FirstOrDefault().Key == _NextStop)
            {
                
                if(samecoordinate)
                {
                    return;
                }
                _LastStop = _NextStop;
                if (andata)
                {
                    try
                    {
                        
                        _NextStop = BusPath.elefermateandata[BusPath.elefermateandata.IndexOf(_LastStop) + 1];
                    }
                    catch
                    {
                        _NextStop = BusPath.elefermateritorno[0];
                        _andata = !andata;
                    }
                }
                else
                {
                    try
                    {
                        _NextStop = BusPath.elefermateritorno[BusPath.elefermateritorno.IndexOf(_LastStop) + 1];
                    }
                    catch
                    {
                        _NextStop = BusPath.elefermateandata[0];
                        _andata = !andata;
                    }
                }
                samecoordinate = true;
            }
            else
            {
                samecoordinate = false;
            }
        }
       
        public BusState(string busname,Percorso buspath,Coordinate StartPosition, int laststop = -1, bool andata = true)
        {
            if(String.IsNullOrWhiteSpace(busname))
            {
                throw new Exception("ADD THE BUSNAME");
            }
            if(BusNames.Contains(busname))
            {
                throw new Exception("BUS COLLEGATO WTF");
            }
            currentposition = StartPosition;
            BusPath = buspath;
            BusName = busname;
            BusNames.Add(busname);
            this._andata = andata;

            if(laststop==-1)
            {
                if(this.andata)
                {
                    this._LastStop = BusPath.elefermateandata.First();
                    bool appoggio = false;
                    foreach(int item in BusPath.elefermateandata)
                    {
                        if(item == this.LastStop)
                        {
                            appoggio = true;
                        }
                        else if(appoggio)
                        {
                            this._NextStop = item;
                            break;
                        }
                    }
                }
                else
                {
                    this._LastStop = BusPath.elefermateritorno.First();
                    bool appoggio = false;
                    foreach (int item in BusPath.elefermateritorno)
                    {
                        if (item == this.LastStop)
                        {
                            appoggio = true;
                        }
                        else if (appoggio)
                        {
                            this._NextStop = item;
                            break;
                        }
                    }
                }
            }
            else
            {
                this._LastStop = laststop;
                if (this.andata)
                {
                    bool appoggio = false;
                    foreach (int item in BusPath.elefermateandata)
                    {
                        if (item == this.LastStop)
                        {
                            appoggio = true;
                        }
                        else if (appoggio)
                        {
                            this._NextStop = item;
                            break;
                        }
                    }
                }
                else
                {
                    bool appoggio = false;
                    foreach (int item in BusPath.elefermateritorno)
                    {
                        if (item == this.LastStop)
                        {
                            appoggio = true;
                        }
                        else if (appoggio)
                        {
                            this._NextStop = item;
                            break;
                        }
                    }
                }
               
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                BusNames.RemoveAll(b => b == this.BusName);              
                disposedValue = true;
            }
        }

        ~BusState()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
