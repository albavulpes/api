using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AlbaVulpes.API.Helpers
{
    public static class ReorderHelper
    {
        public static List<T> Reorder<T, TMember>(this List<T> items, Expression<Func<T, TMember>> memberLamda)
        {
            if (!(memberLamda.Body is MemberExpression memberSelectorExpression))
            {
                throw new InvalidOperationException("Lambda expression must be a member expression.");
            }

            var property = memberSelectorExpression.Member as PropertyInfo;

            if (property == null)
            {
                throw new MemberAccessException($"Member '{memberSelectorExpression.Member.Name}' is not a property.");
            }

            for (var i = 0; i < items.Count; i++)
            {
                property.SetValue(items[i], i + 1, null);
            }

            return items;
        }
    }
}