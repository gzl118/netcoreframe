using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SANS.DbEntity.Models
{
    public partial class MyEFContext : DbContext
    {
        public MyEFContext()
        {
        }

        public MyEFContext(DbContextOptions<MyEFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DictAuthorityType> DictAuthorityType { get; set; }
        public virtual DbSet<SysAmRelated> SysAmRelated { get; set; }
        public virtual DbSet<SysAuthority> SysAuthority { get; set; }
        public virtual DbSet<SysMenu> SysMenu { get; set; }
        public virtual DbSet<SysRaRelated> SysRaRelated { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysUgrRelated> SysUgrRelated { get; set; }
        public virtual DbSet<SysUrRelated> SysUrRelated { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysUserGroup> SysUserGroup { get; set; }
        public virtual DbSet<SysDataDictionary> SysDataDictionary { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DictAuthorityType>(entity =>
            {
                entity.HasKey(e => e.AuthorityTypeId);

                entity.ToTable("Dict_AuthorityType");
            });
            modelBuilder.Entity<SysDataDictionary>(entity =>
            {
                entity.HasKey(e => e.dict_id);

                entity.ToTable("Sys_DataDictionary");

            });
            modelBuilder.Entity<SysAmRelated>(entity =>
            {
                entity.HasKey(e => e.AmRelatedId);

                entity.ToTable("Sys_AmRelated");

            });

            modelBuilder.Entity<SysAuthority>(entity =>
            {
                entity.HasKey(e => e.AuthorityId);

                entity.ToTable("Sys_Authority");
                entity.Property(e => e.DeleteSign).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<SysMenu>(entity =>
            {
                entity.HasKey(e => e.MenuId);

                entity.ToTable("Sys_Menu");
                entity.Property(e => e.DeleteSign).HasDefaultValueSql("1");
                entity.Property(e => e.MenuType).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<SysRaRelated>(entity =>
            {
                entity.HasKey(e => e.RaRelatedId);

                entity.ToTable("Sys_RaRelated");

            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("Sys_Role");
                entity.Property(e => e.DeleteSign).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<SysUgrRelated>(entity =>
            {
                entity.HasKey(e => e.UgrRelatedId);

                entity.ToTable("Sys_UgrRelated");

            });

            modelBuilder.Entity<SysUrRelated>(entity =>
            {
                entity.HasKey(e => e.UrRelatedId);

                entity.ToTable("Sys_UrRelated");

            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Sys_User");
                entity.Property(e => e.DeleteSign).HasDefaultValueSql("1");
            });

            modelBuilder.Entity<SysUserGroup>(entity =>
            {
                entity.HasKey(e => e.UserGroupId);
                entity.ToTable("Sys_UserGroup");
                entity.Property(e => e.DeleteSign).HasDefaultValueSql("1");
            });
        }
    }
}
