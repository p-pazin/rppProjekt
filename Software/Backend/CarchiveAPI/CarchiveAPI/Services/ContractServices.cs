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
        private readonly IMapper _mapper;

        public ContractServices(ContractRepository contractRepository, CompanyServices companyServices, IMapper mapper)
        {
            this._contractRepository = contractRepository;
            this._companyServices = companyServices;
            this._mapper = mapper;
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
    }
}
