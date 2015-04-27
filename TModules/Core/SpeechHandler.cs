using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if __MonoCS__
namespace TModules.Core
{
    public class SpeechHandler
    {
        private Queue<string> messageQueue = new Queue<string>();

        public void AddMessageToQueue(string message)
        {
            if (messageQueue.Count == 0)
                Console.WriteLine ("Speaking: " + message);
            else
                messageQueue.Enqueue(message);
        }

        public SpeechHandler()
        {
        }

        public void StopSpeaking()
        {
            messageQueue.Clear(); //bummer :(
        }
    }
}
#else

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

        public void SpeakNowAndBlock(string message)
        {
            StopSpeaking();
            mSynth.Speak(message);
        }

        public SpeechHandler()
        {
            mSynth.SelectVoiceByHints(VoiceGender.Female);
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

#endif