using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace WitAI
{
    public class WitEntity
    {
        public JsonData _data { get; private set; }

        public WitEntity(JsonData data)
        {
            // not even going to bother because these can always be different data structures
            _data = data;
        }

        public JsonData GetValue(string key)
        {
            try
            {
                var obj = _data[key];
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
