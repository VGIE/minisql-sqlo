using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbManager
{
    public interface MiniSqlQuery
    {
        string Execute(Database database);
    }
}
