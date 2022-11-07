using BuilderConsoleApp.BuilderPattern;
using BuilderConsoleApp.Context;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilderConsoleApp.Services
{
    public interface IEmployeeService
    {
        Task TestFluentBuilderAsync();
    }

    public class EmployeeService : IEmployeeService
    {
        public readonly GangOfFourContext _context;
        public EmployeeService(GangOfFourContext context)
        {
            _context = context;
        }

        public async Task TestFluentBuilderAsync()
        {
            //Client code example

            var cruelManager = new Employee.CruelManagerBuilder(context: _context)
                .WithName("John 'The Cruel'")
                .WithSalary(100000)
                .Build();
            await _context.Employees.AddAsync(cruelManager);
            await _context.SaveChangesAsync(); //Saving at each step here to allow reference to some ManagerId
            Console.WriteLine(cruelManager.ShowEmployee());


            var harshTeamLeader = new Employee.HarshTeamLeaderBuilder(context: _context)
                .WithName("Arthur 'The Harsh'")
                .HasManager("John 'The Cruel'")
                .WithSalary(50000)
                .Build();
            await _context.Employees.AddAsync(harshTeamLeader);
            await _context.SaveChangesAsync(); //Saving at each step here to allow reference to some ManagerId
            Console.WriteLine(harshTeamLeader.ShowEmployee());


            var poorDeveloper = new Employee.PoorDeveloperBuilder(context: _context)
                .WithName("John 'The Poor'")
                .HasManager("Arthur 'The Harsh'")
                .WithSalary(10000)
                .Build();
            await _context.Employees.AddAsync(poorDeveloper);
            await _context.SaveChangesAsync(); //Saving at each step here to allow reference to some ManagerId
            Console.WriteLine(poorDeveloper.ShowEmployee());
        }
    }
}
