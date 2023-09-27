using KiwiQuery.Expressions;
using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery
{
    public class Schema
    {
        private DbConnection connection;
        private QueryBuilderFactory qbFactory;

        public DbConnection Connection => this.connection;

        internal QueryBuilderFactory QueryBuilderFactory => this.qbFactory;

        public Schema(DbConnection connection, Mode mode)
        {
            this.connection = connection;
            this.qbFactory = new QueryBuilderFactory(mode);
        }
        public InsertQuery InsertInto(string table) => new(table, this);

        public DeleteQuery DeleteFrom(string table) => new(table, this);

        public UpdateQuery Update(string table) => new(table, this);

        public SelectQuery Select(params string[] columns) => new(columns.Select(col => this.Column(col)), this);

        public SelectQuery Select(params Column[] columns) => new(columns, this);

        public SelectQuery SelectAll() => new(Array.Empty<Column>(), this);

        public Table Table(string name) => new(name, this);

        public Column Column(string name) => new(name, this);
    }
}
