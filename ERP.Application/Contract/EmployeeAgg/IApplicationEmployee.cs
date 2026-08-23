using ERP.Application.Contract.FilterAgg;
using static ERP.Domain.Entity.EmployeeModel;

namespace ERP.Application.Contract.EmployeeAgg
{
    public interface IApplicationEmployee
    {
        void Create(CreateEmployeeDto command);
        void Edit(EditEmployeeDto command);
        EmployeeViewModel GetBy(int id);
        EditEmployeeDto GetForEdit(int id);
        List<EmployeeViewModel> GetAll(FilterParamsDto filterParams);
        List<EmployeeViewModel> GetAllActive();
        int GetCount(string? subject = null);
        void CheckSalaryStatus();
        void PaySalary(int id);
        List<EmployeeStatusViewModel> CreateStatuses();
        void ChangeStatus(int id, EmployeeStatuses status);
    }
}
