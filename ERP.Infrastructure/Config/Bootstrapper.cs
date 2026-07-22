using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Application.Service;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using ERP.Infrastructure.Context;
using ERP.Infrastructure.Repository;
using ERP.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.OrderItemAgg;
using ERP.Application.Contract.EmployeeAgg;
using ERP.Application.Contract.SideExpenseAgg;
using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.FinancialTransactionAgg;

namespace ERP.Infrastructure.Config
{
    public class Bootstrapper
    {
        public static void Config(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IResultMessage, ResultMessage>();

            services.AddScoped<IRepositoryProductCategory, RepositoryProductCategory>();
            services.AddScoped<IApplicationProductCategory, ApplicationProductCategory>();

            services.AddScoped<IRepositoryProduct, RepositoryProduct>();
            services.AddScoped<IApplicationProduct, ApplicationProduct>();

            services.AddScoped<IRepositoryProductItem, RepositoryProductItem>();
            services.AddScoped<IApplicationProductItem, ApplicationProductItem>();

            services.AddScoped<IRepositoryCustomer, RepositoryCustomer>();
            services.AddScoped<IApplicationCustomer, ApplicationCustomer>();

            services.AddScoped<IRepositoryOrder, RepositoryOrder>();
            services.AddScoped<IApplicationOrder, ApplicationOrder>();

            services.AddScoped<IRepositoryOrderItem, RepositoryOrderItem>();
            services.AddScoped<IApplicationOrderItem, ApplicationOrderItem>();

            services.AddScoped<IRepositoryEmployee, RepositoryEmployee>();
            services.AddScoped<IApplicationEmployee, ApplicationEmployee>();

            services.AddScoped<IRepositorySideExpense, RepositorySideExpense>();
            services.AddScoped<IApplicationSideExpense, ApplicationSideExpense>();

            services.AddScoped<IRepositoryBudget, RepositoryBudget>();
            services.AddScoped<IApplicationBudget, ApplicationBudget>();

            services.AddScoped<IRepositoryFinancialTransaction, RepositoryFinancialTransaction>();
            services.AddScoped<IApplicationFinancialTransaction, ApplicationFinancialTransaction>();

            services.AddScoped<IEnumExtension, EnumExtension>();

            services.AddDbContext<ERPContext>(options => options.UseSqlServer(connectionString));

            services.AddControllersWithViews();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateProductCategoryValidator>();
        }
    }
}
