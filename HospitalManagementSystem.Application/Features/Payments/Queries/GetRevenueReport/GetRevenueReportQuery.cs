using MediatR;

namespace HospitalManagementSystem.Application.Features.Payments.Queries.GetRevenueReport
{
    public class GetRevenueReportQuery : IRequest<RevenueReportDto>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class RevenueReportDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalPayments { get; set; }
        public decimal CashRevenue { get; set; }
        public decimal CardRevenue { get; set; }
        public decimal VNPayRevenue { get; set; }
        public List<DailyRevenueDto> DailyRevenues { get; set; } = new();
    }

    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int PaymentCount { get; set; }
    }
}


