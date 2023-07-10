namespace BudgetManagement.Models
{
    public class DetailDealReport
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<DealByDate> GroupedDeals { get; set; }
        public int DepositBalance => GroupedDeals.Sum(x => x.DepositBalance);
        public int WithdrawBalance => GroupedDeals.Sum(x => x.WithdrawBalance);
        public int Total => DepositBalance - WithdrawBalance;

        public class DealByDate
        {
            public DateTime DealDate { get; set; }
            public IEnumerable<Deal> Deals { get; set; }
            public int DepositBalance => Deals.Where(x => x.OperationTypeId == (int)OperationTypeEnum.Ingreso).Sum(x => x.Price);
            public int WithdrawBalance => Deals.Where(x => x.OperationTypeId == (int)OperationTypeEnum.Gasto).Sum(x => x.Price);
        }
    }
}
