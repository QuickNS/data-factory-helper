using DataFactoryHelper;
using DataFactoryViewer.Utils;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json");
var adfConfig = builder.Configuration.GetSection("DataFactory");
builder.Services.Configure<DataFactoryConfig>(adfConfig);
var dataFactoryConfig = adfConfig.Get<DataFactoryConfig>();

builder.Services.AddSingleton<IDataFactoryClient, DataFactoryClient>(
    provider => new DataFactoryClient(
        dataFactoryConfig.TenantId,
        dataFactoryConfig.SubscriptionId,
        dataFactoryConfig.ClientId,
        dataFactoryConfig.ClientSecret));

builder.Services.AddScoped<IDatasetHelper, DatasetHelper>(
    provider => new DatasetHelper(
        provider.GetService<IDataFactoryClient>(),
        dataFactoryConfig.ResourceGroupName,
        dataFactoryConfig.FactoryName));

builder.Services.AddScoped<ILinkedServicesHelper, LinkedServicesHelper>(
    provider => new LinkedServicesHelper(
        provider.GetService<IDataFactoryClient>(),
        dataFactoryConfig.ResourceGroupName,
        dataFactoryConfig.FactoryName));

builder.Services.AddScoped<IPipelinesHelper, PipelinesHelper>(
    provider => new PipelinesHelper(
        provider.GetService<IDataFactoryClient>(),
        dataFactoryConfig.ResourceGroupName,
        dataFactoryConfig.FactoryName));

builder.Services.AddScoped<IAdfSerializer, AdfSerializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
