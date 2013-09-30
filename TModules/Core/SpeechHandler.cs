using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace TModules.Core
{
    public class SpeechHandler
    {
        private SpeechSynthesizer mSynth = new SpeechSynthesizer();
        private Queue<string> messageQueue = new Queue<string>();

        public void AddMessageToQueue(string message)
        {
            if (messageQueue.Count == 0)
                mSynth.SpeakAsync(message);
            else
                messageQueue.Enqueue(message);
        }

        public SpeechHandler()
        {
            mSynth.SpeakCompleted += mSynth_SpeakCompleted;
        }

        void mSynth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (messageQueue.Count != 0)
            {
                string nextMessage = messageQueue.Dequeue();
                mSynth.SpeakAsync(nextMessage);
            }
        }

        public void StopSpeaking()
        {
            mSynth.SpeakAsyncCancelAll();
            messageQueue.Clear(); //bummer :(
        }
    }
}
