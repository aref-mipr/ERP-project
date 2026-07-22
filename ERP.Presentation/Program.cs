using ERP.Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

Bootstrapper.Config(builder.Services, builder.Configuration.GetConnectionString("ERPv01-DB"));
//builder.Services.AddScoped<InitialCapitalFilter>();

builder.Services.AddRazorPages();

//builder.Services.Configure<MvcOptions>(options =>
//{
//    options.Filters.AddService<InitialCapitalFilter>();
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
