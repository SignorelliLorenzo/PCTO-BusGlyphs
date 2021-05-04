using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Creatore_archivio_pcto;
using System.Timers;

namespace SERVER_BUS
{
    class BusState
    {
        private Timer lastcall = new Timer();
        private bool andata = true;
        static private List<string> BusNames = new List<string>();
        private bool firsttime = true;
        public string BusName { get; }
        private Coordinate _currentposition;
        public Coordinate currentposition {
            get {return _currentposition; } 
            set
            {
                _currentposition = value;
                if(!firsttime)
                {
                    CordinateChanged();
                }
                else
                {
                    firsttime = false;
                }
            } 
                
        }
        double range = 0.2;
        private void CordinateChanged()
        {
            

            if(ArchivioCoordinate_Fermate.Where(x=> x.Value.x<(_currentposition.x+range) && x.Value.x > (_currentposition.x - range) && x.Value.y < (_currentposition.y + range) && x.Value.y > (_currentposition.y - range)).Count() == 0)
            {
                
                _LastStop = _NextStop;
                if(andata)
                {
                    try
                    {
                        _NextStop = BusPath.elefermateandata[BusPath.elefermateandata.IndexOf(_LastStop) + 1];
                    }
                    catch
                    {
                        _NextStop = BusPath.elefermateritorno[0];
                        andata = !andata;
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
                        andata = !andata;
                    }
                }
            }
        }
        private int _LastStop;
        public int LastStop { get { return _LastStop; } }
        private int _NextStop;
        public int NextStop { get { return _NextStop; } }
        private Dictionary<string, Coordinate> ArchivioCoordinate_Fermate = new Dictionary<string, Coordinate>();
        public Percorso BusPath { get; }
        public BusState(string busname,Percorso buspath,Coordinate StartPosition)
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

        }
    }
}
