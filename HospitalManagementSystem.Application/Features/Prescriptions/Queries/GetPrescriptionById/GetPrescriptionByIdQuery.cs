using HospitalManagementSystem.Application.DTOs.Prescriptions;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Prescriptions.Queries.GetPrescriptionById
{
    public class GetPrescriptionByIdQuery : IRequest<PrescriptionDto>
    {
        public required long Id { get; set; }
    }
}


