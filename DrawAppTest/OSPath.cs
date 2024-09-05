namespace DrawAppTest
{
    public class OSPath
    {
        private string _dir;
        private string _file;

        public OSPath(string dir, string file)
        {
            _dir = Sanitize(dir);
            _file = Sanitize(file);
        }

        public virtual string Sanitize(string str)
        {
            //todo trim slashes and conform to os 
            //i dont contruct paths willy nilly. so therefore break if misused.
            //the place to overide, get more 'safe' with it here, or otherwise port the format
            return str;
        }

        public string Path => _dir + "\\" + _file;
    }
}