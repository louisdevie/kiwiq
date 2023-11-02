using KiwiQuery.Sql;

namespace KiwiQuery
{
    /// <summary>
    /// The different modes that can be used. Currently, only MySQL is supported,
    /// but you can add your own backend using <see cref="QueryBuilderFactory.RegisterCustomQueryBuilder(Type)"/>.
    /// </summary>
    public enum Mode
    {
        MySql
    }
}
