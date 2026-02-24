namespace ForestSchedule.Application.DTOs.LessonDtos
{
    public class LessonQueryParameters
    {
        // Searching
        public string? SearchTerm { get; set; }

        // Filtration
        public int? GroupId { get; set; }
        public int? TeacherId { get; set; }

        // Sorting
        public string? SortBy { get; set; } // "subject", "day" ...
        public bool SortDescending { get; set; } = false;

        // Pagination
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        const int maxPageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
