using KiwiQuery.Expressions;
using KiwiQuery.Sql;
using System.Data.Common;

namespace KiwiQuery
{
    public class Schema
    {
        private static Mode defaultMode = Mode.MySql;

        public static void SetDefaultMode(Mode mode) => defaultMode = mode;

        private DbConnection connection;
        private QueryBuilderFactory qbFactory;

        public DbConnection Connection => this.connection;

        internal QueryBuilderFactory QueryBuilderFactory => this.qbFactory;

        public Schema(DbConnection connection, Mode mode)
        {
            this.connection = connection;
            this.qbFactory = new QueryBuilderFactory(mode);
        }

        public Schema(DbConnection connection) : this(connection, defaultMode) { }

        public InsertQuery InsertInto(string table) => new(table, this);

        public DeleteQuery DeleteFrom(string table) => new(table, this);

        public UpdateQuery Update(string table) => new(table, this);

        public SelectQuery Select(params string[] columns) => new(columns.Select(col => this.Column(col)), this);

        public SelectQuery Select(params Value[] columns) => new(columns, this);

        public SelectQuery SelectAll() => new(Array.Empty<Column>(), this);

        public Table Table(string name) => new(name, this);

        public Column Column(string name) => new(name, this);
    }
}
