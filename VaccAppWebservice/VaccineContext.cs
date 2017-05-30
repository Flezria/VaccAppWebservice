namespace VaccAppWebservice
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VaccineContext : DbContext
    {
        public VaccineContext()
            : base("name=VaccineContext")
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<user_childs> user_childs { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<vaccination_check> vaccination_check { get; set; }
        public virtual DbSet<vaccinations> vaccinations { get; set; }
        public virtual DbSet<vaccine_info> vaccine_info { get; set; }
        public virtual DbSet<vaccine_program> vaccine_program { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<user_childs>()
                .HasMany(e => e.vaccination_check)
                .WithRequired(e => e.user_childs)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<users>()
                .HasMany(e => e.user_childs)
                .WithRequired(e => e.users)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vaccine_info>()
                .HasMany(e => e.vaccinations)
                .WithRequired(e => e.vaccine_info)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vaccine_program>()
                .HasMany(e => e.user_childs)
                .WithRequired(e => e.vaccine_program)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vaccine_program>()
                .HasMany(e => e.vaccinations)
                .WithRequired(e => e.vaccine_program)
                .WillCascadeOnDelete(false);
        }
    }
}
