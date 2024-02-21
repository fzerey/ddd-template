using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Fzerey.DDDStarter.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public List<T>? Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public async Task ToPagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            Items = items;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
        public async Task ToPagedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            Items = items;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
