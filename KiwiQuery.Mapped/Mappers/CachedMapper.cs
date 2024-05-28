using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace KiwiQuery.Mapped.Mappers
{
    internal abstract class CachedMapper : IMapper
    {
        private static readonly Dictionary<Type, CachedMapper> Instances = new Dictionary<Type, CachedMapper>();

        public static CachedMapper<T> For<T>()
        where T : notnull
        {
            CachedMapper<T> mapper;
            if (Instances.TryGetValue(typeof(T), out CachedMapper found))
            {
                mapper = (CachedMapper<T>)found;
            }
            else
            {
                mapper = new CachedMapper<T>();
                Instances.Add(typeof(T), mapper);
            }

            return mapper;
        }

        private readonly string tableName;
        private readonly List<FieldInfo> objectFields;
        private readonly List<string> columns;
        private readonly ConstructorInfo constructor;

        public CachedMapper(Type type)
        {
            this.constructor = type.GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null,
                Array.Empty<Type>(), null
            ) ?? throw new ArgumentException($"No default constructor found for in {type.FullName ?? type.Name}");

            this.tableName = GetTableName(type);

            this.objectFields = type.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly
            ).Where(
                field => !field.GetCustomAttributes<NotStoredAttribute>().Any()
            ).ToList();

            this.columns = this.objectFields.Select(GetColumnName).ToList();
        }

        private static string GetTableName(Type type)
        {
            string name = type.Name;
            foreach (TableAttribute attr in type.GetCustomAttributes<TableAttribute>())
            {
                name = attr.Name;
            }

            return name;
        }

        private static string GetColumnName(FieldInfo field)
        {
            string fieldName = field.Name;
            foreach (ColumnAttribute attr in field.GetCustomAttributes<ColumnAttribute>())
            {
                fieldName = attr.Name;
            }

            return fieldName;
        }

        public string TableName => this.tableName;

        public IEnumerable<string> Columns => this.columns;

        public object RowToObject(DbDataReader reader)
        {
            object instance = this.constructor.Invoke(Array.Empty<object>());
            for (int i = 0; i < this.objectFields.Count; i++)
            {
                this.objectFields[i].SetValue(instance, reader.GetValue(i));
            }

            return instance;
        }
    }

    internal class CachedMapper<T> : CachedMapper, IMapper<T>
    where T : notnull
    {
        public CachedMapper() : base(typeof(T))
        {
        }

        public new T RowToObject(DbDataReader reader)
        {
            return (T)base.RowToObject(reader);
        }
    }
}