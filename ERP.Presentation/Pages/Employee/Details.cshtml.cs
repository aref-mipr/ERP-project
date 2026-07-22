using ERP.Application.Contract.EmployeeAgg;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static ERP.Domain.Entity.EmployeeModel;

namespace ERP.Presentation.Pages.Employee
{
    public class DetailsModel : PageModel
    {
        private readonly IApplicationEmployee _applicationEmployee;
        private readonly IResultMessage _resultMessage;
        private readonly IEnumExtension _enumExtension;
        public DetailsModel(IApplicationEmployee applicationEmployee, IResultMessage resultMessage, IEnumExtension enumExtension)
        {
            _applicationEmployee = applicationEmployee;
            _resultMessage = resultMessage;
            _enumExtension = enumExtension;
        }

        public EmployeeViewModel Employee { get; set; }
        public void OnGet(int id)
        {
            Employee = _applicationEmployee.GetBy(id);

            if (Employee.EmployeeStatus == _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.Active) ||
                Employee.EmployeeStatus == _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.ReEmployment))
                TempData["StatusStyle"] = "bg-success";

            else if(Employee.EmployeeStatus == _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.Resigned))
                TempData["StatusStyle"] = "bg-secondary";

            else if (Employee.EmployeeStatus == _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.Suspended))
                TempData["StatusStyle"] = "bg-warning text-dark";

            else 
                TempData["StatusStyle"] = "bg-danger";

        }
    }
}
