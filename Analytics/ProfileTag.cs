using System;
using System.Diagnostics;

namespace Analytics
{
    public class ProfileTag : IDisposable
    {
        private TConsole _logger;
        private Stopwatch _watch;
        public string Tag { get; private set; }

        public ProfileTag (string tag)
        {
            _watch = new Stopwatch ();
            _watch.Start ();
            Tag = tag;
            _logger = new TConsole (string.Format("Profiler Tag: {0}", Tag));
        }

        public void Dispose()
        {
            if (_watch != null)
            {
                _watch.Stop();
                _logger.InfoFormat ("{0}: {1}", Tag, _watch.Elapsed.ToString("G"));
            }
        }
    }
}

