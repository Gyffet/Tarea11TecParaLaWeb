using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Formulario.Model
{
    public partial class FormularioContext : DbContext
    {
        public FormularioContext()
        {
        }

        public FormularioContext(DbContextOptions<FormularioContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Datos> Datos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DatabaseConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Datos>(entity =>
            {
                entity.HasKey(e => e.pkFormulario);

                entity.Property(e => e.NoContrato).IsUnicode(false);

                entity.Property(e => e.apellido).IsUnicode(false);

                entity.Property(e => e.correo).IsUnicode(false);

                entity.Property(e => e.curso).IsUnicode(false);

                entity.Property(e => e.direccion).IsUnicode(false);

                entity.Property(e => e.documento).IsUnicode(false);

                entity.Property(e => e.fechaInscripcion).HasColumnType("date");

                entity.Property(e => e.fechaNacimiento).HasColumnType("date");

                entity.Property(e => e.formaPago).IsUnicode(false);

                entity.Property(e => e.horario).IsUnicode(false);

                entity.Property(e => e.nombre).IsUnicode(false);

                entity.Property(e => e.personaAtendio).IsUnicode(false);

                entity.Property(e => e.telefono).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
