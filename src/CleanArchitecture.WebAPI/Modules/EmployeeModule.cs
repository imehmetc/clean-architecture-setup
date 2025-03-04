﻿using CleanArchitecture.Application.Employees;
using CleanArchitecture.Domanin.Employees;
using MediatR;
using TS.Result;

namespace CleanArchitecture.WebAPI.Modules
{
	public static class EmployeeModule
	{
		public static void RegisterEmployeeRoutes(this IEndpointRouteBuilder app)
		{
			RouteGroupBuilder group = app.MapGroup("/employees").WithTags("Employees").RequireAuthorization();

			group.MapPost(string.Empty,
				async (ISender sender, EmployeeCreateCommand request, CancellationToken cancellationToken) =>
				{
					var response = await sender.Send(request, cancellationToken);
					return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
				})
				.Produces<Result<string>>();

			group.MapGet(string.Empty,
				async (ISender sender, Guid Id, CancellationToken cancellationToken) =>
				{
					var response = await sender.Send(new EmployeeGetQuery(Id), cancellationToken);
					return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
				})
				.Produces<Result<Employee>>();
		}
	}
}
