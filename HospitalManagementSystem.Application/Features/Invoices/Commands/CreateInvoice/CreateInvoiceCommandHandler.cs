using HospitalManagementSystem.Core.Entities;
using HospitalManagementSystem.Core.Interfaces.Repositories;
using MediatR;
using System.Threading;

namespace HospitalManagementSystem.Application.Features.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, long>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public CreateInvoiceCommandHandler(
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository)
        {
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<long> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            // Validate appointment exists
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
            {
                throw new InvalidOperationException($"Không tìm thấy lịch hẹn với Id = {request.AppointmentId}");
            }

            // Check if invoice already exists for this appointment
            var existingInvoice = await _invoiceRepository.GetByAppointmentIdAsync(request.AppointmentId);
            if (existingInvoice != null)
            {
                throw new InvalidOperationException($"Hóa đơn đã tồn tại cho lịch hẹn này.");
            }

            // Generate invoice code
            var invoiceCode = await _invoiceRepository.GenerateInvoiceCodeAsync();

            // Create invoice
            var invoice = new Invoice
            {
                AppointmentId = request.AppointmentId,
                PatientId = appointment.PatientId,
                InvoiceCode = invoiceCode,
                TotalAmount = 0, // Will be calculated after adding items
                Status = 0, // Unpaid
                IssuedDate = DateTime.UtcNow,
                DueDate = request.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            var createdInvoice = await _invoiceRepository.AddAsync(invoice);

            decimal totalAmount = 0;

            // Add consultation fee if appointment is completed
            if (appointment.Status == 1) // Completed
            {
                var doctor = await _doctorRepository.GetByIdAsync(appointment.DoctorId);
                if (doctor != null)
                {
                    var consultationItem = new InvoiceItem
                    {
                        InvoiceId = createdInvoice.Id,
                        ItemDescription = $"Phí khám - {doctor.Specialization}",
                        ItemType = "Consultation",
                        Quantity = 1,
                        UnitPrice = doctor.ConsultationFee,
                        Amount = doctor.ConsultationFee,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _invoiceItemRepository.AddAsync(consultationItem);
                    totalAmount += doctor.ConsultationFee;
                }
            }

            // Add custom items if provided
            if (request.Items != null && request.Items.Any())
            {
                foreach (var itemRequest in request.Items)
                {
                    var amount = itemRequest.UnitPrice * itemRequest.Quantity;
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceId = createdInvoice.Id,
                        ItemDescription = itemRequest.ItemDescription,
                        ItemType = itemRequest.ItemType,
                        Quantity = itemRequest.Quantity,
                        UnitPrice = itemRequest.UnitPrice,
                        Amount = amount,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _invoiceItemRepository.AddAsync(invoiceItem);
                    totalAmount += amount;
                }
            }

            // Update invoice total amount
            invoice.TotalAmount = totalAmount;
            await _invoiceRepository.UpdateAsync(invoice);

            return createdInvoice.Id;
        }
    }
}


