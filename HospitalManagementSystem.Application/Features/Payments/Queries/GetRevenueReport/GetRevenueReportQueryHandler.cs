using HospitalManagementSystem.Application.Features.Payments.Queries.GetRevenueReport;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Payments.Queries.GetRevenueReport
{
    public class GetRevenueReportQueryHandler : IRequestHandler<GetRevenueReportQuery, RevenueReportDto>
    {
        private readonly IPaymentRepository _paymentRepository;

        public GetRevenueReportQueryHandler(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<RevenueReportDto> Handle(GetRevenueReportQuery request, CancellationToken cancellationToken)
        {
            var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1);
            var endDate = request.EndDate ?? DateTime.UtcNow;

            var payments = await _paymentRepository.GetByDateRangeAsync(startDate, endDate);

            var report = new RevenueReportDto
            {
                TotalRevenue = payments.Sum(p => p.Amount),
                TotalPayments = payments.Count,
                CashRevenue = payments.Where(p => p.PaymentMethod == "Cash").Sum(p => p.Amount),
                CardRevenue = payments.Where(p => p.PaymentMethod == "Card").Sum(p => p.Amount),
                VNPayRevenue = payments.Where(p => p.PaymentMethod == "VNPay").Sum(p => p.Amount)
            };

            // Group by date for daily revenue
            var dailyGroups = payments
                .GroupBy(p => p.PaymentDate.Date)
                .Select(g => new DailyRevenueDto
                {
                    Date = g.Key,
                    Amount = g.Sum(p => p.Amount),
                    PaymentCount = g.Count()
                })
                .OrderBy(d => d.Date)
                .ToList();

            report.DailyRevenues = dailyGroups;

            return report;
        }
    }
}


