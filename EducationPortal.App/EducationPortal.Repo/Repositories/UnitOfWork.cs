using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;

namespace EducationPortal.Data.Repo.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PortalDbContext context;
    private readonly IRepositoryFactory repositoryFactory;
    private readonly Dictionary<Type, object> repositories = new();

    public UnitOfWork(PortalDbContext context, IRepositoryFactory repositoryFactory)
    {
        this.context = context;
        this.repositoryFactory = repositoryFactory;
    }

    public IRepository<T, TId> Repository<T, TId>() where T : class, IBaseEntity<TId>
    {
        var type = typeof(T);

        if (repositories.TryGetValue(type, out var existingRepository))
        {
            return (IRepository<T, TId>)existingRepository;
        }

        var repository = repositoryFactory.GetRepository<T, TId>();
        repositories[type] = repository;

        return repository;
    }

    public async Task<bool> SaveAsync()
    {
        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            foreach (var entry in ex.Entries)
            {
                entry.State = EntityState.Detached;
            }
            return false;
        }
    }

    public IDbContextTransaction BeginTransaction()
    {
        return context.Database.BeginTransaction();
    }

    public void Dispose() => context.Dispose();
}