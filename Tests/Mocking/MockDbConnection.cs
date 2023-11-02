using System.Data;
using System.Data.Common;

namespace Tests.Mocking
{
    internal class MockDbConnection : DbConnection
    {
        List<(ExecutionMethod, string)> executedCommands = new();

        public override string ConnectionString { get => ""; set { } }

        public override string Database => "";

        public override string DataSource => "";

        public override string ServerVersion => "";

        public override ConnectionState State => ConnectionState.Open;

        public string LastExecutedCommand => this.executedCommands[^1].Item2;

        public ExecutionMethod LastExecutionMethod => this.executedCommands[^1].Item1;

        public int ExecutedCommandCount => this.executedCommands.Count;

        public string GetNthCommandText(int number) => this.executedCommands[number].Item2;

        public ExecutionMethod GetNthExecutionMethod(int number) => this.executedCommands[number].Item1;

        public void ClearExecutionHistory()
        {
            this.executedCommands.Clear();
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public override void Close() {  throw new NotImplementedException(); }

        public override void Open() {  throw new NotImplementedException(); }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        public void Execute(ExecutionMethod executionMethod, string commandText)
        {
            this.executedCommands.Add((executionMethod, commandText));
        }

        protected override DbCommand CreateDbCommand()
        {
            return new MockDbCommand(this);
        }
    }
}
