using Append.Blazor.Clipboard;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoneyManagement_Web;
using MoneyManagement_Web.Interfaces;
using MoneyManagement_Web.Services;
using Radzen;
using Tewr.Blazor.FileReader;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<LocalStorageAccessor>();
builder.Services.AddScoped<SessionStorageAccessor>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();

builder.Services.AddFileReaderService(options =>
{
    options.UseWasmSharedBuffer = true;
});

builder.Services.AddScoped<IAccessServices, AccessServices>();
builder.Services.AddScoped<IUtilityServices, UtilityServices>();
builder.Services.AddScoped<IAnchillaryService, AnchillaryService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IIdentityAccessService, IdentityAccessService>();
builder.Services.AddScoped<IHouseThingsService, HouseThingsService>();
builder.Services.AddScoped<IStatisticService, StatisticService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<ITestServices, TestServices>();
builder.Services.AddClipboard();
await builder.Build().RunAsync();