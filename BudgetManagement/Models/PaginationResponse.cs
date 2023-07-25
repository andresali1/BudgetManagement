namespace BudgetManagement.Models
{
    public class PaginationResponse
    {
        public int Page { get; set; } = 1;
        public int RecordsByPage { get; set; } = 10;
        public int AmountOfRecords { get; set; }
        public int AmountOfPages => (int)Math.Ceiling((double)AmountOfRecords / RecordsByPage);
        public string BaseUrl { get; set; }
    }

    public class PaginationResponse<T> : PaginationResponse
    {
        public IEnumerable<T> Elements { get; set; }
    }
}
