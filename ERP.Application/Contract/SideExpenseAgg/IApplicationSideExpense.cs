using ERP.Application.Contract.FilterAgg;

namespace ERP.Application.Contract.SideExpenseAgg
{
    public interface IApplicationSideExpense
    {
        void Create(CreateSideExpenseDto command);
        void Edit(EditSideExpenseDto command);
        SideExpenseViewModel GetBy(int id);
        List<SideExpenseViewModel> GetAll(FilterParamsDto filterParams);
        EditSideExpenseDto GetForEdit(int id);
        int GetCount(string? subject = null);
    }
}
