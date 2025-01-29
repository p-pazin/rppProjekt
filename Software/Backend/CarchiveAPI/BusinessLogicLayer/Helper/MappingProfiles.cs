using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using AutoMapper;

namespace CarchiveAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() { 
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Offer, OfferDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDto>().ReverseMap();
            CreateMap<VehiclePhoto, VehiclePhotoDto>().ReverseMap();
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<Insurance, InsuranceDto>().ReverseMap();
            CreateMap<Penalty, PenaltyDto>().ReverseMap();
            CreateMap<Contract, ContractDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Place, opt => opt.MapFrom(src => src.Place))
            .ForMember(dest => dest.DateOfCreation, opt => opt.MapFrom(src => src.DateOfCreation))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Signed, opt => opt.MapFrom(src => src.Signed))
            .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.Contact.FirstName + " " + src.Contact.LastName));

            CreateMap<Ad, AdDto>()
                .ForMember(dest => dest.Brand, opt =>
                    opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Brand : null))
                .ForMember(dest => dest.Model, opt =>
                    opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Model : null))
                .ForMember(dest => dest.Links, opt =>
                    opt.MapFrom(src => src.Vehicle != null && src.Vehicle.VehiclePhotos != null
                        ? src.Vehicle.VehiclePhotos.Select(vp => vp.Link).ToList() 
                        : new List<string>()))
                .ForMember(dest => dest.DateOfPublishment, opt => opt.MapFrom(src => src.DateOfPublishment))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ReverseMap();


            CreateMap<Ad, IndexAdDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.DateOfPublishment, opt => opt.MapFrom(src => src.DateOfPublishment))

                .ForMember(dest => dest.Registration, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Registration : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.State : 0))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Brand : null))
                .ForMember(dest => dest.Mileage, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Mileage : 0))
                .ForMember(dest => dest.ProductionYear, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.ProductionYear : 0))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Model : null))
                .ForMember(dest => dest.Engine, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Engine : null))
                .ForMember(dest => dest.CubicCapacity, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.CubicCapacity : 0.0))
                .ForMember(dest => dest.EnginePower, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.EnginePower : 0.0))
                .ForMember(dest => dest.RegisteredTo, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.RegisteredTo : default(DateOnly)))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Color : null))
                .ForMember(dest => dest.DriveType, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.DriveType : null))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Price : 0.0))
                .ForMember(dest => dest.TransmissionType, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.TransmissionType : null))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Type : null))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Condition : null))

                .ForMember(dest => dest.Link, opt => opt.MapFrom(src =>
                    src.Vehicle != null &&
                    src.Vehicle.VehiclePhotos != null &&
                    src.Vehicle.VehiclePhotos.Any()
                        ? src.Vehicle.VehiclePhotos.FirstOrDefault().Link
                        : null));

        }
    }
}
