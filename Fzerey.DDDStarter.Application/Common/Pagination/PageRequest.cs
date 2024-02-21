using System.ComponentModel.DataAnnotations;

namespace Fzerey.DDDStarter.Application.Common.Pagination
{
    public class PageRequest
    {
        public const int MaxPageSize = 100;

        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        [Range(1, MaxPageSize)]
        public int PageSize { get; set; } = 10;

        public string? SearchQuery { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
}
