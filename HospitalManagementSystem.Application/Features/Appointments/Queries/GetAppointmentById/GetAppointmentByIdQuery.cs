using HospitalManagementSystem.Application.DTOs.Appointments;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Appointments.Queries.GetAppointmentById
{
    public class GetAppointmentByIdQuery : IRequest<AppointmentDto>
    {
        public long Id { get; set; }
    }
}

