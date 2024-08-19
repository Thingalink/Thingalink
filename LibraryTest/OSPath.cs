namespace LibraryTest;

public class OSPath
{
    private string _dir;
    private string _file;

    public OSPath(string dir, string file)
    {
        _dir = Sanitize(dir);
        _file = Sanitize(file);
    }

    string Sanitize(string str)
    {
        //todo trim slashes and conform to os 
        return str;
    }

    public string Path => _dir + "\\" + _file;
}
