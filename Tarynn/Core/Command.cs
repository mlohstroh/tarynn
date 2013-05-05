using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.Core
{
    public class Command : Query
    {
        public Command(string text)
            : base(text)
        {

        }
    }
}
