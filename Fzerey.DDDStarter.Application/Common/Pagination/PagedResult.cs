namespace Fzerey.DDDStarter.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public List<T> Items { get; init; } = [];
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public int PageIndex { get; init; }
        public int PageSize { get; init; }

        public static PagedResult<T> Create(List<T> items, int totalCount, int pageIndex, int pageSize)
        {
            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }
    }
}
