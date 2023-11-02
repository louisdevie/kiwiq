using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using System.Data.Common;
using System.Text;

namespace KiwiQuery.Sql
{
    public abstract class QueryBuilder
    {
        private StringBuilder buffer;
        private int openBrackets;
        private DbCommand command;
        private int nextParameterId;

        public StringBuilder Buffer => this.buffer;

        public QueryBuilder(DbCommand command)
        {
            this.buffer = new StringBuilder();
            this.openBrackets = 0;
            this.command = command;
            this.nextParameterId = 1;
        }
        
        public string RegisterParameter(DbParameter parameter)
        {
            string parameterName = $"@p{this.nextParameterId}";
            this.nextParameterId++;
            parameter.ParameterName = parameterName;
            this.command.Parameters.Add(parameter);
            return parameterName;
        }

        public string ResisterParameterWithValue(object? parameterValue)
        {
            DbParameter param = this.command.CreateParameter();
            param.Value = parameterValue;
            return this.RegisterParameter(param);
        }

        public virtual QueryBuilder OpenBracket()
        {
            openBrackets++;
            buffer.Append('(');
            return this;
        }

        public virtual QueryBuilder CloseBracket()
        {
            if (openBrackets <= 0)
            {
                throw new InvalidOperationException("There is no matching bracket.");
            }
            openBrackets--;
            buffer.Append(')');
            return this;
        }

        public QueryBuilder AppendCommaSeparatedElements(IEnumerable<IWriteable> elements)
        {
            bool firstElement = true;
            foreach (IWriteable element in elements)
            {
                if (firstElement)
                {
                    firstElement = false;
                }
                else
                {
                    this.AppendComma();
                }
                element.WriteTo(this);
            }
            return this;
        }

        public override string ToString()
        {
            if (this.openBrackets > 0)
            {
                throw new InvalidOperationException("There is a stray unclosed bracket.");
            }
            return this.Buffer.ToString();
        }

        public abstract QueryBuilder AppendLastInsertIdQuery();
        public abstract QueryBuilder AppendAccessor();
        public abstract QueryBuilder AppendCommaSeparatedColumnNames(IEnumerable<string> columns);
        public abstract QueryBuilder AppendCommaSeparatedNamedParameters(IEnumerable<string> parameters);
        public abstract QueryBuilder AppendComparisonOperator(ComparisonOperator op);
        public abstract QueryBuilder AppendDeleteFromKeywords();
        public abstract QueryBuilder AppendInsertIntoKeywords();
        public abstract QueryBuilder AppendRaw(string sql);
        public abstract QueryBuilder AppendTableOrColumnName(string tableOrColumn);
        public abstract QueryBuilder AppendValuesKeyword();
        public abstract QueryBuilder AppendWhereKeyword();
        public abstract QueryBuilder AppendUpdateKeyword();
        public abstract QueryBuilder AppendSetKeyword();
        public abstract QueryBuilder AppendComma();
        public abstract QueryBuilder AppendSetClauseAssignment();
        public abstract QueryBuilder AppendAsKeyword();
        public abstract QueryBuilder AppendAllColumnsWildcard();
        public abstract QueryBuilder AppendFromKeyword();
        public abstract QueryBuilder AppendSelectKeyword();
        public abstract QueryBuilder AppendJoinKeyword();
        public abstract QueryBuilder AppendInnerKeyword();
        public abstract QueryBuilder AppendLeftKeyword();
        public abstract QueryBuilder AppendOnKeyword();
        public abstract QueryBuilder AppendNamedParameter(string name);
        public abstract QueryBuilder AppendArithmeticOperator(ArithmeticOperator op);
        public abstract QueryBuilder AppendNull();
        public abstract QueryBuilder AppendLogicalOperator(LogicalOperator op);
    }
}