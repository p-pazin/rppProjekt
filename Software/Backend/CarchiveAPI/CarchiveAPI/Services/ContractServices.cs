using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace CarchiveAPI.Services
{
    public class ContractServices
    {
        private readonly ContractRepository _contractRepository;
        private readonly CompanyServices _companyServices;
        private readonly UserRepository _userRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly ContactRepository _contactRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly OfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public ContractServices(ContractRepository contractRepository, CompanyServices companyServices, UserRepository userRepository, 
            CompanyRepository companyRepository, ContactRepository contactRepository, VehicleRepository vehicleRepository, 
            OfferRepository offerRepository, IMapper mapper)
        {
            this._contractRepository = contractRepository;
            this._companyServices = companyServices;
            this._userRepository = userRepository;
            this._companyRepository = companyRepository;
            this._contactRepository = contactRepository;
            this._vehicleRepository = vehicleRepository;
            this._offerRepository = offerRepository;
            this._mapper = mapper;
        }

        public ICollection<SaleContractDto> GetContracts(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contracts = _contractRepository.GetContracts(companyId);
            return _mapper.Map<List<SaleContractDto>>(contracts);
        }

        public bool CheckIfContractExists(int contractId)
        {
            return _contractRepository.ContractExists(contractId);
        }

        public SaleContractDto GetSaleContract(int contractId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetSaleContractDto(contractId, companyId);

            return contract;
        }

        public Contract GetContract(int contractId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetSaleContract(contractId, companyId);
            return contract;
        }
        public SaleContractDto MapContract(Contract contract)
        {
            return _mapper.Map<SaleContractDto>(contract);
        }
        public bool DeleteContract(Contract contractToDelete)
        {
            if (contractToDelete == null)
            {
                return false;
            }
            return _contractRepository.DeleteContract(contractToDelete);
        }

        public bool CreateContractForSale(ContractDto contractCreate, int? contactId, int? vehicleId, int? offerId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            User user = _userRepository.GetUserAndCompanyByEmail(email);
            Company company = user.Company;
            var contract = new Contract
            {
                Title = contractCreate.Title,
                Place = contractCreate.Place,
                DateOfCreation = contractCreate.DateOfCreation,
                Type = contractCreate.Type,
                Content = contractCreate.Content,
                Signed = contractCreate.Signed,
                User = user,
                Company = company,
            };

            if(offerId.HasValue)
            {
                Offer offer = _offerRepository.GetOfferById((int)offerId);
                if (offer == null)
                {
                    return false;
                }
                contract.Offer = offer;
            }
            else
            {
                Contact contact = _contactRepository.GetContact((int)contactId, companyId);
                Vehicle vehicle = _vehicleRepository.GetOneVehicleById((int)vehicleId, companyId);
                if(contact == null || vehicle == null)
                {
                    return false;
                }
                contract.Contact = contact;
                contract.Vehicle = vehicle;
            }

            return _contractRepository.CreateContractForSale(contract);
        }

        public bool UpdateContractForSale(ContractDto contractDto, int? contactId, int? vehicleId, int? offerId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetSaleContract(contractDto.Id, companyId);

            if(contract.Type != 1)
            {
                return false;
            }
            if(offerId.HasValue)
            {
                contract.Offer = _offerRepository.GetOfferById((int)offerId);
                if(contract.Offer == null)
                {
                    return false;
                }
                contract.Contact = null;
                contract.ContactId = null;
                contract.Vehicle = null;
                contract.VehicleId = null;
            }
            else
            {
                contract.Contact = _contactRepository.GetContact((int)contactId, companyId);
                contract.Vehicle = _vehicleRepository.GetOneVehicleById((int)vehicleId, companyId);
                if (contract.Contact == null || contract.Vehicle == null)
                {
                    return false;
                }
                contract.Offer = null;
                contract.OfferId = null;
            }

            return _contractRepository.UpdateContract(contract);
        }
    }
}
