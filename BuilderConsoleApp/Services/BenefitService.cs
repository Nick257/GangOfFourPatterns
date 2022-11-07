using BuilderConsoleApp.BuilderPattern;
using BuilderConsoleApp.Context;

using Microsoft.EntityFrameworkCore;

namespace BuilderConsoleApp.Services
{
    public interface IBenefitService
    {
        Task InsertDemoBenefitAsync();
    }

    public class BenefitService : IBenefitService
    {
        public readonly GangOfFourContext _context;
        public BenefitService(GangOfFourContext context)
        {
            _context = context;
        }

         public async Task InsertDemoBenefitAsync()
        {
            if (! await _context.Benefits.AnyAsync())
            {
                Console.WriteLine($"Inserting some benefits that can be granted to employees at next step." + Environment.NewLine);

                List<Benefit> benefits = new();

                benefits.Add(new Benefit
                {
                    BenefitName = "Access badge",
                    AvailableForThisEmployeePositionAndHigher = EmployeePosition.PoorDeveloper
                });

                benefits.Add(new Benefit
                {
                    BenefitName = "Sauna/massage session",
                    AvailableForThisEmployeePositionAndHigher = EmployeePosition.HarshTeamLeader
                });

                benefits.Add(new Benefit
                {
                    BenefitName = "Ferrari 458 Speciale",
                    AvailableForThisEmployeePositionAndHigher = EmployeePosition.CruelManager
                });

                await _context.Benefits.AddRangeAsync(benefits);
                await _context.SaveChangesAsync();

                foreach (var p in benefits)
                {
                    Console.WriteLine($"benefit type added to store : {p.BenefitName} {p.BenefitId}" + Environment.NewLine);
                }
            }
        }
    }
}
