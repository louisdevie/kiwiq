using KiwiQuery.Expressions;
using KiwiQuery.Predicates;
using System.Data.Common;

namespace KiwiQuery.Sql
{
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
            EnsureWordBoundary();
            this.Buffer.Append(keywords);
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendInsertIntoKeywords() => AppendKeywords("INSERT INTO");

        public override QueryBuilder AppendValuesKeyword() => AppendKeywords("VALUES");

        public override QueryBuilder AppendDeleteFromKeywords() => AppendKeywords("DELETE FROM");

        public override QueryBuilder AppendWhereKeyword() => AppendKeywords("WHERE");

        public override QueryBuilder AppendUpdateKeyword() => AppendKeywords("UPDATE");

        public override QueryBuilder AppendSetKeyword() => AppendKeywords("SET");

        public override QueryBuilder AppendFromKeyword() => AppendKeywords("FROM");

        public override QueryBuilder AppendAsKeyword() => AppendKeywords("AS");

        public override QueryBuilder AppendSelectKeyword() => AppendKeywords("SELECT");

        public override QueryBuilder AppendJoinKeyword() => AppendKeywords("JOIN");

        public override QueryBuilder AppendInnerKeyword() => AppendKeywords("INNER");

        public override QueryBuilder AppendLeftKeyword() => AppendKeywords("LEFT");

        public override QueryBuilder AppendOnKeyword() => AppendKeywords("ON");

        public override QueryBuilder AppendNull() => AppendKeywords("NULL");


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
                case ArithmeticOperator.Plus:
                    this.Buffer.Append('+');
                    break;
                case ArithmeticOperator.Minus:
                    this.Buffer.Append("-");
                    break;
                case ArithmeticOperator.Times:
                    this.Buffer.Append('*');
                    break;
            }
            this.endsWithWordBoundary = true;
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
            EnsureWordBoundary();
            this.Buffer.Append('`').Append(tableOrColumn).Append('`');
            this.endsWithWordBoundary = false;
            return this;
        }
        public override QueryBuilder AppendNamedParameter(string name)
        {
            EnsureWordBoundary();
            this.Buffer.Append('@').Append(name);
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendCommaSeparatedColumnNames(IEnumerable<string> columns)
        {
            EnsureWordBoundary();
            this.Buffer.Append('`')
                  .AppendJoin($"`,`", columns)
                  .Append('`');
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendCommaSeparatedNamedParameters(IEnumerable<string> parameters)
        {
            EnsureWordBoundary();
            this.Buffer.AppendJoin($",", parameters);
            this.endsWithWordBoundary = false;
            return this;
        }

        public override QueryBuilder AppendRaw(string sql)
        {
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
