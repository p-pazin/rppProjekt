using AutoMapper;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class ContractServices
    {
        private readonly ContractRepository _contractRepository;
        private readonly CompanyServices _companyServices;
        private readonly UserRepository _userRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly ReservationRepository _reservationRepository;
        private readonly ContactRepository _contactRepository;
        private readonly InsuranceRepository _insuranceRepository;
        private readonly IMapper _mapper;

        public ContractServices(ContractRepository contractRepository, CompanyServices companyServices, UserRepository userRepository, VehicleRepository vehicleRepository
            ,ReservationRepository reservationRepository, ContactRepository contactRepository, IMapper mapper, InsuranceRepository insuranceRepository)
        {
            this._contractRepository = contractRepository;
            this._companyServices = companyServices;
            this._userRepository = userRepository;
            this._vehicleRepository = vehicleRepository;
            this._reservationRepository = reservationRepository;
            this._contactRepository = contactRepository;
            this._mapper = mapper;
            _insuranceRepository = insuranceRepository;
        }

        public ICollection<ContractDto> GetContracts(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contracts = _contractRepository.GetContracts(companyId);
            return _mapper.Map<List<ContractDto>>(contracts);
        }

        public bool CheckIfContractExists(int contractId)
        {
            return _contractRepository.ContractExists(contractId);
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
            var contract = _contractRepository.GetContractsRent(contractId, companyId);
            return _mapper.Map<RentContractDto>(contract);
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

        public bool AddRentContract(ContractDto newContract, string? email, int contactId, int vehicleId, int reservationId, int insuranceId)
        {
            int companyId = _companyServices.GetCompanyId(email);
            Contact contact = _contactRepository.GetContact(contactId, companyId);
            Vehicle vehicle = _vehicleRepository.GetOneVehicleById(vehicleId, companyId);
            Reservation reservation = _reservationRepository.Get(reservationId, companyId);
            User user = _userRepository.GetUserAndCompanyByEmail(email);
            Insurance insurance = _insuranceRepository.Get(insuranceId);
            if (contact == null || vehicle == null || user == null || reservation == null || insurance == null)
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
                Vehicle = vehicle,
                Reservation = reservation,
                Insurance = insurance,
                User = user
            };
            return _contractRepository.AddContract(contract);
        }

        public bool UpdateRentContract(ContractDto newContract, string? email, int contactId, int vehicleId, int reservationId, int insuranceId)
        {
            int companyId = _companyServices.GetCompanyId(email);
            Contact contact = _contactRepository.GetContact(contactId, companyId);
            Vehicle vehicle = _vehicleRepository.GetOneVehicleById(vehicleId, companyId);
            Reservation reservation = _reservationRepository.Get(reservationId, companyId);
            User user = _userRepository.GetUserAndCompanyByEmail(email);
            Insurance insurance = _insuranceRepository.Get(insuranceId);
            Contract contract = _contractRepository.GetContract(newContract.Id, companyId);
            if(contract == null || contact == null || vehicle == null || user == null || reservation == null || insurance == null || contract.Signed == 1)
            {
                return false;
            }
            contract.Title = newContract.Title;
            contract.Place = newContract.Place;
            contract.Type = 2;
            contract.Content = newContract.Content;
            contract.Signed = newContract.Signed;
            contract.Company = user.Company;
            contract.Contact = contact;
            contract.Vehicle = vehicle;
            contract.Reservation = reservation;
            contract.Insurance = insurance;
            contract.User = user;
            return _contractRepository.UpdateContract(contract);
        }
        
    }
}
