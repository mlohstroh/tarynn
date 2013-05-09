using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.Core
{
    /// <summary>
    /// A class to handle screen exit arguments
    /// </summary>
    public class TarynnEchoEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reason">The reason for exiting the screen</param>
        /// <param name="type">The type of screen that is being exited</param>
        public TarynnEchoEventArgs(string echo)
        {
            this.Echo = echo;
        }

        /// <summary>
        /// The reason for exiting the screen
        /// </summary>
        public string Echo { get; set; }
    }
}
