using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.FilterAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;
using System.Globalization;

namespace ERP.Application.Service
{
    public class ApplicationCustomer : IApplicationCustomer
    {
        private readonly IRepositoryCustomer _repositoryCustomer;
        public ApplicationCustomer(IRepositoryCustomer repositoryCustomer)
        {
            _repositoryCustomer = repositoryCustomer;
        }

        public void Create(CreateCustomerDto command)
        {
            command.CustomersCriteria.SubscriptionCode = _repositoryCustomer.GetAll().Count() + 1;
            var customer = new CustomerModel(command.CustomersCriteria);
            _repositoryCustomer.Create(customer);
            _repositoryCustomer.SaveChange();
        }

        public void Edit(EditCustomerDto command)
        {
            var quary = _repositoryCustomer.GetBy(command.Id);
            if (quary == null)
                throw new NullReferenceException();

            quary.Edit(command.CustomersCriteria);
            _repositoryCustomer.SaveChange();
        }

        public CustomerViewModel GetBy(int id)
        {
            var customer = _repositoryCustomer.GetBy(id);
            if (customer == null)
                throw new NullReferenceException();

            var persianDate = new PersianCalendar();

            return new CustomerViewModel
            {
                Id = customer.Id,
                CreationTime =
                        $"{customer.CreationTime:HH:mm} , " +
                        $"{persianDate.GetYear(customer.CreationTime):0000}/" +
                        $"{persianDate.GetMonth(customer.CreationTime):00}/" +
                        $"{persianDate.GetDayOfMonth(customer.CreationTime):00}",
                FullName = $"{customer.FirstName} {customer.LastName}",
                Phone = customer.Phone,
                Email = customer.Email,
                SubscriptionCode = customer.SubscriptionCode,
            };
        }

        public EditCustomerDto GetForEdit(int id)
        {
            var customer = _repositoryCustomer.GetBy(id);
            if (customer == null)
                throw new NullReferenceException();

            return new EditCustomerDto
            {
                Id = customer.Id,
                CustomersCriteria = new CustomerCriteria
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    Email = customer.Email,
                }
            };
        }

        public List<CustomerViewModel> GetAll(FilterParamsDto filterParams)
        {
            var customers = _repositoryCustomer.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterParams.Subject))
                customers = customers
                    .Where(x => x.FirstName.Contains(filterParams.Subject) ||
                    x.LastName.Contains(filterParams.Subject) ||
                    x.SubscriptionCode.ToString().Contains(filterParams.Subject));

            var persianDate = new PersianCalendar();

            return customers.OrderByDescending(x => x.SubscriptionCode)
                .Skip(filterParams.Skip)
                .Take(filterParams.Take)
                .Select(x => new CustomerViewModel
            {
                Id = x.Id,
                    CreationTime =
                        $"{x.CreationTime:HH:mm} , " +
                        $"{persianDate.GetYear(x.CreationTime):0000}/" +
                        $"{persianDate.GetMonth(x.CreationTime):00}/" +
                        $"{persianDate.GetDayOfMonth(x.CreationTime):00}",
                    FullName = $"{x.FirstName} {x.LastName}",
                Phone = x.Phone,
                Email = x.Email,
                SubscriptionCode = x.SubscriptionCode,
            }).ToList();
        }

        public int GetCount(string? subject = null)
        {
            var customers = _repositoryCustomer.GetAll().AsQueryable();
            if(!string.IsNullOrWhiteSpace(subject))
                customers = customers
                    .Where(x => x.FirstName.Contains(subject) ||
                    x.LastName.Contains(subject) ||
                    x.SubscriptionCode.ToString().Contains(subject));

            return customers.Count();
        }
    }
}
