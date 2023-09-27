using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KiwiQuery.Sql
{
    internal class QueryBuilderFactory
    {
        private Mode mode;

        public QueryBuilderFactory(Mode mode)
        {
            this.mode = mode;
        }

        public QueryBuilder NewQueryBuilder(DbCommand command)
        {
            switch (this.mode)
            {
                case Mode.MySql:
                    return new MySqlQueryBuilder(command);

                default:
                    throw new NotSupportedException($"No implementation found for {mode}.");
            }
        }
    }
}
