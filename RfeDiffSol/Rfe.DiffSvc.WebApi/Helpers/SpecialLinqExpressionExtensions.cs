using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;



namespace Rfe.DiffSvc.WebApi.Helpers
{



    /// <summary>
    /// Exposes "and" and "or" operators to be used in dynamically built LINQ queries.
    /// Taken from: https://gist.github.com/akunzai/49506f9cd2e07136c75ec581159ce0dd
    /// </summary>
    public static class ExpressionExtensions
    {



        /// <summary>
        /// Combines two LINQ expressions (used in a WHERE clause) with a logical AND.
        /// </summary>
        /// <typeparam name="T">Type of the entity the expressions are used on.</typeparam>
        /// <param name="left">Left LINQ expression ("this").</param>
        /// <param name="right">Right LINQ expression.</param>
        /// <returns>Returns an expression which is sort-of "left AND right".</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) return right;
            var and = Expression.AndAlso(left.Body, right.Body);
            return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
        }



        /// <summary>
        /// Combines two LINQ expressions (for WHERE clause) with a logical OR.
        /// </summary>
        /// <typeparam name="T">Type of the entity involved.</typeparam>
        /// <param name="left">Left LINQ expression ("this").</param>
        /// <param name="right">Right LINQ expression.</param>
        /// <returns>Returns an expression which is sort-of "left OR right".</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null) return right;
            var and = Expression.OrElse(left.Body, right.Body);
            return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
        }



    }



}
