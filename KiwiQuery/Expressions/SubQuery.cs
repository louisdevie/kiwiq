using KiwiQuery.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Expressions
{
    public class SubQuery : Value
    {
        private SelectQuery query;

        public SubQuery(SelectQuery query)
        {
            this.query = query;
        }

        public override void WriteTo(QueryBuilder builder)
        {
            builder.OpenBracket();
            this.query.BuildCommandText(builder);
            builder.CloseBracket();
        }
    }
}
