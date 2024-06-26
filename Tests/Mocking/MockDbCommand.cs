﻿using System.Data;
using System.Data.Common;

namespace KiwiQuery.Tests.Mocking
{
    internal class MockDbCommand : DbCommand
    {
        private MockDbConnection connection;
        private MockDbParameterCollection parameters;

        public override string CommandText { get; set; }

        public override int CommandTimeout { get; set; }

        public override CommandType CommandType { get; set; }
        
        public override bool DesignTimeVisible { get; set; }
        
        public override UpdateRowSource UpdatedRowSource { get; set; }
        
        protected override DbConnection? DbConnection { get => this.connection; set => throw new InvalidOperationException("Cannot change the connection of a mock command"); }

        protected override DbParameterCollection DbParameterCollection => this.parameters;

        public MockDbParameterCollection MockParameters => this.parameters;

        protected override DbTransaction? DbTransaction { get; set; }

        public MockDbCommand(MockDbConnection connection)
        {
            this.connection = connection;
            this.parameters = new MockDbParameterCollection();
            this.CommandText = string.Empty;
        }

        public MockDbCommand(): this(new MockDbConnection()) { }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            this.connection.Execute(ExecutionMethod.NonQuery, this);
            return this.connection.LinesAffected;
        }

        public override object? ExecuteScalar()
        {
            this.connection.Execute(ExecutionMethod.Scalar, this);
            return this.connection.ScalarResult;
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            return new MockDbParameter();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            this.connection.Execute(ExecutionMethod.Reader, this);
            return this.connection.Results!;
        }
    }
}
