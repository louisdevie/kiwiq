using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Sql
{
    public interface IWriteable
    {
        void WriteTo(QueryBuilder builder);
    }
}
