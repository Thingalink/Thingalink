namespace Thingalink
{
    public interface IReadable
    {
        object Object { get; }
    }
    
    /// <summary>
    /// A Mote where contents are readily visible
    /// </summary>
    public class Speck : Mote, IReadable
    {
        public delegate object Pass(Speck speck);
        public delegate void PassValue(object value);

        public object Object => base.Value;

        public Speck()
        {
        }

        public Speck(object obj) : base(obj)
        {
        }
    }
}
