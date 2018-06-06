using Microsoft.EntityFrameworkCore;
using QueryableRequests.Criteria;
using System.Collections.Generic;
using System.Linq;

namespace QueryableRequests
{
    public class QueryBuilder<T> : IIncrementalQueryBuilder<T>, ICompleteQueryBuilder<T>
        where T : class
    {
        IQueryable<T> _query;

        public QueryBuilder(DbSet<T> query)
        {
            _query = query;
        }

        public IIncrementalQueryBuilder<T> Filter(IEnumerable<FilterCirteria<T>> filters)
        {
            foreach ( var filter in filters )
                _query = _query
                    .Where(filter.Predicate)
                    .AsQueryable();

            return this;
        }

        public IIncrementalQueryBuilder<T> Sort(IEnumerable<SortCriteria<T>> sorts)
        {
            if ( !sorts.Any() )
                return this;

            var orderedSorts = sorts.OrderBy(s => s.SortPosition);
            var firstSort = orderedSorts.First();
            var remainingSorts = orderedSorts.Skip(1);

            var orderedQueryable = ApplyOrderBy(firstSort);

            foreach ( var s in remainingSorts )
                orderedQueryable = ApplyThenBy(s);

            _query = orderedQueryable.AsQueryable();

            return this;

            IOrderedQueryable<T> ApplyOrderBy(SortCriteria<T> sort) =>
                sort.SortDirection == SortDirection.Ascending
                    ? _query.OrderBy(sort.KeySelector)
                    : _query.OrderByDescending(sort.KeySelector);

            IOrderedQueryable<T> ApplyThenBy(SortCriteria<T> sort) =>
                sort.SortDirection == SortDirection.Ascending
                    ? orderedQueryable.ThenBy(sort.KeySelector)
                    : orderedQueryable.ThenByDescending(sort.KeySelector);
        }

        public IIncrementalQueryBuilder<T> Include(IEnumerable<IncludeCriteria<T>> includes)
        {
            if ( !includes.Any() )
                return this;

            var dbSet = _query as DbSet<T>;

            var firstInclude = includes.First();
            var remainingInclujdes = includes.Skip(1);

            var includableQueryable = dbSet.Include(firstInclude.KeySelector);

            foreach ( var i in remainingInclujdes )
                includableQueryable = includableQueryable.Include(i.KeySelector);

            _query = includableQueryable.AsQueryable();

            return this;
        }

        public IQueryable<T> Build() => _query;

        public IQueryable<T> Build<TRequest>(TRequest req)
            where TRequest : QueryableRequest<T>
        {
            return new QueryBuilder<T>(_query as DbSet<T>)
                .Include(req.Includes)
                .Filter(req.Filters)
                .Sort(req.SortCriteria)
                .Build();
        }
    }

    public interface IIncrementalQueryBuilder<T>
        where T : class
    {
        IIncrementalQueryBuilder<T> Include(IEnumerable<IncludeCriteria<T>> includes);
        IIncrementalQueryBuilder<T> Sort(IEnumerable<SortCriteria<T>> sorts);
        IIncrementalQueryBuilder<T> Filter(IEnumerable<FilterCirteria<T>> filters);
        IQueryable<T> Build();
    }

    public interface ICompleteQueryBuilder<T>
    {
        IQueryable<T> Build<TRequest>(TRequest req)
            where TRequest : QueryableRequest<T>;
    }
}
