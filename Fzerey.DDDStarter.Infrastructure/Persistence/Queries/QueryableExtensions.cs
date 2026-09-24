using Fzerey.DDDStarter.Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Fzerey.DDDStarter.Infrastructure.Persistence.Queries
{
    internal static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> source,
            PageRequest request,
            CancellationToken cancellationToken
        )
        {
            var pageIndex = Math.Max(request.PageIndex, 1);
            var pageSize = Math.Clamp(request.PageSize, 1, PageRequest.MaxPageSize);
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return PagedResult<T>.Create(items, count, pageIndex, pageSize);
        }
    }
}
