namespace BudgetManagement.Models
{
    public class PaginationViewModel
    {
        public int Page { get; set; } = 1;
        private int recordsByPage = 10;
        private readonly int maxRecordsByPage = 50;

        public int RecordsByPage
        {
            get
            {
                return recordsByPage;
            }
            set
            {
                recordsByPage = (value > maxRecordsByPage) ? maxRecordsByPage : value;
            }
        }

        public int RecordsToSkip => recordsByPage * (Page - 1);
    }
}
