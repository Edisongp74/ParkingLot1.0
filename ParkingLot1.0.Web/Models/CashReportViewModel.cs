namespace ParkingLot1._0.Web.Models
{
    public class CashReportViewModel
    {
        public decimal TotalIncome { get; set; }
        public int TotalPayments { get; set; }
        public int TotalMonthlyPayments { get; set; }
        public List<PaymentHistoryViewModel> Payments { get; set; } = new();
    }
}