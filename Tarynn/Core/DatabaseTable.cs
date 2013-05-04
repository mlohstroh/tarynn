using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.Core
{
    public interface DatabaseTable
    {
        DatabaseTable[] All();
        DatabaseTable Find(int id);
    }
}
