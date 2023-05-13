using DataFactoryHelper;
using DataFactoryViewer.Utils;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(
    options =>
    {
        options.SerializerSettings.Formatting = Formatting.Indented;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
        options.SerializerSettings.Converters = JsonUtils.GetJsonConverters();
    }
    );

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json");
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

builder.Services.AddScoped<ITriggersHelper, TriggersHelper>(
    provider => new TriggersHelper(
        provider.GetService<IDataFactoryClient>(),
        dataFactoryConfig.ResourceGroupName,
        dataFactoryConfig.FactoryName));

builder.Services.AddScoped<IDataflowsHelper, DataflowsHelper>(
    provider => new DataflowsHelper(
        provider.GetService<IDataFactoryClient>(),
        dataFactoryConfig.ResourceGroupName,
        dataFactoryConfig.FactoryName));

builder.Services.AddScoped<IAdfSerializer, AdfSerializer>();
builder.Services.AddScoped<IJsonToBicepConverter, JsonToBicepConverter>();

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
