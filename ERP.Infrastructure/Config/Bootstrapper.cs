using ERP.Application.Contract.BudgetAgg;
using ERP.Application.Contract.CustomerAgg;
using ERP.Application.Contract.EmployeeAgg;
using ERP.Application.Contract.FinancialTransactionAgg;
using ERP.Application.Contract.OrderAgg;
using ERP.Application.Contract.OrderItemAgg;
using ERP.Application.Contract.ProductAgg;
using ERP.Application.Contract.ProductCategoryAgg;
using ERP.Application.Contract.ProductItemAgg;
using ERP.Application.Contract.SideExpenseAgg;
using ERP.Application.Contract.UserAgg;
using ERP.Application.Service;
using ERP.Domain.Interface.Repository;
using ERP.Domain.Interface.Utility;
using ERP.Infrastructure.Context;
using ERP.Infrastructure.Repository;
using ERP.Infrastructure.Utility;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddScoped<IRepositoryUser, RepositoryUser>();
            services.AddScoped<IApplicationUser, ApplicationUser>();

            services.AddScoped<IEnumExtension, EnumExtension>();
            services.AddScoped<IEncoder, Encoder>();
            services.AddScoped<IFileManager, FileManager>();

            services.AddDbContext<ERPContext>(options => options.UseSqlServer(connectionString));

            services.AddControllersWithViews();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateProductCategoryValidator>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(option =>
            {
                option.LoginPath = "/User/Login";
                option.ExpireTimeSpan = TimeSpan.FromDays(10);
            });
            services.AddHttpContextAccessor();
        }
    }
}
