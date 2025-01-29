using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class InsuranceServices
    {
        //private readonly ContractRepository _contractRepository;
        private readonly InsuranceRepository _insuranceRepository;
        private readonly IMapper _mapper;
        public InsuranceServices(InsuranceRepository insuranceRepository, IMapper mapper)
        {
            this._insuranceRepository = insuranceRepository;
            this._mapper = mapper;
        }

        public ICollection<InsuranceDto> GetInsurances()
        {
            var insurances = _insuranceRepository.GetAll();
            return _mapper.Map<List<InsuranceDto>>(insurances);
        }

        public InsuranceDto GetInsurance(int id)
        {
            var insurance = _insuranceRepository.Get(id);
            return _mapper.Map<InsuranceDto>(insurance);
        }
    }
}
