// Project linked to my blog series 'C# Fluent Builder Pattern Combined to EF Core Step-by-Step' https://nick257.be
// (c) Nicolas Debrun Tonnemans 2022 - free to use for any purpose at the condition to mention these 2 commented lines

using BuilderConsoleApp.Context;
using BuilderConsoleApp.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
    logging.ClearProviders();
    logging.AddConsole().SetMinimumLevel(LogLevel.Warning);
    })
    .ConfigureServices(async (hostContext, services) =>
    {
        services.AddDbContext<GangOfFourContext>(options =>
        {
            options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GangOfFourPatternsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        });
        services.AddScoped<IBenefitService, BenefitService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddLogging();

        var _benefitService = services.BuildServiceProvider().GetService<IBenefitService>();
        await _benefitService.InsertDemoBenefitAsync();

        var _employeeService = services.BuildServiceProvider().GetService<IEmployeeService>();
        await _employeeService.TestFluentBuilderAsync();
    })
    .Build();

host.Run();
