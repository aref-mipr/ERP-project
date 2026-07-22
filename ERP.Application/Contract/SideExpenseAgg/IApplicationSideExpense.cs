namespace ERP.Application.Contract.SideExpenseAgg
{
    public interface IApplicationSideExpense
    {
        void Create(CreateSideExpenseDto command);
        void Edit(EditSideExpenseDto command);
        SideExpenseViewModel GetBy(int id);
        EditSideExpenseDto GetForEdit(int id);
        List<SideExpenseViewModel> GetAll();
    }
}
