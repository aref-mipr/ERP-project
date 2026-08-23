using ERP.Application.Contract.EmployeeAgg;
using ERP.Application.Contract.FilterAgg;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ERP.Domain.Entity.EmployeeModel;

namespace ERP.Presentation.Pages.Employee
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApplicationEmployee _applicationEmployee;
        private readonly IRepositoryEmployee _repositoryEmployee;
        private readonly IResultMessage _resultMessage;
        private readonly IEnumExtension _enumExtension;
        private readonly IRepositoryBudget _repositoryBudget;
        public IndexModel(IApplicationEmployee applicationEmployee, IRepositoryEmployee repositoryEmployee,
            IResultMessage resultMessage, IEnumExtension enumExtension, IRepositoryBudget repositoryBudget)
        {
            _applicationEmployee = applicationEmployee;
            _repositoryEmployee = repositoryEmployee;
            _resultMessage = resultMessage;
            _enumExtension = enumExtension;
            _repositoryBudget = repositoryBudget;
        }

        public List<EmployeeViewModel> Employees { get; set; }

        [BindProperty]
        public EmployeeStatuses Status { get; set; }
        public SelectList StatusesList { get; set; }
        public SelectList ReEmploymentStatus { get; set; }
        public FilterParamsDto FilterParams { get; set; }
        public SearchViewModel Search { get; set; }


        public void OnGet(int pageId = 1, string? search = "")
        {
            ViewData["PageTitle"] = "مدیریت کارمندان";
            ViewData["EmployeeActive"] = "active";
            _applicationEmployee.CheckSalaryStatus();

            var allsStatuses = _applicationEmployee.CreateStatuses()
                .Where(x => x.Text != _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.ReEmployment) &&
                x.Text != _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.Active));

            StatusesList = new SelectList(allsStatuses, "Value", "Text");

            var reEmployment = _applicationEmployee.CreateStatuses()
                .Where(x => x.Text == _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.ReEmployment));

            ReEmploymentStatus = new SelectList(reEmployment, "Value", "Text");

            TempData["Active"] = _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.Active);
            TempData["ReEmployment"] = _enumExtension.EmployeeStatusesToPersianString(EmployeeStatuses.ReEmployment);

            const int take = 15;
            int count = _applicationEmployee.GetCount(search);
            int pageCount = (int)Math.Ceiling((double)count / take);

            if (pageCount < 1)
                pageCount = 1;

            if (pageId < 1)
                pageId = 1;

            if (pageId > pageCount)
                pageId = pageCount;

            var filterParamsCriteria = new FilterParamsCriteria
            {
                Take = take,
                PageCount = pageCount,
                PageId = pageId,
                Subject = search
            };

            FilterParams = new FilterParamsDto(filterParamsCriteria);
            Search = new SearchViewModel
            {
                FilterParams = FilterParams
            };
            Employees = _applicationEmployee.GetAll(FilterParams);
            TempData["NumberItems"] = _applicationEmployee.GetCount();
        }

        public RedirectToPageResult OnPost(int id)
        {
            var employee = _applicationEmployee.GetBy(id);
            if (Status != EmployeeStatuses.ReEmployment)
            {
                if(_repositoryEmployee.GetBy(id).AmountOwed != 0)
                {
                    TempData["ErrorMessage"] = _resultMessage.Error($"ابتدا بدهی معوقه ی {employee.FullName} را پرداخت کنید ");
                    return RedirectToPage();
                }
            }
            _applicationEmployee.ChangeStatus(id, Status);
            TempData["Message"] = _resultMessage.Success($"جایگاه {employee.FullName} با موفقیت ویرایش شد");
            return RedirectToPage();
        }

        public RedirectToPageResult OnGetPayed(int id)
        {
            if(_repositoryEmployee.GetBy(id).AmountOwed > _repositoryBudget.GetLast().TotalBudget)
            {
                TempData["ErrorMessage"] = _resultMessage.Error("عدم بودجه کافی");
                return RedirectToPage();
            }
            _applicationEmployee.PaySalary(id);
            var employee = _applicationEmployee.GetBy(id);
            TempData["Message"] = _resultMessage.Success($"دستمزد {employee.FullName} با موفقیت پزداخت شد");
            return RedirectToPage();
        }
    }
}
