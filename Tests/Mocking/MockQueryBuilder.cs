using System.Data.Common;
using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using KiwiQuery.Sql;

namespace KiwiQuery.Tests.Mocking
{
    internal class MockQueryBuilder : QueryBuilder
    {
        private static Dialect? mode;

        public static Dialect MockDialect => mode ??= QueryBuilderFactory.Current.RegisterCustomQueryBuilder<MockQueryBuilder>();

        private bool firstToken = true;

        public MockQueryBuilder(DbCommand command) : base(command) { }

        public MockQueryBuilder() : base(new MockDbCommand()) { }

        private void Space()
        {
            if (this.firstToken)
            {
                this.firstToken = false;
            }
            else
            {
                this.Buffer.Append(" ");
            }
        }

        public override QueryBuilder OpenBracket()
        {
            this.Space();
            return base.OpenBracket();
        }

        public override QueryBuilder CloseBracket()
        {
            this.Space();
            return base.CloseBracket();
        }

        public override QueryBuilder AppendAccessor()
        {
            this.Space();
            this.Buffer.Append("->");
            return this;
        }

        public override QueryBuilder AppendAllColumnsWildcard()
        {
            this.Space();
            this.Buffer.Append("#all");
            return this;
        }

        public override QueryBuilder AppendArithmeticOperator(ArithmeticOperator op)
        {
            this.Space();
            switch (op)
            {
                case ArithmeticOperator.Addition:
                    this.Buffer.Append('+');
                    break;
                case ArithmeticOperator.Division:
                    this.Buffer.Append('/');
                    break;
                case ArithmeticOperator.Modulo:
                    this.Buffer.Append('%');
                    break;
                case ArithmeticOperator.Multiplication:
                    this.Buffer.Append('*');
                    break;
                case ArithmeticOperator.Substraction:
                    this.Buffer.Append('-');
                    break;
            }
            return this;
        }

        public override QueryBuilder AppendAsKeyword()
        {
            this.Space();
            this.Buffer.Append("as");
            return this;
        }

        public override QueryBuilder AppendComma()
        {
            this.Space();
            this.Buffer.Append(',');
            return this;
        }

        public override QueryBuilder AppendCommaSeparatedColumnNames(IEnumerable<string> columns)
        {
            this.Space();
            this.Buffer.AppendJoin(',', columns.Select(col => $"${col}"));
            return this;
        }

        public override QueryBuilder AppendCommaSeparatedNamedParameters(IEnumerable<string> parameters)
        {
            this.Space();
            this.Buffer.AppendJoin(',', parameters.Select(param => $"@{param}"));
            return this;
        }

        public override QueryBuilder AppendComparisonOperator(ComparisonOperator op)
        {
            this.Space();
            switch (op)
            {
                case ComparisonOperator.Equal:
                    this.Buffer.Append("==");
                    break;
                case ComparisonOperator.GreaterThan:
                    this.Buffer.Append('>');
                    break;
                case ComparisonOperator.GreaterThanOrEqual:
                    this.Buffer.Append(">=");
                    break;
                case ComparisonOperator.LessThan:
                    this.Buffer.Append('<');
                    break;
                case ComparisonOperator.LessThanOrEqual:
                    this.Buffer.Append("<=");
                    break;
                case ComparisonOperator.NotEqual:
                    this.Buffer.Append("!=");
                    break;
            }
            return this;
        }

        public override QueryBuilder AppendDeleteFromKeywords()
        {
            this.Space();
            this.Buffer.Append("delete-from");
            return this;
        }

        public override QueryBuilder AppendFromKeyword()
        {
            this.Space();
            this.Buffer.Append("from");
            return this;
        }

        public override QueryBuilder AppendInnerKeyword()
        {
            this.Space();
            this.Buffer.Append("inner");
            return this;
        }

        public override QueryBuilder AppendInsertIntoKeywords()
        {
            this.Space();
            this.Buffer.Append("insert-into");
            return this;
        }

        public override QueryBuilder AppendJoinKeyword()
        {
            this.Space();
            this.Buffer.Append("join");
            return this;
        }

        public override QueryBuilder AppendLastInsertIdQuery()
        {
            this.Space();
            this.Buffer.Append("select #last-insert-id");
            return this;
        }

        public override QueryBuilder AppendTruthyConstant()
        {
            this.Space();
            this.Buffer.Append("");
            return this;
        }

        public override QueryBuilder AppendFalsyConstant()
        {
            this.Space();
            this.Buffer.Append("select #last-insert-id");
            return this;
        }

        public override QueryBuilder AppendLeftKeyword()
        {
            this.Space();
            this.Buffer.Append("left");
            return this;
        }

        public override QueryBuilder AppendLogicalOperator(LogicalOperator op)
        {
            this.Space();
            switch (op)
            {
                case LogicalOperator.And:
                    this.Buffer.Append("&&");
                    break;
                case LogicalOperator.Not:
                    this.Buffer.Append('!');
                    break;
                case LogicalOperator.Or:
                    this.Buffer.Append("||");
                    break;
            }
            return this;
        }

        public override QueryBuilder AppendNamedParameter(string name)
        {
            this.Space();
            this.Buffer.Append('@').Append(name);
            return this;
        }

        public override QueryBuilder AppendNull()
        {
            this.Space();
            this.Buffer.Append("#null");
            return this;
        }

        public override QueryBuilder AppendOnKeyword()
        {
            this.Space();
            this.Buffer.Append("on");
            return this;
        }

        public override QueryBuilder AppendRaw(string sql)
        {
            this.Space();
            this.Buffer.Append(sql);
            return this;
        }

        public override QueryBuilder AppendSelectKeyword()
        {
            this.Space();
            this.Buffer.Append("select");
            return this;
        }

        public override QueryBuilder AppendSetClauseAssignment()
        {
            this.Space();
            this.Buffer.Append(":=");
            return this;
        }

        public override QueryBuilder AppendSetKeyword()
        {
            this.Space();
            this.Buffer.Append("set");
            return this;
        }

        public override QueryBuilder AppendTableOrColumnName(string tableOrColumn)
        {
            this.Space();
            this.Buffer.Append('$').Append(tableOrColumn);
            return this;
        }

        public override QueryBuilder AppendUpdateKeyword()
        {
            this.Space();
            this.Buffer.Append("update");
            return this;
        }

        public override QueryBuilder AppendValuesKeyword()
        {
            this.Space();
            this.Buffer.Append("values");
            return this;
        }

        public override QueryBuilder AppendWhereKeyword()
        {
            this.Space();
            this.Buffer.Append("where");
            return this;
        }

        public override QueryBuilder AppendLimitClause(int limit, int offset)
        {
            this.Space();
            string limitParam = this.ResisterParameterWithValue(limit);
            string offsetParam = this.ResisterParameterWithValue(offset);
            this.Buffer.Append($"limit {limitParam} offset {offsetParam}");
            return this;
        }

        public override QueryBuilder AppendDistinctKeyword()
        {
            this.Space();
            this.Buffer.Append("distinct");
            return this;
        }
    }
}
