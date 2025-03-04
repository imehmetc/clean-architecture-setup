using CleanArchitecture.Application.Employees;
using CleanArchitecture.Domanin.Employees;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CleanArchitecture.Application.Employees
{
	public sealed record EmployeeCreateCommand(
		string FirstName,
		string LastName,
		DateOnly BirthOfDate,
		decimal Salary,
		PersonnelInformation PersonnelInformation,
		Address? Address,
		bool IsActive) : IRequest<Result<string>>;
}

public sealed class EmployeeCreateCommandValidatior : AbstractValidator<EmployeeCreateCommand>
{
	public EmployeeCreateCommandValidatior()
	{
		RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır!");
		RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır!");
		RuleFor(x => x.PersonnelInformation.IdentityNumber)
			.MinimumLength(11).WithMessage("Geçerli bir TC Numarası girin!")
			.MaximumLength(11).WithMessage("Geçerli bir TC Numarası girin!");
	}
}


internal sealed class EmployeeCreateCommandHandler(
	IEmployeeRepository employeeRepository,
	IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
	public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
	{
		var isEmployeeExists = await employeeRepository.AnyAsync(p => p.PersonnelInformation.IdentityNumber == request.PersonnelInformation.IdentityNumber, cancellationToken);

		if (isEmployeeExists)
		{
			return Result<string>.Failure("Bu TC numarası daha önce kaydedilmiş");
		}


		Employee employee = request.Adapt<Employee>();

		employeeRepository.Add(employee);

		await unitOfWork.SaveChangesAsync(cancellationToken);

		return "Personel kaydı başarıyla tamamlandı!";
	}
}