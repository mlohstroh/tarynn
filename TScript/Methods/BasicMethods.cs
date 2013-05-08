using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Methods
{
    public class BasicMethods : MethodPackage
    {
        public BasicMethods()
            : base("Basic Methods")
        {
            this.methodNames.Add("rand");
            this.methodNames.Add("insert");
            this.methodNames.Add("mod");
            this.methodNames.Add("valueAt");
            this.supportedTypes.Add("integer");
            this.supportedTypes.Add("string");
            this.supportedTypes.Add("list");
        }

        public override TObjectChange GetResultForMethod(string method, object[] args)
        {
            TObjectChange change = new TObjectChange();

            switch (method)
            {
                case "rand":
                    Random r = new Random();
                    int value = r.Next(int.Parse((string)args[0]));
                    TObject obj = (TObject)args[1];
                    TObject obj2 = new TObject(obj.InnerType, value, obj.Name);
                    change = new TObjectChange(obj, obj2);
                    break;
                case "insert":
                    TObject list = (TObject)args[1];
                    List<object> innerList = (List<object>)list.Value;
                    innerList.Add(args[0]);
                    change = MakeChange(list, innerList);
                    break;
                case "mod":
                    int big = -1;
                    if (args[0].GetType() == typeof(TObject))
                    {
                        big = (int)((TObject)args[0]).Value;
                    }
                    else
                    {
                        big = int.Parse((string)args[0]);
                    }
 
                    int small = int.Parse((string)args[1]);
                    int mod = big % small;
                    change = MakeChange((TObject)args[2], mod);
                    break;
                case "valueAt":
                    int index;
                    if (args[0].GetType() == typeof(TObject))
                    {
                        index = (int)((TObject)args[0]).Value;
                    }
                    else
                    {
                        index = int.Parse((string)args[0]);
                    }
                    object valueAt = ((List<object>)((TObject)args[1]).Value).ElementAt(index);
                    change = MakeChange((TObject)args[2], valueAt);
                    break;
            }

            return change;
        }

        public override TObject GetNewObjectForType(string type, string name)
        {
            TObject obj = new TObject();
            obj.Name = name;
            obj.InnerType = type;

            switch (type)
            {
                case "integer":
                    obj.Value = 0;
                    break;
                case "list":
                    obj.Value = new List<object>();
                    break;
                case "string":
                    obj.Value = "";
                    break;
            }

            return obj;
        }

        private TObjectChange MakeChange(TObject obj, object newValue)
        {
            TObject newObj = new TObject(obj.InnerType, newValue, obj.Name);
            return new TObjectChange(obj, newObj);
        }
    }
}
