using CleanArchitecture.Application.Employees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CleanArchitecture.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery] // ekstra sorgu atabilmeyi sağlar

public class AppODataController(ISender sender) : ODataController
{
	public static IEdmModel GetEdmModel() // openapi
	{
		ODataConventionModelBuilder builder = new();
		builder.EnableLowerCamelCase();
		builder.EntitySet<EmployeeGetAllQueryResponse>("employees");
		return builder.GetEdmModel();
	}


	[HttpGet("employees")]
	public async Task<IQueryable<EmployeeGetAllQueryResponse>> GetAllEmployees(CancellationToken cancellationToken)
	{
		var response = await sender.Send(new EmployeeGetAllQuery(), cancellationToken);

		return response;
	}

	// IActionResult yerine IQueryable<EmployeeGetAllQueryResponse> yazıldığında scalar'da dönecek cevap görünür.
}
