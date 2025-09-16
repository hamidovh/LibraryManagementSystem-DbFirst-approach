using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using LibraryManagementSystem.BL;
using LibraryManagementSystem.DAL;

namespace LibraryManagementSystem.MVCUI.Models
{
    public partial class LibraryManagementSystemDBEntities : DbContext
    {
        public LibraryManagementSystemDBEntities()
            : base("name=LibraryManagementSystemDBEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Cerime> Cerime { get; set; }
        public virtual DbSet<Icare> Icare { get; set; }
        public virtual DbSet<Istifadechi> Istifadechi { get; set; }
        public virtual DbSet<Kateqoriya> Kateqoriya { get; set; }
        public virtual DbSet<Kitab> Kitab { get; set; }
        public virtual DbSet<Muellif> Muellif { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<Elaqe> Elaqe { get; set; }

        public System.Data.Entity.DbSet<LibraryManagementSystem.DAL.Elaqe> Elaqes { get; set; }
    }
}
