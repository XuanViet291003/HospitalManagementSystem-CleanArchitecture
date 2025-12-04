using AutoMapper;
using HospitalManagementSystem.Application.DTOs.Appointments;
using HospitalManagementSystem.Application.DTOs.Departments;
using HospitalManagementSystem.Application.DTOs.Doctors;
using HospitalManagementSystem.Application.DTOs.Invoices;
using HospitalManagementSystem.Application.DTOs.MedicalRecords;
using HospitalManagementSystem.Application.DTOs.Medicines;
using HospitalManagementSystem.Application.DTOs.Payments;
using HospitalManagementSystem.Application.DTOs.Prescriptions;
using HospitalManagementSystem.Application.DTOs.SystemConfigurations;
using HospitalManagementSystem.Core.Entities;

namespace HospitalManagementSystem.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentDto>();

            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.Email : "N/A")) // TODO: Map from UserProfile when available
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Name : "N/A"));

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.Doctor))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient));

            CreateMap<Doctor, DoctorInfoDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => 
                    src.User != null ? src.User.Email : "N/A")) // TODO: Map from UserProfile when available
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => 
                    src.Department != null ? src.Department.Name : "N/A"));

            CreateMap<Patient, PatientInfoDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null));

            CreateMap<MedicalRecord, MedicalRecordDto>()
                .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.Appointment != null ? src.Appointment.AppointmentTime : DateTime.MinValue))
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.Doctor))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient));

            CreateMap<Medicine, MedicineDto>();

            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(dest => dest.Items, opt => opt.Ignore()) // Will be mapped separately
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // Will be calculated

            CreateMap<PrescriptionItem, PrescriptionItemDto>()
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => 
                    src.Medicine != null ? src.Medicine.UnitPrice * src.Quantity : 0));

            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.Items, opt => opt.Ignore()) // Will be mapped separately
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient));

            CreateMap<InvoiceItem, InvoiceItemDto>();

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.InvoiceCode, opt => opt.MapFrom(src => src.Invoice != null ? src.Invoice.InvoiceCode : "N/A"));

            CreateMap<SystemConfiguration, SystemConfigurationDto>();
        }
    }
}
