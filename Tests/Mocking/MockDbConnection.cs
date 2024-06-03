using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace KiwiQuery.Tests.Mocking
{
    internal class MockDbConnection : DbConnection
    {
        List<(ExecutionMethod, MockDbCommand)> executedCommands = new();
        private MockDbDataReader? results = null;

        public void MockResults(IEnumerable<string> names, IEnumerable<Row> rows)
        {
            this.results = new MockDbDataReader(names, rows);
        }

        public MockDbDataReader? Results => this.results;

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
    }

    internal static class DbConnectionExtensions
    {
        public static void CheckSelectQueryExecution(this MockDbConnection connection, string expected)
        {
            Assert.Equal(1, connection.ExecutedCommandCount);

            Assert.Equal(expected, connection.LastExecutedCommand.CommandText);
            Assert.Equal(ExecutionMethod.Reader, connection.LastExecutionMethod);

            connection.ClearExecutionHistory();
        }
        
        public static string GetSingleSelectQuery(this MockDbConnection connection)
        {
            Assert.Equal(1, connection.ExecutedCommandCount);
            string command = connection.LastExecutedCommand.CommandText;
            connection.ClearExecutionHistory();
            return command;
        }
    }
}