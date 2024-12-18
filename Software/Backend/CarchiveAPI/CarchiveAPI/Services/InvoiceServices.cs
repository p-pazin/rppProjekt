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
        private readonly IMapper _mapper;

        public InvoiceServices(DataContext context, InvoiceRepository invoiceRepository, 
            CompanyServices companyServices, IMapper mapper, UserRepository userRepository)
        {
            this._context = context;
            this._invoiceRepository = invoiceRepository;
            this._companyServices = companyServices;
            this._mapper = mapper;
            _userRepository = userRepository;
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
    }
}
