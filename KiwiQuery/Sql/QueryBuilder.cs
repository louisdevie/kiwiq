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
        private readonly StringBuilder buffer;
        private readonly DbCommand command;
        private int openBrackets;
        private bool endsWithWordBoundary;
        private int nextParameterId;

        /// <summary>
        /// The internal string builder used to write the SQL to.
        /// </summary>
        protected StringBuilder Buffer => this.buffer;

        /// <summary>
        /// Creates a new query builder attached to a <see cref="DbCommand"/>.
        /// </summary>
        /// <param name="command"></param>
        protected QueryBuilder(DbCommand command)
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
            this.EndsWithWordBoundary();
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
            this.EndsWithWordBoundary();
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

        /// <summary>Writes the accessor symbol (a dot by default).</summary>
        public virtual QueryBuilder AppendAccessor()
        {
            this.Buffer.Append('.');
            this.EndsWithWordBoundary();
            return this;
        }

        /// <summary>
        /// Adds a space to the query to ensure that two words are separated.
        /// </summary>
        protected void EnsureWordBoundary()
        {
            if (!this.endsWithWordBoundary)
            {
                this.Buffer.Append(' ');
                this.EndsWithWordBoundary();
            }
        }

        /// <summary>
        /// Indicate that a word <em>MAY</em> be written directly after the last character.
        /// </summary>
        protected void EndsWithWordBoundary()
        {
            this.endsWithWordBoundary = true;
        }

        /// <summary>
        /// Indicate that a word <em>CAN NOT</em> be written directly after the last character.
        /// </summary>
        protected void DoesntEndWithWordBoundary()
        {
            this.endsWithWordBoundary = false;
        }

        #region Keywords

        private QueryBuilder AppendKeywords(string keywords)
        {
            this.EnsureWordBoundary();
            this.Buffer.Append(keywords);
            this.DoesntEndWithWordBoundary();
            return this;
        }

        /// <summary>Writes the INSERT INTO keywords.</summary>
        public virtual QueryBuilder AppendInsertIntoKeywords() => this.AppendKeywords("INSERT INTO");

        /// <summary>Writes the VALUES keyword.</summary>
        public virtual QueryBuilder AppendValuesKeyword() => this.AppendKeywords("VALUES");

        /// <summary>Writes the DELETE FROM keywords.</summary>
        public virtual QueryBuilder AppendDeleteFromKeywords() => this.AppendKeywords("DELETE FROM");

        /// <summary>Writes the WHERE keyword.</summary>
        public virtual QueryBuilder AppendWhereKeyword() => this.AppendKeywords("WHERE");

        /// <summary>Writes the UPDATE keyword.</summary>
        public virtual QueryBuilder AppendUpdateKeyword() => this.AppendKeywords("UPDATE");

        /// <summary>Writes the SET keyword.</summary>
        public virtual QueryBuilder AppendSetKeyword() => this.AppendKeywords("SET");

        /// <summary>Writes the FROM keyword.</summary>
        public virtual QueryBuilder AppendFromKeyword() => this.AppendKeywords("FROM");

        /// <summary>Writes the AS keyword.</summary>
        public virtual QueryBuilder AppendAsKeyword() => this.AppendKeywords("AS");

        /// <summary>Writes the SELECT keyword.</summary>
        public virtual QueryBuilder AppendSelectKeyword() => this.AppendKeywords("SELECT");

        /// <summary>Writes the JOIN keyword.</summary>
        public virtual QueryBuilder AppendJoinKeyword() => this.AppendKeywords("JOIN");

        /// <summary>Writes the INNER keyword.</summary>
        public virtual QueryBuilder AppendInnerKeyword() => this.AppendKeywords("INNER");

        /// <summary>Writes the LEFT keyword.</summary>
        public virtual QueryBuilder AppendLeftKeyword() => this.AppendKeywords("LEFT");

        /// <summary>Writes the ON keyword.</summary>
        public virtual QueryBuilder AppendOnKeyword() => this.AppendKeywords("ON");

        /// <summary>Write the NULL constant.</summary>
        public virtual QueryBuilder AppendNull() => this.AppendKeywords("NULL");

        /// <summary>Write a LIMIT clause (some dialects may use the OFFSET keyword).</summary>
        /// <param name="limit">The maximum number of rows the query should return.</param>
        /// <param name="offset">The row offset, starting at 0.</param>
        public virtual QueryBuilder AppendLimitClause(int limit, int offset)
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
        
        /// <summary>Writes the DISTINCT keyword.</summary>
        public virtual QueryBuilder AppendDistinctKeyword() => this.AppendKeywords("DISTINCT");

        #endregion

        #region Punctuation and operators

        /// <summary>Writes a comma.</summary>
        public virtual QueryBuilder AppendComma()
        {
            this.Buffer.Append(',');
            this.EndsWithWordBoundary();
            return this;
        }

        /// <summary>Writes a comparison operator.</summary>
        public virtual QueryBuilder AppendComparisonOperator(ComparisonOperator op)
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

            this.EndsWithWordBoundary();
            return this;
        }

        /// <summary>Writes an arithmetic operator.</summary>
        public virtual QueryBuilder AppendArithmeticOperator(ArithmeticOperator op)
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

            this.EndsWithWordBoundary();
            return this;
        }

        /// <summary>Writes a logical operator.</summary>
        public virtual QueryBuilder AppendLogicalOperator(LogicalOperator op)
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

            this.DoesntEndWithWordBoundary();
            return this;
        }

        /// <summary>Writes the symbol used to assign values in a SET statement (usually and equal sign).</summary>
        public virtual QueryBuilder AppendSetClauseAssignment()
        {
            this.Buffer.Append('=');
            this.EndsWithWordBoundary();
            return this;
        }

        /// <summary>Writes a wildcard that selects all the columns (usually <strong>*</strong> or <strong>ALL</strong>).</summary>
        public virtual QueryBuilder AppendAllColumnsWildcard()
        {
            this.EnsureWordBoundary();
            this.Buffer.Append('*');
            this.DoesntEndWithWordBoundary();
            return this;
        }

        #endregion

        #region Misc

        /// <summary>Writes the name of a table or a column. It should be escaped if it is a reserved keyword.</summary>
        public abstract QueryBuilder AppendTableOrColumnName(string tableOrColumn);

        /// <summary>Writes a named parameter. The <paramref name="name"/> is just the name without the leading @.</summary>
        public abstract QueryBuilder AppendNamedParameter(string name);

        /// <summary>Writes a sequence of column names separated by commas.</summary>
        public abstract QueryBuilder AppendCommaSeparatedColumnNames(IEnumerable<string> columns);

        /// <summary>
        /// Writes a sequence of named parameters separated by commas.
        /// The <paramref name="parameters"/> are just the names without the leading @.
        /// </summary>
        public abstract QueryBuilder AppendCommaSeparatedNamedParameters(IEnumerable<string> parameters);
        
        /// <summary>Writes SQL as is.</summary>
        public virtual QueryBuilder AppendRaw(string sql)
        {
            this.EnsureWordBoundary();
            this.Buffer.Append(sql);
            this.DoesntEndWithWordBoundary();
            return this;
        }
        
        /// <summary>Writes a query that retrieves the primary key of the last inserted row.</summary>
        public virtual QueryBuilder AppendLastInsertIdQuery()
        {
            this.Buffer.Append("SELECT LAST_INSERT_ID()");
            return this;
        }

        /// <summary>Writes a constant value that is always considered "true".</summary>
        public abstract QueryBuilder AppendTruthyConstant();

        /// <summary>Writes a constant value that is always considered "false".</summary>
        public abstract QueryBuilder AppendFalsyConstant();

        #endregion
    }
}