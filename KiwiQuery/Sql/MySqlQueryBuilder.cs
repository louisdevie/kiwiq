using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using System.Collections.Generic;
using System.Data.Common;

namespace KiwiQuery.Sql
{
    /// <summary>
    /// A query builder for MySQL.
    /// </summary>
    internal class MySqlQueryBuilder : QueryBuilder
    {
        private bool endsWithWordBoundary;

        public MySqlQueryBuilder(DbCommand command) : base(command)
        {
            this.endsWithWordBoundary = true;
        }

        private void EnsureWordBoundary()
        {
            if (!this.endsWithWordBoundary)
            {
                this.Buffer.Append(' ');
                this.endsWithWordBoundary = true;
            }
        }

        public override QueryBuilder OpenBracket()
        {
            this.endsWithWordBoundary = true;
            return base.OpenBracket();
        }

        public override QueryBuilder CloseBracket()
        {
            this.endsWithWordBoundary = true;
            return base.CloseBracket();
        }

        #region Keywords

        private QueryBuilder AppendKeywords(string keywords)
        {
            this.EnsureWordBoundary();
            this.Buffer.Append(keywords);
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendInsertIntoKeywords() => this.AppendKeywords("INSERT INTO");

        public override QueryBuilder AppendValuesKeyword() => this.AppendKeywords("VALUES");

        public override QueryBuilder AppendDeleteFromKeywords() => this.AppendKeywords("DELETE FROM");

        public override QueryBuilder AppendWhereKeyword() => this.AppendKeywords("WHERE");

        public override QueryBuilder AppendUpdateKeyword() => this.AppendKeywords("UPDATE");

        public override QueryBuilder AppendSetKeyword() => this.AppendKeywords("SET");

        public override QueryBuilder AppendFromKeyword() => this.AppendKeywords("FROM");

        public override QueryBuilder AppendAsKeyword() => this.AppendKeywords("AS");

        public override QueryBuilder AppendSelectKeyword() => this.AppendKeywords("SELECT");

        public override QueryBuilder AppendJoinKeyword() => this.AppendKeywords("JOIN");

        public override QueryBuilder AppendInnerKeyword() => this.AppendKeywords("INNER");

        public override QueryBuilder AppendLeftKeyword() => this.AppendKeywords("LEFT");

        public override QueryBuilder AppendOnKeyword() => this.AppendKeywords("ON");

        public override QueryBuilder AppendNull() => this.AppendKeywords("NULL");

        public override QueryBuilder AppendLimitClause(int limit, int offset)
        {
            string limitParameter = this.ResisterParameterWithValue(limit);

            this.AppendKeywords("LIMIT");

            if (offset == 0)
            {
                this.AppendRaw(limitParameter);
            }
            else
            {
                string offsetParameter = this.ResisterParameterWithValue(offset);

                this.AppendRaw(offsetParameter);
                this.AppendComma();
                this.AppendRaw(limitParameter);
            }

            return this;
        }

        #endregion

        #region Punctuation and operators

        public override QueryBuilder AppendAccessor()
        {
            this.Buffer.Append('.');
            this.endsWithWordBoundary = true;
            return this;
        }

        public override QueryBuilder AppendComma()
        {
            this.Buffer.Append(',');
            this.endsWithWordBoundary = true;
            return this;
        }

        public override QueryBuilder AppendComparisonOperator(ComparisonOperator op)
        {
            switch (op)
            {
                case ComparisonOperator.Equal:
                    this.Buffer.Append('=');
                    break;
                case ComparisonOperator.NotEqual:
                    this.Buffer.Append("<>");
                    break;
                case ComparisonOperator.LessThan:
                    this.Buffer.Append('<');
                    break;
                case ComparisonOperator.GreaterThan:
                    this.Buffer.Append('>');
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    this.Buffer.Append("<=");
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    this.Buffer.Append(">=");
                    break;
            }
            this.endsWithWordBoundary = true;
            return this;
        }

        public override QueryBuilder AppendArithmeticOperator(ArithmeticOperator op)
        {
            switch (op)
            {
                case ArithmeticOperator.Addition:
                    this.Buffer.Append('+');
                    break;
                case ArithmeticOperator.Substraction:
                    this.Buffer.Append('-');
                    break;
                case ArithmeticOperator.Multiplication:
                    this.Buffer.Append('*');
                    break;
                case ArithmeticOperator.Division:
                    this.Buffer.Append('/');
                    break;
                case ArithmeticOperator.Modulo:
                    this.Buffer.Append('%');
                    break;
            }
            this.endsWithWordBoundary = true;
            return this;
        }

        public override QueryBuilder AppendLogicalOperator(LogicalOperator op)
        {
            this.EnsureWordBoundary();
            switch (op)
            {
                case LogicalOperator.Not:
                    this.Buffer.Append("NOT");
                    break;
                case LogicalOperator.Or:
                    this.Buffer.Append("OR");
                    break;
                case LogicalOperator.And:
                    this.Buffer.Append("AND");
                    break;
            }
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendSetClauseAssignment()
        {
            this.Buffer.Append('=');
            this.endsWithWordBoundary = true;
            return this;
        }

        public override QueryBuilder AppendAllColumnsWildcard()
        {
            this.EnsureWordBoundary();
            this.Buffer.Append('*');
            this.endsWithWordBoundary = false;
            return this;
        }

        #endregion

        #region Misc

        public override QueryBuilder AppendTableOrColumnName(string tableOrColumn)
        {
            this.EnsureWordBoundary();
            this.Buffer.Append('`').Append(tableOrColumn).Append('`');
            this.endsWithWordBoundary = false;
            return this;
        }
        public override QueryBuilder AppendNamedParameter(string name)
        {
            this.EnsureWordBoundary();
            this.Buffer.Append('@').Append(name);
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendCommaSeparatedColumnNames(IEnumerable<string> columns)
        {
            this.EnsureWordBoundary();
            this.Buffer.Append('`')
                  .AppendJoin($"`,`", columns)
                  .Append('`');
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendCommaSeparatedNamedParameters(IEnumerable<string> parameters)
        {
            this.EnsureWordBoundary();
            this.Buffer.AppendJoin($",", parameters);
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendRaw(string sql)
        {
            this.EnsureWordBoundary();
            this.Buffer.Append(sql);
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendLastInsertIdQuery()
        {
            this.Buffer.Append("SELECT LAST_INSERT_ID()");
            return this;
        }

        #endregion
    }
}
