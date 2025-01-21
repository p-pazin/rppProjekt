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
        private readonly ReservationRepository _reservationRepository;
        private readonly InsuranceRepository _insuranceRepository;
        private readonly IMapper _mapper;

        public ContractServices(ContractRepository contractRepository, CompanyServices companyServices, UserRepository userRepository, 
            CompanyRepository companyRepository, ContactRepository contactRepository, VehicleRepository vehicleRepository, 
            OfferRepository offerRepository, IMapper mapper, ReservationRepository reservationRepository, InsuranceRepository insuranceRepository)
        {
            this._contractRepository = contractRepository;
            this._companyServices = companyServices;
            this._userRepository = userRepository;
            this._vehicleRepository = vehicleRepository;
            this._reservationRepository = reservationRepository;
            this._contactRepository = contactRepository;
            this._userRepository = userRepository;
            this._companyRepository = companyRepository;
            this._contactRepository = contactRepository;
            this._vehicleRepository = vehicleRepository;
            this._offerRepository = offerRepository;
            this._mapper = mapper;
            _insuranceRepository = insuranceRepository;
        }

        public ICollection<ContractDto> GetContracts(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contracts = _contractRepository.GetContracts(companyId);
            return _mapper.Map<List<ContractDto>>(contracts);
        }

        public ICollection<ContractDto> GetUnsignedContracts(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contracts = _contractRepository.GetUnsignedContracts(companyId);
            return _mapper.Map<List<ContractDto>>(contracts);
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
            var contract = _contractRepository.GetContract(contractId, companyId);
            return _mapper.Map<Contract>(contract);
        }

        public RentContractDto GetRentContract(int contractId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var company = _companyRepository.GetCompanyById(companyId);
            var contract = _contractRepository.GetContractsRent(contractId, companyId);
            var reservation = _reservationRepository.Get(contract.Reservation.Id, companyId);
            var vehicle = _vehicleRepository.GetOneVehicleById(reservation.Vehicle.Id, companyId);
            var contact = _contactRepository.GetContact(reservation.Contact.Id, companyId);
            var director = _userRepository.GetUserByAdminRoleAndCheckCompany(companyId);

            RentContractDto rentContract = new RentContractDto
            {
                Id = contractId,
                Title = contract.Title,
                Place = contract.Place,
                DateOfCreation = contract.DateOfCreation,
                Type = contract.Type,
                Content = contract.Content,
                Signed = contract.Signed,
                Name = company.Name,
                City = company.City,
                Address = company.Address,
                Pin = company.Pin,
                FirstNameDirector = director.FirstName,
                LastNameDirector = director.LastName,
                FirstNameContact = contact.FirstName,
                LastNameContact = contact.LastName,
                PinContact = contact.Pin,
                CountryContact = contact.Country,
                CityContact = contact.City,
                AddressContact = contact.Address,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Engine = vehicle.Engine,
                Registration = vehicle.Registration,
                Mileage = vehicle.Mileage,
                ReservationId = reservation.Id,
                Price = reservation.Price,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                MaxMileage = reservation.MaxMileage,
                NameInsurance = contract.Insurance.Name,
                CostInsurance = contract.Insurance.Cost
            };

            return rentContract;
        }

        public ContractDto MapContract(Contract contract)
        {
            return _mapper.Map<ContractDto>(contract);
        }
        public bool DeleteContract(Contract contractToDelete)
        {
            if (contractToDelete == null)
            {
                return false;
            }
            return _contractRepository.DeleteContract(contractToDelete);
        }

        public bool AddRentContract(ContractDto newContract, string? email, int reservationId, int insuranceId)
        {
            int companyId = _companyServices.GetCompanyId(email);
            Reservation reservation = _reservationRepository.Get(reservationId, companyId);
            User user = _userRepository.GetUserAndCompanyByEmail(email);
            Insurance insurance = _insuranceRepository.Get(insuranceId);
            Contact contact = _contactRepository.GetContact(reservation.Contact.Id, companyId);
            if (user == null || reservation == null || insurance == null)
            {
                return false;
            }
            Contract contract = new Contract
            {
                Title = newContract.Title,
                Place = newContract.Place,
                DateOfCreation = newContract.DateOfCreation,
                Type = 2,
                Content = newContract.Content,
                Signed = newContract.Signed,
                Company = user.Company,
                Contact = contact,
                Reservation = reservation,
                Insurance = insurance,
                User = user
            };
            return _contractRepository.AddContract(contract);
        }

        public bool UpdateRentContract(ContractDto newContract, string? email, int reservationId, int insuranceId)
        {
            int companyId = _companyServices.GetCompanyId(email);
            Reservation reservation = _reservationRepository.Get(reservationId, companyId);
            User user = _userRepository.GetUserAndCompanyByEmail(email);
            Insurance insurance = _insuranceRepository.Get(insuranceId);
            Contract contract = _contractRepository.GetContract(newContract.Id, companyId);
            Contact contact = _contactRepository.GetContact(reservation.Contact.Id, companyId);
            if(contract == null || user == null || reservation == null || insurance == null || contract.Signed == 1)
            {
                return false;
            }
            contract.Title = newContract.Title;
            contract.Place = newContract.Place;
            contract.Type = 2;
            contract.Content = newContract.Content;
            contract.Signed = newContract.Signed;
            contract.Company = user.Company;
            contract.Reservation = reservation;
            contract.Contact = contact;
            contract.Insurance = insurance;
            contract.User = user;
            return _contractRepository.UpdateContract(contract);
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
                Type = 1,
                Content = contractCreate.Content,
                Signed = contractCreate.Signed,
                User = user,
                Company = company,
            };

            if(offerId.HasValue)
            {
                Offer offer = _offerRepository.GetOfferById((int)offerId, companyId);
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
                if(contact == null || vehicle == null || vehicle.Usage == 2 || vehicle.State == 2)
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
                contract.Offer = _offerRepository.GetOfferById((int)offerId, companyId);
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
                if (contract.Contact == null || contract.Vehicle == null || contract.Vehicle.Usage == 2 || contract.Vehicle.State == 2)
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
