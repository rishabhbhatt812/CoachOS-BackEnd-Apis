using System;
using System.Collections.Generic;
using System.Text;

namespace CoachOS.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
    }
}
