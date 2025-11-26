using System.Linq.Expressions;

namespace LogisticsPlatform.Infrastructure.Extensions
{
    public static class OrderByDynamicExtension
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string property)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = Expression.PropertyOrField(param, property);
            var keySelector = Expression.Lambda(body, param);

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), body.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, keySelector })!;
        }

        public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string property)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = Expression.PropertyOrField(param, property);
            var keySelector = Expression.Lambda(body, param);

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), body.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, keySelector })!;
        }
    }
}
