using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class InvoiceServices
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly CompanyServices _companyServices;
        private readonly UserRepository _userRepository;
        private readonly ContractRepository _contractRepository;
        private readonly VehicleRepository _vehicleRepository;
        private readonly OfferVehicleRepository _offerVehicleRepository;
        private readonly ReservationRepository _reservationRepository;
        private DataContext _context;
        private PenaltyRepository _penaltyRepository;
        private readonly IMapper _mapper;

        public InvoiceServices(DataContext context,InvoiceRepository invoiceRepository, 
            CompanyServices companyServices, IMapper mapper, UserRepository userRepository, 
            ContractRepository contractRepository, PenaltyRepository penaltyRepository, VehicleRepository vehicleRepository, 
            OfferVehicleRepository offerVehicleRepository, ReservationRepository reservationRepository)
        {
            this._invoiceRepository = invoiceRepository;
            this._companyServices = companyServices;
            this._context = context;
            this._mapper = mapper;
            this._userRepository = userRepository;
            this._contractRepository = contractRepository;
            this._vehicleRepository = vehicleRepository;
            this._offerVehicleRepository = offerVehicleRepository;
            this._userRepository = userRepository;
            this._contractRepository = contractRepository;
            this._penaltyRepository = penaltyRepository;
            this._reservationRepository = reservationRepository;
        }

        public ICollection<InvoiceDto> GetInvoices(string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var invoices = _invoiceRepository.GetAll(companyId);
            return _mapper.Map<List<InvoiceDto>>(invoices);
        }

        public InvoiceDto GetOneInvoice(string email, int id)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var invoice = _invoiceRepository.Get(id, companyId);
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public bool DeleteInvoice(string email, int id)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var invoice = _invoiceRepository.Get(id, companyId);
            var penalties = _penaltyRepository.GetList(invoice.Id);
            foreach (var penalty in penalties)
            {
                _penaltyRepository.Delete(penalty);
            }
            if (invoice == null)
            {
                return false;
            }
            return _invoiceRepository.DeleteInvoice(invoice);
        }
        public bool CreateInvoiceForSale(InvoiceDto invoiceCreate, int contractId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetSaleContract(contractId, companyId);

            if (contract == null || contract.Type == 2)
            {
                return false;
            }

            if(contract.OfferId.HasValue)
            {
                var offer = contract.Offer;

                var offerVehicles = _offerVehicleRepository.GetAllByOfferId(offer.Id);

                foreach (var offerVehicle in offerVehicles)
                {
                    var vehicle = _vehicleRepository.GetOneVehicleById(offerVehicle.VehicleId, companyId);

                    if (vehicle != null)
                    {
                        vehicle.State = 2;
                        _vehicleRepository.UpdateVehicle(vehicle);
                    }
                }
            }
            else
            {
                var vehicle = contract.Vehicle;
                vehicle.State = 2;

                _vehicleRepository.UpdateVehicle(vehicle);
            }

            var invoice = new Invoice
            {
                DateOfCreation = invoiceCreate.DateOfCreation,
                Vat = invoiceCreate.Vat,
                PaymentMethod = invoiceCreate.PaymentMethod,
                TotalCost = invoiceCreate.TotalCost,
                Contract = contract
            };

            return _invoiceRepository.AddInvoice(invoice, companyId);
        }

        public bool PostStartInvoiceRent(InvoiceDto invoiceCreate, int contractId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetContractsRent(contractId, companyId);

            if (contract == null || contract.Type == 1 || contract.Signed == 1)
            {
                return false;
            }

            var reservation = _reservationRepository.Get((int)contract.ReservationId, companyId);

            var numberOfDays = (contract.Reservation.EndDate.ToDateTime(TimeOnly.MinValue) -
                    contract.Reservation.StartDate.ToDateTime(TimeOnly.MinValue)).Days;
            var invoice = new Invoice
            {
                DateOfCreation = invoiceCreate.DateOfCreation,
                Vat = invoiceCreate.Vat,
                TotalCost = Math.Round(((contract.Reservation.Price * numberOfDays) + contract.Insurance.Cost) * ((invoiceCreate.Vat + 100) / 100), 2),
                Contract = contract,
                PaymentMethod = invoiceCreate.PaymentMethod
            };

            var invoiceResult = _invoiceRepository.AddInvoice(invoice, companyId);

            var vehicle = _context.Vehicles.Find(contract.VehicleId);
            if (vehicle != null)
            {
                vehicle.State = 3;
                _vehicleRepository.UpdateVehicle(vehicle);
            }
            contract.Signed = 1;
            _contractRepository.UpdateContract(contract);
            _context.SaveChanges();

            return invoiceResult;
        }

        public bool PostFinalInvoiceRent(InvoiceDto invoiceCreate, int contractId, string email, List<int> penaltyId)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetContractsRent(contractId, companyId);
            List<Penalty> penalties = new List<Penalty>();
            foreach (var p in penaltyId)
            {
                penalties.Add(_penaltyRepository.Get(p));
            }

            if (contract == null || contract.Type == 1)
            {
                return false;
            }
            double cost = 0;
            foreach (var penalty in penalties)
            {
                cost += penalty.Cost;
            }
            var invoice = new Invoice
            {
                DateOfCreation = invoiceCreate.DateOfCreation,
                Vat = invoiceCreate.Vat,
                TotalCost = ((cost + invoiceCreate.Mileage) * (invoiceCreate.Vat + 100)/100),
                Contract = contract,
                PaymentMethod = invoiceCreate.PaymentMethod,
                Mileage = invoiceCreate.Mileage
            };
            _invoiceRepository.AddInvoice(invoice, companyId);

            foreach (var penalty in penalties)
            {
                var invoicePenalty = new InvoicePenalty
                {
                    InvoiceId = invoice.Id,
                    PenaltyId = penalty.Id
                };
                _penaltyRepository.AddPenltyInvoice(invoicePenalty);
                _context.SaveChanges();
            }

            var vehicle = _context.Vehicles.Find(contract.VehicleId);
            if (vehicle != null)
            {
                vehicle.State = 1;
                _vehicleRepository.UpdateVehicle(vehicle);
            }
            

            return true;
        }
    }
}
