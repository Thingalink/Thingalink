using System.Media;

namespace DrawAppTest
{
    public class OSAudio
    {
        private OSPath _file;
        SoundPlayer SoundPlayer;

        public OSAudio(string dir, string file)
        {
            _file = new OSPath(dir, file);
            SoundPlayer = new SoundPlayer(_file.Path);
        }

        public void Play()
        {
            // if (TheSingleton.Sound.Enabled)
            SoundPlayer.Play();
        }
        public void PlaySync()
        {
            // if (TheSingleton.Sound.Enabled)
            SoundPlayer.PlaySync();
        }
        public void Stop()
        {
            SoundPlayer.Stop();
        }
    }
}