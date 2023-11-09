using KiwiQuery.Expressions;
using KiwiQuery.Expressions.Predicates;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace KiwiQuery.Sql
{
    /// <summary>
    /// The class responsible for generating SQL. <br/>
    /// Instances of this class are constructed through the <see cref="QueryBuilderFactory"/>.
    /// </summary>
    public abstract class QueryBuilder
    {
        private StringBuilder buffer;
        private int openBrackets;
        private DbCommand command;
        private int nextParameterId;

        /// <summary>
        /// The internal string builder used to write the SQL to.
        /// </summary>
        protected StringBuilder Buffer => this.buffer;

        /// <summary>
        /// Creates a new query builder attached to a <see cref="DbCommand"/>.
        /// </summary>
        /// <param name="command"></param>
        public QueryBuilder(DbCommand command)
        {
            this.buffer = new StringBuilder();
            this.openBrackets = 0;
            this.command = command;
            this.nextParameterId = 1;
        }

        /// <summary>
        /// Generates a name for the given <see cref="DbParameter"/> ands add it to the current command.
        /// </summary>
        /// <param name="parameter">The parameter to add. Its name will be overrriden.</param>
        /// <returns>The generated name for the parameter, <em>including the leading @</em>.</returns>
        public string RegisterParameter(DbParameter parameter)
        {
            string parameterName = $"@p{this.nextParameterId}";
            this.nextParameterId++;
            parameter.ParameterName = parameterName;
            this.command.Parameters.Add(parameter);
            return parameterName;
        }

        /// <summary>
        /// Generates a named parameter containing the given value ands add it to the current command.
        /// </summary>
        /// <param name="parameterValue">The value to use for the parameter.</param>
        /// <returns>The generated name for the parameter, <em>including the leading @</em>.</returns>
        public string ResisterParameterWithValue(object? parameterValue)
        {
            DbParameter param = this.command.CreateParameter();
            param.Value = parameterValue;
            return this.RegisterParameter(param);
        }

        /// <summary>
        /// Opens a bracket :
        /// <code>
        /// (
        /// </code>
        /// </summary>
        public virtual QueryBuilder OpenBracket()
        {
            this.openBrackets++;
            this.buffer.Append('(');
            return this;
        }

        /// <summary>
        /// Closes a bracket :
        /// <code>
        /// )
        /// </code>
        /// </summary>
        public virtual QueryBuilder CloseBracket()
        {
            if (this.openBrackets <= 0)
            {
                throw new InvalidOperationException("There is no matching bracket.");
            }
            this.openBrackets--;
            this.buffer.Append(')');
            return this;
        }

        /// <summary>
        /// Writes a list of <see cref="IWriteable"/>s elements separated by commas.
        /// </summary>
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

        /// <summary>
        /// Gets the whole generated SQL command as text.<br/>
        /// This method will throw an <see cref="InvalidOperationException"/> if not all brackets are closed.
        /// </summary>
        public override string ToString()
        {
            if (this.openBrackets > 0)
            {
                throw new InvalidOperationException("There is a stray unclosed bracket.");
            }
            return this.Buffer.ToString();
        }

        /// <summary>Writes the accessor symbol (usually a dot).</summary>
        public abstract QueryBuilder AppendAccessor();
        
        /// <summary>Writes a query that retrieves the primary key of the last inserted row.</summary>
        public abstract QueryBuilder AppendLastInsertIdQuery();

        /// <summary>Writes a sequence of column names separated by commas.</summary>
        public abstract QueryBuilder AppendCommaSeparatedColumnNames(IEnumerable<string> columns);

        /// <summary>
        /// Writes a sequence of named parameters separated by commas.
        /// The <paramref name="parameters"/> are just the names without the leading @.
        /// </summary>
        public abstract QueryBuilder AppendCommaSeparatedNamedParameters(IEnumerable<string> parameters);
        
        /// <summary>Writes a comparison operator.</summary>
        public abstract QueryBuilder AppendComparisonOperator(ComparisonOperator op);

        /// <summary>Writes the DELETE FROM keywords.</summary>
        public abstract QueryBuilder AppendDeleteFromKeywords();
        
        /// <summary>Writes the INSERT INTO keywords.</summary>
        public abstract QueryBuilder AppendInsertIntoKeywords();
        
        /// <summary>Writes SQL as is.</summary>
        public abstract QueryBuilder AppendRaw(string sql);

        /// <summary>Writes the name of a table or a column. It should be escaped if it is a reserved keyword.</summary>
        public abstract QueryBuilder AppendTableOrColumnName(string tableOrColumn);

        /// <summary>Writes the VALUES keyword.</summary>
        public abstract QueryBuilder AppendValuesKeyword();

        /// <summary>Writes the WHERE keyword.</summary>
        public abstract QueryBuilder AppendWhereKeyword();

        /// <summary>Writes the UPDATE keyword.</summary>
        public abstract QueryBuilder AppendUpdateKeyword();

        /// <summary>Writes the SET keyword.</summary>
        public abstract QueryBuilder AppendSetKeyword();

        /// <summary>Writes a comma.</summary>
        public abstract QueryBuilder AppendComma();

        /// <summary>Writes the symbol used to assign values in a SET statement (usually and equal sign).</summary>
        public abstract QueryBuilder AppendSetClauseAssignment();
        
        /// <summary>Writes the AS keyword.</summary>
        public abstract QueryBuilder AppendAsKeyword();

        /// <summary>Writes a wildcard that selects all the columns (usually <strong>*</strong> or <strong>ALL</strong>).</summary>
        public abstract QueryBuilder AppendAllColumnsWildcard();

        /// <summary>Writes the FROM keyword.</summary>
        public abstract QueryBuilder AppendFromKeyword();

        /// <summary>Writes the SELECT keyword.</summary>
        public abstract QueryBuilder AppendSelectKeyword();

        /// <summary>Writes the JOIN keyword.</summary>
        public abstract QueryBuilder AppendJoinKeyword();

        /// <summary>Writes the INNER keyword.</summary>
        public abstract QueryBuilder AppendInnerKeyword();

        /// <summary>Writes the LEFT keyword.</summary>
        public abstract QueryBuilder AppendLeftKeyword();

        /// <summary>Writes the ON keyword.</summary>
        public abstract QueryBuilder AppendOnKeyword();

        /// <summary>Writes a named parameter. The <paramref name="name"/> is just the name without the leading @.</summary>
        public abstract QueryBuilder AppendNamedParameter(string name);

        /// <summary>Writes an arithmetic operator.</summary>
        public abstract QueryBuilder AppendArithmeticOperator(ArithmeticOperator op);

        /// <summary>Write the NULL constant.</summary>
        public abstract QueryBuilder AppendNull();

        /// <summary>Writes a logical operator.</summary>
        public abstract QueryBuilder AppendLogicalOperator(LogicalOperator op);

        /// <summary>Write a LIMIT clause (some dialect may use the OFFSET keyword).</summary>
        /// <param name="limit">The maximum number of rows the query should return.</param>
        /// <param name="offset">The row offset, starting at 0.</param>
        public abstract QueryBuilder AppendLimitClause(int limit, int offset);
    }
}