namespace Fzerey.DDDStarter.Application.Common.Pagination
{
    public class PageRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SearchQuery { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
}
