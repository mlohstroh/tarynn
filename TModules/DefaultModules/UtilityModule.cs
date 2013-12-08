using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TModules.Core;
using System.Text.RegularExpressions;

namespace TModules.DefaultModules
{
    class UtilityModule : TModule
    {
        private DateTime _startTime;
        private TimeSpan _lastTimeSpan = TimeSpan.Zero;

        public UtilityModule(ModuleManager host)
            : base("Utilities", host)
        {
            AddCallback("start timer", StartTimer);
            AddCallback("stop timer", (Match message) =>
            {
                _lastTimeSpan = (DateTime.Now - _startTime);
            });
            AddCallback("last time in (minutes|seconds|hours)", SpeakLastTime);
            AddCallback("stop talking", (Match message) =>
            {
                Host.InterruptSpeech();
            });
            AddCallback("(\\d) (plus|\\+|minus|-|times|\\*|over|\\/) (\\d)", Calculate);
        }

        private void StartTimer(Match message)
        {
            _startTime = DateTime.Now;
            Host.SpeakEventually("Timer started!");
        }

        private void Calculate(Match message)
        {
            int valueOne = int.Parse(message.Groups[1].Value);
            string action = message.Groups[2].Value;
            int valueTwo = int.Parse(message.Groups[3].Value);
        }

        private void SpeakLastTime(Match message)
        {
            if (_lastTimeSpan == TimeSpan.Zero)
            {
                Host.SpeakEventually("I don't have a last time stored");
                return;
            }
            switch (message.Groups[1].Value)
            {
                case "minutes":
                    Host.SpeakEventually("Last time took " + _lastTimeSpan.Minutes + " minutes");
                    break;
                case "seconds":
                    Host.SpeakEventually("Last time took " + _lastTimeSpan.Seconds + " seconds");
                    break;
                case "hours":
                    Host.SpeakEventually("Last time took " + _lastTimeSpan.Hours + " hours");
                    break;
            }
        }
    }
}
