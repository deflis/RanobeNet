using System.Linq.Expressions;

namespace RanobeNet.Data
{
    public class Query<T>
    {
        public Expression<Func<T, dynamic>> KeySelector { get; }
        public bool Descending { get; }
        public int PageNumber { get; }
        public int PageSize { get; }

        public Query(Expression<Func<T, dynamic>> keySelector, bool descending, int pageNumber, int pageSize)
        {
            KeySelector = keySelector;
            Descending = descending;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
    public class QueryBuilder<T>
    {
        private Expression<Func<T, dynamic>> KeySelector { get; set; } = v => v;
        private bool Descending { get; set; } = false;
        private int PageNumber { get; }
        private int PageSize { get; }
        private QueryBuilder(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber <= 0 ? 1 : pageNumber;
            PageSize = pageSize <= 0 ? 10 : pageSize;
        }
        public static QueryBuilder<T> create(int pageNumber, int pageSize)
        {
            return new QueryBuilder<T>(pageNumber, pageSize);
        }
        public QueryBuilder<T> SetKeySelector(Expression<Func<T, dynamic>> keySelector)
        {
            KeySelector = keySelector;
            return this;
        }
        public QueryBuilder<T> SetDescending(bool descending)
        {
            Descending = descending;
            return this;
        }

        public Query<T> build()
        {
            return new Query<T>(KeySelector, Descending, PageNumber, PageSize);
        }
    }

}
