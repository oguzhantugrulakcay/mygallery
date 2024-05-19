using System.Linq.Dynamic.Core;
using mygallery.Data;

namespace mygallery.Extensions
{
    public static class QueryableExtensions
    {
        public static PageableData<T> ToGridJson<T>(this IQueryable<T> query, string sort, int skip, int take)
        {
            return new PageableData<T>
            {
                count = query.Count(),
                data = query.OrderBy(sort).Skip(skip).Take(take)
                        .ToList()
            };
        }

        public static string ToCsv<T>(this IQueryable<T> source, string separator = ",")
        {
            if (source == null) throw new ArgumentNullException("source");

            return string.Join(separator, source.Select(s => s.ToString()).ToArray());
        }

        public static string ToCsv<T>(this IEnumerable<T> source, string separator = ",")
        {
            if (source == null) throw new ArgumentNullException("source");

            return string.Join(separator, source.Select(s => s.ToString()).ToArray());
        }
    }
}
