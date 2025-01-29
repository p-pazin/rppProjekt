using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class PenaltyServices
    {
        private DataContext _context;
        private PenaltyRepository _penaltyServices;
        private readonly IMapper _mapper;
        public PenaltyServices(DataContext context, IMapper mapper, PenaltyRepository penaltyServices)
        {
            this._context = context;
            this._penaltyServices = penaltyServices;
            this._mapper = mapper;
        }

        public ICollection<PenaltyDto> GetPenalties()
        {
            var penalties = _penaltyServices.GetAll();
            return _mapper.Map<List<PenaltyDto>>(penalties);
        }
    }
}
