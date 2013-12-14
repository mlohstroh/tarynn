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

        public static TObject Empty()
        {
            return new TObject(null, null, null);
        }

        public bool IsEmpty()
        {
            return this.InnerType == null && this.Name == null && this.Value == null;
        }

        public static bool operator ==(TObject one, TObject two)
        {
            return one.Value == two.Value;
        }

        public static bool operator !=(TObject one, TObject two)
        {
            return one.Value != two.Value;
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

        public static TObjectChange Empty()
        {
            return new TObjectChange(TObject.Empty(), TObject.Empty());
        }

        public bool IsEmpty()
        {
            return this.objectOne.IsEmpty() && this.objectTwo.IsEmpty();
        }
    }
}
