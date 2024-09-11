using System;
using System.Collections.Specialized;

namespace Thingalink
{
    /// <summary>
    /// a number is string when displaed anyway
    /// speck likes being anything. Usage still needs some fixed types
    /// use speck for a primitive only if it caries metadata. can. but for efficent large sets, dont.
    /// </summary>
    public class NumericValue : Mote
    {
        public string ValueText => FormatValue(); 
        public NumericValue()
        {

        }

        protected virtual string FormatValue()
        {
            return Value.ToString();
        }

        public NumericValue(int value) : base(value)
        {

        }
        public NumericValue(float value) : base(value)
        {

        }
        public NumericValue(double value) : base(value)
        {

        }
        public NumericValue(long value) : base(value)
        {

        }
    }
    /// <summary>
    /// a name is not text of the value so overriding Format would be doing the wrong thing
    /// don't 
    /// </summary>
    public class NamedNumber : NumericValue
    {
        protected string name;
        public string Name => name;

        public NamedNumber(string name)
        {
            this.name = name;
        }
        public NamedNumber(string name, int value) : base(value)
        {
            this.name = name;
        }
        public NamedNumber(string name, float value) : base(value)
        {
            this.name = name;
        }
        public NamedNumber(string name, double value) : base(value)
        {
            this.name = name;
        }
        public NamedNumber(string name, long value) : base(value)
        {
            this.name = name;
        }
    }

    

}
