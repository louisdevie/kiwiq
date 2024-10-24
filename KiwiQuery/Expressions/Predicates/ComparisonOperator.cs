namespace KiwiQuery.Expressions.Predicates
{
    /// <summary>
    /// Operators that turn two values into a boolean result.
    /// </summary>
    public enum ComparisonOperator
    {
        /// <summary>
        /// The equality operator.
        /// </summary>
        Equal,
        
        /// <summary>
        /// The inequality operator. 
        /// </summary>
        NotEqual,
        
        /// <summary>
        /// The (strictly) less than operator.
        /// </summary>
        LessThan,
        
        /// <summary>
        /// The (strictly) greater than operator.
        /// </summary>
        GreaterThan,
        
        /// <summary>
        /// The less than or equal operator.
        /// </summary>
        LessThanOrEqual,
        
        /// <summary>
        /// The greater than or equal operator.
        /// </summary>
        GreaterThanOrEqual
    }

}
