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
        private readonly IMapper _mapper;

        public InvoiceServices(InvoiceRepository invoiceRepository, 
            CompanyServices companyServices, IMapper mapper, UserRepository userRepository, 
            ContractRepository contractRepository)
        {
            this._invoiceRepository = invoiceRepository;
            this._companyServices = companyServices;
            this._mapper = mapper;
            this._userRepository = userRepository;
            this._contractRepository = contractRepository;
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
        public bool CreateInvoiceForSale(InvoiceDto invoiceCreate, int contractId, string email)
        {
            int companyId = _companyServices.GetCompanyId(email);
            var contract = _contractRepository.GetSaleContract(contractId, companyId);

            if (contract == null || contract.Type == 2)
            {
                return false;
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
    }
}
