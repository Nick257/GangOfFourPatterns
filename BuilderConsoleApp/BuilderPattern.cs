using BuilderConsoleApp.Context;

using System.ComponentModel.DataAnnotations.Schema;

namespace BuilderConsoleApp.BuilderPattern
{
    public enum EmployeePosition
    {
        PoorDeveloper = 0,
        HarshTeamLeader = 1,
        CruelManager = 2
    }

    public interface IEmployee
    {
        Guid EmployeeId { get; }
        Guid? ManagerId { get; }
        string Name { get; }
        EmployeePosition EmployeePosition { get; }
        int Salary { get; }
        ICollection<Benefit> Benefits { get; }

        string ShowEmployee();
    }

    [Table("Employees")]
    public class Employee : IEmployee
    {
        private Guid _employeeId;
        private Guid? managerId;
        private string name;
        private int salary;
        private EmployeePosition _employeePosition;
        private ICollection<Benefit> benefits;

        public Guid EmployeeId
        {
            get { return _employeeId; }
            private set { _employeeId = Guid.NewGuid(); }
        }
        public Guid? ManagerId
        {
            get { return managerId; }
            private set { managerId = value; }
        }
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }
        public int Salary
        {
            get { return salary; }
            private set { salary = value; }
        }
        public EmployeePosition EmployeePosition
        {
            get { return _employeePosition; }
            private set { _employeePosition = value; }
        }
        public ICollection<Benefit> Benefits
        {
            get { return benefits; }
            private set { benefits = value; }
        }

        public string ShowEmployee()
        {
            return $" Employee Id : {EmployeeId} | Name: {Name}\n Has Manager with ID : {ManagerId}\n Salary: {Salary}\n Position: {EmployeePosition}" + Environment.NewLine
                + "  .benefit granted : " + String.Join(Environment.NewLine + "  .benefit granted : ", Benefits.Select(b => b.BenefitName))
                + Environment.NewLine;
        }

        public class CruelManagerBuilder
        {
            private readonly Employee _employee;
            private readonly GangOfFourContext _context;

            public CruelManagerBuilder(GangOfFourContext context)
            {
                _context = context;
                _employee = new Employee();
                _employee.managerId = null;
                _employee._employeePosition = EmployeePosition.CruelManager;
                _employee.benefits = _context.Benefits
                    .Where(p => p.AvailableForThisEmployeePositionAndHigher <= _employee.EmployeePosition)
                    .ToList();
            }

            public CruelManagerBuilder WithName(string name)
            {
                _employee.name = name;
                return this;
            }

            public CruelManagerBuilder WithSalary(int salary)
            {
                _employee.salary = salary;
                return this;
            }

            public Employee Build() => _employee;
        }

        public class HarshTeamLeaderBuilder
        {
            private readonly Employee _employee;
            private readonly GangOfFourContext _context;

            public HarshTeamLeaderBuilder(GangOfFourContext context)
            {
                _context = context;
                _employee = new Employee();

                _employee.managerId = _context.Employees
                    .Where(p => (int)p.EmployeePosition == ((int)_employee.EmployeePosition + 1))
                    .Select(x => x.EmployeeId)
                    .FirstOrDefault();

                _employee._employeePosition = EmployeePosition.HarshTeamLeader;

                _employee.benefits = _context.Benefits
                    .Where(p => p.AvailableForThisEmployeePositionAndHigher <= _employee.EmployeePosition)
                    .ToList();
            }

            public HarshTeamLeaderBuilder WithName(string name)
            {
                _employee.name = name;
                return this;
            }
            public HarshTeamLeaderBuilder HasManager(string name)
            {
                _employee.managerId = _context.Employees.Where(p => p.Name.Contains(name)).Select(x => x.EmployeeId).FirstOrDefault();
                return this;
            }

            public HarshTeamLeaderBuilder WithSalary(int salary)
            {
                _employee.salary = salary;
                return this;
            }

            public Employee Build() => _employee;
        }

        public class PoorDeveloperBuilder
        {
            private readonly Employee _employee;
            private readonly GangOfFourContext _context;

            public PoorDeveloperBuilder(GangOfFourContext context)
            {
                _context = context;
                _employee = new Employee();

                _employee.managerId = _context.Employees
                    .Where(p => (int)p.EmployeePosition == ((int)_employee.EmployeePosition + 1))
                    .Select(x => x.EmployeeId)
                    .FirstOrDefault();

                _employee._employeePosition = EmployeePosition.PoorDeveloper;

                _employee.benefits = _context.Benefits
                    .Where(p => p.AvailableForThisEmployeePositionAndHigher <= _employee.EmployeePosition)
                    .ToList();
            }

            public PoorDeveloperBuilder WithName(string name)
            {
                _employee.name = name;
                return this;
            }

            public PoorDeveloperBuilder HasManager(string name)
            {
                _employee.managerId = _context.Employees.Where(p => p.Name.Contains(name)).Select(x => x.EmployeeId).FirstOrDefault();
                return this;
            }

            public PoorDeveloperBuilder WithSalary(int salary)
            {
                _employee.salary = salary;
                return this;
            }

            public Employee Build() => _employee;
        }
    }


    [Table("Benefits")]
    public class Benefit
    {
        public Guid BenefitId { get; set; } = Guid.NewGuid();
        public string BenefitName { get; set; }
        public EmployeePosition AvailableForThisEmployeePositionAndHigher { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
