using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Expressions
{
    public class All : Value
    {
        public override void WriteTo(QueryBuilder builder)
        {
            builder.AppendAllColumnsWildcard();
        }
    }
}
