using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingalink
{
   
    /// <summary>
    /// </summary>
    public class StringValue : Mote
    {

        public StringValue() : this(null) { }
        public StringValue(string value) : base(value)
        {
        }

    }


}
