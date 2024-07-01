using System;
using System.Collections.Generic;
using KiwiQuery.Sql;

namespace KiwiQuery
{
    /// <summary>
    /// The different modes that can be used. Currently, only MySQL is supported,
    /// but you can add your own backend using <see cref="QueryBuilderFactory.RegisterCustomQueryBuilder(Type)"/>.
    /// </summary>
    public readonly struct Dialect : IEquatable<Dialect>
    {
        /// <summary>
        /// The built-in MySQL dialect.
        /// </summary>
        public static readonly Dialect MySql = new Dialect("@@mysql");
        
        /// <summary>
        /// The built-in SQLite dialect.
        /// </summary>
        public static readonly Dialect Sqlite = new Dialect("@@sqlite");
        
        private readonly String identifier;

        private Dialect(String identifier)
        {
            this.identifier = identifier;
        }

        /// <inheritdoc/>
        public bool Equals(Dialect other)
        {
            return this.identifier == other.identifier;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj.GetType() == this.GetType() && this.Equals((Dialect)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.identifier.GetHashCode();
        }

        /// <summary>
        /// Compare two <see cref="Dialect"/>s through their <see cref="Equals(KiwiQuery.Dialect)"/> method.
        /// </summary>
        public static bool operator ==(Dialect left, Dialect right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Compare two <see cref="Dialect"/>s through their <see cref="Equals(KiwiQuery.Dialect)"/> method.
        /// </summary>
        public static bool operator !=(Dialect left, Dialect right)
        {
            return !Equals(left, right);
        }

        internal static Dialect ForClass(Type implementation)
        {
            return new Dialect(implementation.FullName ?? implementation.Name);
        }
    }
}
