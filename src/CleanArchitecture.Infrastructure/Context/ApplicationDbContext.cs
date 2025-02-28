﻿using CleanArchitecture.Domanin.Abstractions;
using CleanArchitecture.Domanin.Employees;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Context
{
	internal sealed class ApplicationDbContext : DbContext,
		IUnitOfWork
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Employee> Employees { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<Entity>();

			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					entry.Property(p => p.CreateAt)
						.CurrentValue = DateTime.Now;
				}

				if (entry.State == EntityState.Modified)
				{
					if(entry.Property(p => p.IsDeleted).CurrentValue == true)
					{
						entry.Property(p => p.DeleteAt)
						.CurrentValue = DateTime.Now;
					}
					else
					{
						entry.Property(p => p.UpdateAt)
						.CurrentValue = DateTime.Now;
					}
				}

				if (entry.State == EntityState.Deleted) throw new ArgumentException("Db'den direk silme işlemi yapamazsınız.");
			}

			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
