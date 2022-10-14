using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bus_Percorsi;

namespace Mex
{
    public class Base
    {
        public bool Status;
        public List<string> Error;
    }
    public class ServerIMG 
    {
        public class Request
        {
            public string img;
        }
        public class Response : Base
        {
            public Fermata fermata;
            public List<Fermata> ArriviProb;
        }
    }
    public class ServerBuses
    {
        public class Request
        {
            public Fermata Partenza;
            public Fermata Arrivo;

        }
        public class Response : Base
        {
            public List<Bus> buses;
        }
    }
    public class ServerNearBus 
    {
        public class Request
        {
            public List<Bus> buses;
        }
        public class Response : Base
        {
            public Bus bus;
        }
    }
    public class ServerPosition 
    {
        public class Request
        {
            public Bus bus;
        }
        public class Response : Base
        {
            public string img;
        }
    }
}
