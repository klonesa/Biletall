using Biletall.Core.Domain.Repositories;
using Biletall.DataAccess.Base;
using Biletall.DataAccess.EntityFramework.Context;
using Biletall.Entities.Concrete;
using Biletall.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Biletall.DataAccess.EntityFramework.UnityOfWork
{
    public class UoWBiletall : IUoWBiletall
    {
        public BiletallContext _dbContext { get; }

        public UoWBiletall(BiletallContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ApplicationRole> IdentityRoles => _dbContext.Roles.AsQueryable();

        public IQueryable<IdentityUserRole<string>> IdentityUserRoles => _dbContext.UserRoles.AsQueryable();

        public IQueryable<ApplicationUser> Users => _dbContext.Users.AsQueryable();

        public IQueryable<IdentityUserClaim<string>> IdentityUserClaim => _dbContext.UserClaims.AsQueryable();

        #region ENTITIES

        public IRepositoryBase<Reservation> Reservation => new EfRepositoryBase<Reservation>(_dbContext);

        #endregion

        public void ExecuteSqlRaw(string sqlCommand)
        {
            _dbContext.Database.SetCommandTimeout(new TimeSpan(0, 2, 0));
            _dbContext.Database.ExecuteSqlRaw(sqlCommand);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
