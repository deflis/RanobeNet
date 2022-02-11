
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Data
{
    public class PagedList<T>
    {
        [Required]
        public int CurrentPage { get; private set; }
        [Required]
        public int TotalPages { get; private set; }
        [Required]
        public int PageSize { get; private set; }
        [Required]
        public int TotalCount { get; private set; }
        [Required]
        public bool HasPrevious => CurrentPage > 1;
        [Required]
        public bool HasNext => CurrentPage < TotalPages;
        [Required]
        public List<T> Items { get; private set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }
    }
    public static class PagedListExtension
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, Query<T> query)
        {
            var count = source.Count();
            var items = source.OrderByAscOrDesc(query.KeySelector, query.Descending).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();
            return new PagedList<T>(items, count, query.PageNumber, query.PageSize);
        }

        public static PagedList<TResult> ToPagedList<TSource, TResult>(this IQueryable<TSource> source, Query<TSource> query, Func<TSource, TResult> converter)
        {
            var count = source.Count();
            var items = source.OrderByAscOrDesc(query.KeySelector, query.Descending).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).Select(converter).ToList();
            return new PagedList<TResult>(items, count, query.PageNumber, query.PageSize);
        }

        async public static Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, Query<T> query)
        {
            var count = source.Count();
            var items = await source.OrderByAscOrDesc(query.KeySelector, query.Descending).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
            return new PagedList<T>(items, count, query.PageNumber, query.PageSize);
        }
        async public static Task<PagedList<TResult>> ToPagedListAsync<TSource, TResult>(this IQueryable<TSource> source, Query<TSource> query, Expression<Func<TSource, TResult>> converter)
        {
            var count = source.Count();
            var items = await source.OrderByAscOrDesc(query.KeySelector, query.Descending).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).Select(converter).ToListAsync();
            return new PagedList<TResult>(items, count, query.PageNumber, query.PageSize);
        }

        async public static Task<PagedList<TResult>> ToPagedListAsync<TSource, TResult>(this IQueryable<TSource> source, Query<TSource> query, IMapper mapper, object? parameters = null, params Expression<Func<TResult, object>>[] membersToExpand)
        {
            var count = source.Count();
            var items = await mapper.ProjectTo<TResult>(source.OrderByAscOrDesc(query.KeySelector, query.Descending).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize), parameters, membersToExpand).ToListAsync();
            return new PagedList<TResult>(items, count, query.PageNumber, query.PageSize);
        }

        private static IOrderedQueryable<TSource> OrderByAscOrDesc<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool desc)
        {
            if (!desc)
            {
                return source.OrderBy(keySelector);
            }
            else
            {
                return source.OrderByDescending(keySelector);
            }
        }
    }
}
