namespace ERP.Application.Contract.CustomerAgg
{
    public interface IApplicationCustomer
    {
        void Create(CreateCustomerDto command);
        void Edit(EditCustomerDto command);
        CustomerViewModel GetBy(int id);
        EditCustomerDto GetForEdit(int id);
        List<CustomerViewModel> GetAll();
    }
}
