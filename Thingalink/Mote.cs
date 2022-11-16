using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingalink
{
    /// <summary>
    /// testing the thing is null is toxic. Always define the motes even if they will be empty
    /// undefined content decays to natural boolean
    /// </summary>
    public interface IMote { bool IsNull { get; } bool IsNotNull { get; } }

    /// <summary>
    ///: a small particle : SPECK
    ///immutable as is. can derive to edit. or otherwise build a securable object
    /// </summary>
    public class Mote : IMote
    {
        protected object Value;

        public Mote() : this(null) { }
        public Mote(object value)
        {
            Value = value;
        }

        //to prevent always checking objects for null there needs to be usage patterns to ensure the objets are never not initialized.
        //content may be null but the container never should
        public bool IsNull => Value == null;
        public bool IsNotNull => Value != null;
    }


}
