using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiwiQuery.Sql;

namespace KiwiQuery.Clauses
{
    internal abstract class Clause : IWriteable
    {
        public abstract void WriteTo(QueryBuilder builder);
    }
}
