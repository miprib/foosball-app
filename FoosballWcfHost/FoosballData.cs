using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FoosballWcfHost
{
    [DataContract]
    public class FoosballData
    {
        public FoosballData()
        {
            Name = "Hello ";
            SayHello = false;
        }

        [DataMember]
        public bool SayHello { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}