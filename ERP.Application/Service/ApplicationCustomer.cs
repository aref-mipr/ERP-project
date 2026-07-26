using ERP.Application.Contract.CustomerAgg;
using ERP.Domain.Criteria;
using ERP.Domain.Entity;
using ERP.Domain.Interface.Repository;

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

            return new CustomerViewModel
            {
                Id = customer.Id,
                CreationTime = customer.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
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
        public List<CustomerViewModel> GetAll()
        {
            return _repositoryCustomer.GetAll().Select(x => new CustomerViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime.ToString("mm : HH , yyyy/MM/dd"),
                FullName = $"{x.FirstName} {x.LastName}",
                Phone = x.Phone,
                Email = x.Email,
                SubscriptionCode = x.SubscriptionCode,
            }).OrderBy(x => x.SubscriptionCode).ToList();
        }
    }
}
