using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class ReservationServices
    {
        private DataContext _context;
        private CompanyServices _companyServices;
        private ReservationRepository _reservationRepository;
        private UserRepository _userRepository;
        private VehicleRepository _vehicleRepository;
        private ContactRepository _contactRepository;
        private readonly IMapper _mapper;
        public ReservationServices(DataContext context, ReservationRepository reservationRepository, CompanyServices companyServices,
            UserRepository userRepository, VehicleRepository vehicleRepository, ContactRepository contactRepository, IMapper mapper)
        {
            this._context = context;
            this._reservationRepository = reservationRepository;
            this._companyServices = companyServices;
            this._userRepository = userRepository;
            this._vehicleRepository = vehicleRepository;
            this._contactRepository = contactRepository;
            this._mapper = mapper;
        }
        public ICollection<ReservationDto> GetReservations(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var reservations = _reservationRepository.GetAll(companyId);
            return _mapper.Map<List<ReservationDto>>(reservations);
        }
        public ReservationDto GetOneReservation(string email, int id)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var reservation = _reservationRepository.Get(id, companyId);
            return _mapper.Map<ReservationDto>(reservation);
        }
        public bool AddReservation(string email, ReservationDto reservationDto, int vehicleId, int contactId)
        {
            int companyId = _companyServices.GetCompanyId(email);
            User user = _userRepository.GetUserAndCompanyByEmail(email);
            Vehicle vehicle = _vehicleRepository.GetOneVehicleById(vehicleId, companyId);
            Contact contact = _contactRepository.GetContact(contactId, companyId);
            if (vehicle == null || contact == null || user == null || vehicle.Usage == 1 || vehicle.State != 1)
            {
                return false;
            }
            Reservation reservation = new Reservation
            {
                DateOfCreation = reservationDto.DateOfCreation,
                StartDate = reservationDto.StartDate,
                EndDate = reservationDto.EndDate,
                Price = reservationDto.Price,
                State = reservationDto.State,
                MaxMileage = reservationDto.MaxMileage,
                Contact = contact,
                Vehicle = vehicle,
                User = user
            };
            return _reservationRepository.Add(reservation);
        }

        public bool UpdateReservation(string email, ReservationDto reservationDto, int id, int vehicleId)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var reservation = _reservationRepository.Get(id, companyId);
            var vehicle = _vehicleRepository.GetOneVehicleById(vehicleId, companyId);
            if (reservation == null)
            {
                return false;
            }
            reservation.State = reservationDto.State;
            reservation.Price = reservationDto.Price;
            reservation.StartDate = reservationDto.StartDate;
            reservation.EndDate = reservationDto.EndDate;
            reservation.MaxMileage = reservationDto.MaxMileage;
            reservation.Vehicle = vehicle;
            return _reservationRepository.Update(reservation);
        }

        public bool DeleteReservation(string email, int id) {
            int companyId = _companyServices.GetCompanyId(email);
            var reservation = _reservationRepository.Get(id, companyId);
            if (reservation == null)
            {
                return false;
            }
            return _reservationRepository.Delete(id, companyId);
        }
    }
}