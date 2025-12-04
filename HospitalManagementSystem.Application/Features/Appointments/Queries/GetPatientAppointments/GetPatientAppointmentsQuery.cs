using HospitalManagementSystem.Application.DTOs.Appointments;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Appointments.Queries.GetPatientAppointments
{
    public class GetPatientAppointmentsQuery : IRequest<List<AppointmentDto>>
    {
        public required long PatientId { get; set; }
    }
}


