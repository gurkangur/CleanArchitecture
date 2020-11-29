using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Persistence.Extensions;

namespace Persistence
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.AutoDetectChangesEnabled = false;

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContextBase).Assembly);
            modelBuilder.ApplyGlobalFilters<ISoftDelete>(e => !e.IsDeleted);
            //modelBuilder.ApplyGlobalFilters<bool>("IsDeleted", false);
        }
        public override int SaveChanges()
        {
            HandleSoftDeletableEntities();
            HandleAuditableEntities();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleSoftDeletableEntities();
            HandleAuditableEntities();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void HandleSoftDeletableEntities()
        {
            string currentUser = Thread.CurrentPrincipal?.Identity?.Name;
            var entries = ChangeTracker.Entries().Where(x => x.Entity is ISoftDelete && x.State == EntityState.Deleted);
            foreach (var entry in entries)
            {
                entry.State = EntityState.Modified;
                ISoftDelete entity = (ISoftDelete)entry.Entity;
                entity.IsDeleted = true;
                entity.DeleterUser = currentUser;
                entity.DeletionTime = DateTime.Now;
            }
        }
        private void HandleAuditableEntities()
        {
            string currentUser = Thread.CurrentPrincipal?.Identity?.Name;

            var entities = ChangeTracker.Entries().Where(x => x.Entity is IAuditedEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in entities)
            {
                if (entry.Entity is IAuditedEntity)
                {
                    IAuditedEntity auditable = ((IAuditedEntity)entry.Entity);

                    if (entry.State == EntityState.Added)
                    {
                        if (auditable.CreationTime == DateTime.MinValue)
                        {
                            auditable.CreationTime = DateTime.Now;
                            auditable.LastModificationTime = auditable.CreationTime;
                        }

                        if (String.IsNullOrEmpty(auditable.CreatorUser))
                        {
                            auditable.CreatorUser = currentUser;
                        }
                    }
                    else
                    {
                        if (!auditable.LastModificationTime.HasValue)
                        {
                            auditable.LastModificationTime = DateTime.Now;
                        }
                        auditable.LastModifierUser = currentUser;
                    }
                }
            }
        }

        DbSet<Category> Categories { get; set; }
    }
}