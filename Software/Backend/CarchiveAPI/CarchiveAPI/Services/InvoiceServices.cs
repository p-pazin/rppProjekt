using AutoMapper;
using CarchiveAPI.Data;
using CarchiveAPI.Dto;
using CarchiveAPI.Models;
using CarchiveAPI.Repositories;

namespace CarchiveAPI.Services
{
    public class InvoiceServices
    {
        private DataContext _context;
        private InvoiceRepository _invoiceRepository;
        private CompanyServices _companyServices;
        private UserRepository _userRepository;
        private ContractRepository _contractRepository;
        private PenaltyRepository _penaltyRepository;
        private readonly IMapper _mapper;

        public InvoiceServices(DataContext context, InvoiceRepository invoiceRepository, 
            CompanyServices companyServices, IMapper mapper, UserRepository userRepository, ContractRepository contractRepository, PenaltyRepository penaltyRepository)
        {
            this._context = context;
            this._invoiceRepository = invoiceRepository;
            this._companyServices = companyServices;
            this._mapper = mapper;
            _userRepository = userRepository;
            _contractRepository = contractRepository;
            _penaltyRepository = penaltyRepository;
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
            if (invoice == null)
            {
                return false;
            }
            return _invoiceRepository.DeleteInvoice(invoice);
        }

        public bool PostStartInvoiceRent(InvoiceDto invoiceCreate, int contractId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetContractsRent(contractId, companyId);
            if (contract == null || contract.Type == 1)
            {
                return false;
            }
            var numberOfDays = (contract.Reservation.EndDate.ToDateTime(TimeOnly.MinValue) -
                    contract.Reservation.StartDate.ToDateTime(TimeOnly.MinValue)).Days;
            var invoice = new Invoice
            {
                DateOfCreation = invoiceCreate.DateOfCreation,
                Vat = invoiceCreate.Vat,
                TotalCost = ((contract.Reservation.Price * numberOfDays) + contract.Insurance.Cost) * ((invoiceCreate.Vat + 100) / 100),
                Contract = contract,
                PaymentMethod = invoiceCreate.PaymentMethod
            };

            return _invoiceRepository.AddInvoice(invoice, companyId);
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

            return true;
        }
    }
}
