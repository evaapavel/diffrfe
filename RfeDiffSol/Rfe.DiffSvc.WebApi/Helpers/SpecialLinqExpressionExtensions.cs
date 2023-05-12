using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rfe.DiffSvc.WebApi.Helpers
{
        public static class ExpressionExtensions
        {
            public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
            {
                if (left == null) return right;
                var and = Expression.AndAlso(left.Body, right.Body);
                return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
            }

            public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
            {
                if (left == null) return right;
                var and = Expression.OrElse(left.Body, right.Body);
                return Expression.Lambda<Func<T, bool>>(and, left.Parameters.Single());
            }
        }
}
