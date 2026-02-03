using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.DTOs.Common
{
    public interface IUnitOfWork
    {
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
    }

}
