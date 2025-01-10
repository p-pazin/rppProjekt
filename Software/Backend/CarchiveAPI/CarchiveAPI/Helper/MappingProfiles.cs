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
            CreateMap<Contract, ContractDto>().ReverseMap();
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
            CreateMap<Contract, SaleContractDto>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.CompanyPin, opt => opt.MapFrom(src => src.Company.Pin))
                .ForMember(dest => dest.CompanyAddress, opt => opt.MapFrom(src => src.Company.Address))
                .ForMember(dest => dest.ContactPin, opt => opt.MapFrom(src => src.Contact != null ? src.Contact.Pin :
                (src.Offer != null && src.Offer.Contact != null ? src.Offer.Contact.Pin : "")))
                .ForMember(dest => dest.ContactAddress, opt => opt.MapFrom(src => src.Contact != null ? src.Contact.Address :
                (src.Offer != null && src.Offer.Contact != null ? src.Offer.Contact.Address : "")))
                .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.Contact != null ? src.Contact.FirstName + " " + src.Contact.LastName :
                (src.Offer != null && src.Offer.Contact != null ? src.Offer.Contact.FirstName + " " + src.Offer.Contact.LastName : "")))
                .ForMember(dest => dest.VehicleBrand, opt => opt.MapFrom(src => src.Vehicle.Brand))
                .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => src.Vehicle.Model))
                .ForMember(dest => dest.VehicleRegistration, opt => opt.MapFrom(src => src.Vehicle.Registration))
                .ForMember(dest => dest.VehicleCubicCapacity, opt => opt.MapFrom(src => src.Vehicle.CubicCapacity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Price : 
                (src.Offer != null ? src.Offer.Price : 0)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + src.User.LastName))
                .ForMember(dest => dest.Vehicles, opt => opt.MapFrom((src, dest) => src.Offer != null ?
                src.Offer.OfferVehicles.Select(ov => new VehicleDto { Brand = ov.Vehicle.Brand })
                .ToList(): new List<VehicleDto>()));
            CreateMap<Ad, AdDto>()
                .ForMember(dest => dest.Brand, opt =>
                    opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Brand : null))
                .ForMember(dest => dest.Model, opt =>
                    opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.Model : null))
                .ForMember(dest => dest.Link, opt =>
                    opt.MapFrom(src => src.Vehicle != null &&
                                      src.Vehicle.VehiclePhotos != null &&
                                      src.Vehicle.VehiclePhotos.Any()
                        ? src.Vehicle.VehiclePhotos.FirstOrDefault().Link
                        : null))
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
