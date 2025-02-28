using CleanArchitecture.Domanin.Employees;
using CleanArchitecture.Infrastructure.Context;
using GenericRepository;

namespace CleanArchitecture.Infrastructure.Repositories
{
	internal sealed class EmployeeRepository : Repository<Employee, ApplicationDbContext>, IEmployeeRepository
	{
		public EmployeeRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
