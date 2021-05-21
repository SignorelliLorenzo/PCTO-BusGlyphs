using Creatore_archivio_pcto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static Dictionary<int, Coordinate> ArchivioCoordinate_Fermate = new Dictionary<int, Coordinate>();

        public Percorso BusPath { get; }
        public string BusName { get; }
        private Coordinate _currentposition;
        public Coordinate currentposition
        {
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
                    Inizialize("ArchivioCoordinate.Json");
                    firsttime = false;
                }
            }

        }
        double range = 0.000002;
        bool samecoordinate = false;
        private bool disposedValue;

        /// <summary>
        /// Operation for when the coordinate changes
        /// </summary>
        private void Inizialize(string path)
        {
            //var newdictionary = new Dictionary<int, Coordinate>() {
            //    {3,new Coordinate(45.6964538, 9.6686629) },
            //    {2,new Coordinate(45.6969260, 9.6690978) },
            //    {6,new Coordinate(45.6975549, 9.6667936) },
            //    {7,new Coordinate(45.7008704, 9.6655066) },
            //    {5,new Coordinate(45.7029905, 9.6720559) },
            //    {0,new Coordinate(45.7032898, 9.6775359) },
            //};

            //var newfile = new StreamWriter(path);
            //newfile.WriteLine(JsonConvert.SerializeObject(newdictionary,Formatting.Indented));
            //newfile.Close();
            var miofile = new StreamReader(path);
            ArchivioCoordinate_Fermate = JsonConvert.DeserializeObject<Dictionary<int, Coordinate>>(miofile.ReadToEnd());
            miofile.Close();
        }
        private void CoordinateChanged()
        {
            var serchcoordinate = ArchivioCoordinate_Fermate[_NextStop];
            if (serchcoordinate.x < (_currentposition.x + range) && serchcoordinate.x > (_currentposition.x - range) && serchcoordinate.y < (_currentposition.y + range) && serchcoordinate.y > (_currentposition.y - range))
            {

                if (samecoordinate)
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

        public BusState(string busname, Percorso buspath, Coordinate StartPosition, int laststop = -1, bool andata = true)
        {
            if (String.IsNullOrWhiteSpace(busname))
            {
                throw new Exception("ADD THE BUSNAME");
            }
            if (BusNames.Contains(busname))
            {
                throw new Exception("BUS COLLEGATO WTF");
            }
            currentposition = StartPosition;
            BusPath = buspath;
            BusName = busname;
            BusNames.Add(busname);
            this._andata = andata;

            if (laststop == -1)
            {
                if (this.andata)
                {
                    this._LastStop = BusPath.elefermateandata.First();
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
