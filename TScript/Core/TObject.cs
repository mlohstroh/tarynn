using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript
{
    /// <summary>
    /// An arbitrary object that stores a value and its type
    /// </summary>
    public struct TObject
    {
        public string InnerType;
        public object Value;
        public string Name;

        public TObject(string t, object v, string n)
        {
            this.InnerType = t;
            this.Name = n;
            this.Value = v;
        }
    }

    /// <summary>
    /// A request to switch object one with the second object
    /// </summary>
    public struct TObjectChange
    {
        public TObject objectOne;
        public TObject objectTwo;

        public TObjectChange(TObject one, TObject two)
        {
            this.objectOne = one;
            this.objectTwo = two;
        }
    }
}
