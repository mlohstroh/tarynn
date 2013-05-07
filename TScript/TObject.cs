using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript
{
    public struct TObject
    {
        public string InnerType;
        public string Value;
        public string Name;

        public TObject(string t, string v, string n)
        {
            this.InnerType = t;
            this.Name = n;
            this.Value = v;
        }
    }
}
