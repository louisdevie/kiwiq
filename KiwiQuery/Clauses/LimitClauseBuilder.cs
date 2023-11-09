using KiwiQuery.Sql;
using System;
using System.Collections.Generic;

namespace KiwiQuery.Clauses
{
    internal class LimitClauseBuilder : IClauseBuilder
    {
        private int limitOnly;
        private LimitClause? clause;

        public LimitClauseBuilder()
        {
            this.limitOnly = -1;
        }

        public void WriteClauseTo(QueryBuilder qb) => this.clause?.WriteTo(qb);

        /// <summary>
        /// Adds a LIMIT statement to the query without offset. The offset can
        /// be configured by combining <br/> this method with <see cref="Offset"/>
        /// or using the <see cref="Limit(int, int)"/> overload.
        /// </summary>
        /// <param name="limit">The maximum number of rows the query should return.</param>
        public void Limit(int limit)
        {
            if (this.clause is null)
            {
                this.limitOnly = limit;
                this.clause = new LimitClause(limit, 0);
            }
            else
            {
                throw new InvalidOperationException("A limit clause already exists in this query, or Offset() was called before Limit().");
            }
        }

        /// <summary>
        /// Adds a LIMIT statement to the query with an offset. Calling
        /// <code>Limit(a, b)</code>
        /// is the same as using
        /// <code>Limit(a).Offset(b)</code>
        /// </summary>
        /// <param name="limit">The maximum number of rows the query should return.</param>
        /// <param name="offset">The numbers of rows to skip in the results.</param>
        public void Limit(int limit, int offset)
        {
            if (this.clause is null)
            {
                this.clause = new LimitClause(limit, offset);
            }
            else
            {
                throw new InvalidOperationException("A limit clause already exists in this query.");
            }
        }

        /// <summary>
        /// Add an offset to the LIMIT statement.
        /// </summary>
        /// <param name="offset">The numbers of rows to skip in the results.</param>
        public void Offset(int offset)
        {
            if (this.clause is null)
            {
                throw new InvalidOperationException("A limit must be reated before calling Offset().");
            }
            else
            {
                this.clause = new LimitClause(this.limitOnly, offset);
            }
        }
    }
}
