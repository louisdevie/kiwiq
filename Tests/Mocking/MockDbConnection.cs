using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace KiwiQuery.Tests.Mocking
{
    internal class MockDbConnection : DbConnection
    {
        List<(ExecutionMethod, MockDbCommand)> executedCommands = new();
        private MockDbDataReader? results;
        private int mockLinesAffected = -1;
        private object? scalarResult;

        public void MockResults(IEnumerable<string> names, IEnumerable<Row> rows)
        {
            this.results = new MockDbDataReader(names, rows);
        }

        public void MockScalarResult(object? value)
        {
            this.scalarResult = value;
        }
        
        public void MockLinesAffected(int lineCount)
        {
            this.mockLinesAffected = lineCount;
        }

        public MockDbDataReader? Results => this.results;

        public object? ScalarResult => this.scalarResult;
        
        public int LinesAffected => this.mockLinesAffected;

        public override string ConnectionString
        {
            get => "";
            set { }
        }

        public override string Database => "";

        public override string DataSource => "";

        public override string ServerVersion => "";

        public override ConnectionState State => ConnectionState.Open;

        public MockDbCommand LastExecutedCommand => this.executedCommands[^1].Item2;

        public ExecutionMethod LastExecutionMethod => this.executedCommands[^1].Item1;

        public int ExecutedCommandCount => this.executedCommands.Count;

        public MockDbCommand GetNthCommandText(int number) => this.executedCommands[number].Item2;

        public ExecutionMethod GetNthExecutionMethod(int number) => this.executedCommands[number].Item1;

        public void ClearExecutionHistory()
        {
            this.executedCommands.Clear();
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        public void Execute(ExecutionMethod executionMethod, MockDbCommand commandText)
        {
            this.executedCommands.Add((executionMethod, commandText));
        }

        protected override DbCommand CreateDbCommand()
        {
            return new MockDbCommand(this);
        }
        
        public void CheckSelectCommandExecution(string expected, params object[] parameters)
        {
            Assert.Equal(1, this.ExecutedCommandCount);

            Assert.Equal(expected, this.LastExecutedCommand.CommandText);
            Assert.Equal(ExecutionMethod.Reader, this.LastExecutionMethod);
            Assert.Equal(parameters, this.LastExecutedCommand.MockParameters.Select(param => param.Value));

            this.ClearExecutionHistory();
        }
        
        public void CheckNonQueryExecution(string expected, object[] parameters)
        {
            Assert.Equal(1, this.ExecutedCommandCount);

            Assert.Equal(expected, this.LastExecutedCommand.CommandText);
            Assert.Equal(ExecutionMethod.NonQuery, this.LastExecutionMethod);
            Assert.Equal(parameters, this.LastExecutedCommand.MockParameters.Select(param => param.Value));

            this.ClearExecutionHistory();
        }
        
        public void CheckScalarExecution(int number, string expected)
        {
            Assert.True(number <= this.ExecutedCommandCount);

            var (method, command) = this.executedCommands[number - 1];
            Assert.Equal(expected, command.CommandText);
            Assert.Equal(ExecutionMethod.Scalar, method);
        }
        
        public void CheckNonQueryExecution(int number, string expected, object[] parameters)
        {
            Assert.True(number <= this.ExecutedCommandCount);

            var (method, command) = this.executedCommands[number - 1];
            Assert.Equal(expected, command.CommandText);
            Assert.Equal(ExecutionMethod.NonQuery, method);
            Assert.Equal(parameters, command.MockParameters.Select(param => param.Value));
        }

        public void ExpectNoMoreThan(int count)
        {
            Assert.Equal(count, this.ExecutedCommandCount);
            this.ClearExecutionHistory();
        }
        
        public string GetSingleSelectCommand()
        {
            Assert.Equal(1, this.ExecutedCommandCount);
            string command = this.LastExecutedCommand.CommandText;
            this.ClearExecutionHistory();
            return command;
        }
    }
}