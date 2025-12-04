using HospitalManagementSystem.Application.DTOs.Medicines;
using MediatR;

namespace HospitalManagementSystem.Application.Features.Medicines.Queries.GetMedicines
{
    public class GetMedicinesQuery : IRequest<List<MedicineDto>>
    {
        public string? SearchTerm { get; set; }
    }
}


