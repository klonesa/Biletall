using Biletall.Core.Domain.Repositories;
using Biletall.Entities.Concrete;
using Biletall.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Biletall.DataAccess.EntityFramework.UnityOfWork
{
    public interface IUoWBiletall : IDisposable
    {
        IDbContextTransaction BeginTransaction();

        IQueryable<ApplicationRole> IdentityRoles { get; }
        IQueryable<ApplicationUser> Users { get; }
        IQueryable<IdentityUserRole<string>> IdentityUserRoles { get; }
        IQueryable<IdentityUserClaim<string>> IdentityUserClaim { get; }

        #region ENTITIES
        IRepositoryBase<Reservation> Reservation { get; }

        #endregion

        void ExecuteSqlRaw(string sqlCommand);
        Task<int> CommitAsync();
        int Commit();
    }
}
