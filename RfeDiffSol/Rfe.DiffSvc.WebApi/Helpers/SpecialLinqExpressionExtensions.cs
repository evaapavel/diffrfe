using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Query;



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
            //if (left == null) return right;

            // Let's handle the singular cases first.
            if (left == null)
            {
                return right;
            }

            if (right == null)
            {
                return left;
            }

            // From now on, we know that both left and right IS something.

            // This causes an issue:
            //var and = Expression.AndAlso(left.Body, right.Body);
            //return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());

            // This does not work either:

            //// So - let's do it a more complicated way.
            //BinaryExpression andExpr = Expression.AndAlso(left.Body, right.Body);
            //Expression<Func<T, bool>> origResult = Expression.Lambda<Func<T, bool>>(andExpr, left.Parameters.Single());

            //// Now perform a parameter replacement.
            //ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            //Expression newBody = ReplacingExpressionVisitor.Replace(origResult.Parameters.Single(), paramExpr, origResult.Body);

            //// Prepare a new result.
            //Expression<Func<T, bool>> newResult = Expression.Lambda<Func<T, bool>>(newBody, paramExpr);
            //return newResult;

            // My guess for now:

            // Prepare a new parameter object.
            ParameterExpression paramExpr = Expression.Parameter(typeof(T));

            // Replace a parameter in the left expression.
            Expression leftReplaced = ReplacingExpressionVisitor.Replace(left.Parameters.Single(), paramExpr, left.Body);

            // Replace a parameter in the right expression.
            Expression rightReplaced = ReplacingExpressionVisitor.Replace(right.Parameters.Single(), paramExpr, right.Body);

            // Generate new lambdas out of the replaced expressions.
            Expression<Func<T, bool>> leftLambdaReplaced = Expression.Lambda<Func<T, bool>>(leftReplaced, paramExpr);
            Expression<Func<T, bool>> rightLambdaReplaced = Expression.Lambda<Func<T, bool>>(rightReplaced, paramExpr);

            // Combine bodies of the new lambdas with AND.
            BinaryExpression andExpr = Expression.AndAlso(leftLambdaReplaced.Body, rightLambdaReplaced.Body);

            // Prepare a result.
            Expression<Func<T, bool>> result = Expression.Lambda<Func<T, bool>>(andExpr, paramExpr);

            // Return the result.
            return result;
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
            //if (left == null) return right;

            // From now on, we know that both left and right IS something.

            // Let's handle the singular cases first.
            if (left == null)
            {
                return right;
            }

            if (right == null)
            {
                return left;
            }

            // This causes an issue:
            //var and = Expression.OrElse(left.Body, right.Body);
            //return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());

            // This does not work either:

            //// Well, let's take another way.
            //BinaryExpression orExpr = Expression.OrElse(left.Body, right.Body);
            //Expression<Func<T, bool>> origResult = Expression.Lambda<Func<T, bool>>(orExpr, left.Parameters.Single());

            //// Now perform a parameter replacement.
            //ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            //Expression newBody = ReplacingExpressionVisitor.Replace(origResult.Parameters.Single(), paramExpr, origResult.Body);

            //// Prepare a new result.
            //Expression<Func<T, bool>> newResult = Expression.Lambda<Func<T, bool>>(newBody, paramExpr);
            //return newResult;

            // My guess for now:

            // Prepare a new parameter object.
            ParameterExpression paramExpr = Expression.Parameter(typeof(T));

            // Replace a parameter in the left expression.
            Expression leftReplaced = ReplacingExpressionVisitor.Replace(left.Parameters.Single(), paramExpr, left.Body);

            // Replace a parameter in the right expression.
            Expression rightReplaced = ReplacingExpressionVisitor.Replace(right.Parameters.Single(), paramExpr, right.Body);

            // Generate new lambdas out of the replaced expressions.
            Expression<Func<T, bool>> leftLambdaReplaced = Expression.Lambda<Func<T, bool>>(leftReplaced, paramExpr);
            Expression<Func<T, bool>> rightLambdaReplaced = Expression.Lambda<Func<T, bool>>(rightReplaced, paramExpr);

            // Combine bodies of the new lambdas with OR.
            BinaryExpression orExpr = Expression.OrElse(leftLambdaReplaced.Body, rightLambdaReplaced.Body);

            // Prepare a result.
            Expression<Func<T, bool>> result = Expression.Lambda<Func<T, bool>>(orExpr, paramExpr);

            // Return the result.
            return result;
        }



    }



}
