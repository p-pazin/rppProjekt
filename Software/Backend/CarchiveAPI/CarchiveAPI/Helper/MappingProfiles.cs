using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using AutoMapper;

namespace CarchiveAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() { 
            CreateMap<Ad, AdDto>().ReverseMap();
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
        }
    }
}
