using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using AutoMapper;

namespace CarchiveAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() { 
            //CreateMap<Ad, AdDto>().ReverseMap();
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
        }
    }
}
