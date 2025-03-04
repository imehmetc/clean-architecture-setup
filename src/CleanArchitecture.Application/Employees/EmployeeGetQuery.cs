using CleanArchitecture.Domanin.Employees;
using MediatR;
using TS.Result;

namespace CleanArchitecture.Application.Employees;

public sealed record  EmployeeGetQuery(
	Guid Id) : IRequest<Result<Employee>>;

public sealed class EmployeeGetQueryHandler(
	IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeGetQuery, Result<Employee>>
{
	public async Task<Result<Employee>> Handle(EmployeeGetQuery request, CancellationToken cancellationToken)
	{
		var employee = await employeeRepository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		if (employee is null) return Result<Employee>.Failure("Personel Bulunamadı");

		return employee;
	}
}
