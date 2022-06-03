using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Formulario.Model
{
    public partial class CedulasEvaluacionContext : DbContext
    {
        public CedulasEvaluacionContext()
        {
        }

        public CedulasEvaluacionContext(DbContextOptions<CedulasEvaluacionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArchivosCedulas> ArchivosCedulas { get; set; }
        public virtual DbSet<CatalogoServicios> CatalogoServicios { get; set; }
        public virtual DbSet<CedFumFechas> CedFumFechas { get; set; }
        public virtual DbSet<CedFumFumigacion> CedFumFumigacion { get; set; }
        public virtual DbSet<CedFumHoras> CedFumHoras { get; set; }
        public virtual DbSet<CedLimpActividades> CedLimpActividades { get; set; }
        public virtual DbSet<CedLimpEquipo> CedLimpEquipo { get; set; }
        public virtual DbSet<CedMenAcuse> CedMenAcuse { get; set; }
        public virtual DbSet<CedMenEntrega> CedMenEntrega { get; set; }
        public virtual DbSet<CedMenExtravio> CedMenExtravio { get; set; }
        public virtual DbSet<CedMenMalEstado> CedMenMalEstado { get; set; }
        public virtual DbSet<CedMenMaterial> CedMenMaterial { get; set; }
        public virtual DbSet<CedMenRecoleccion> CedMenRecoleccion { get; set; }
        public virtual DbSet<CedulasAreaInmueble> CedulasAreaInmueble { get; set; }
        public virtual DbSet<ContratoCedula> ContratoCedula { get; set; }
        public virtual DbSet<EntregablesFumigacion> EntregablesFumigacion { get; set; }
        public virtual DbSet<EntregablesLimpieza> EntregablesLimpieza { get; set; }
        public virtual DbSet<EvaluacionFumigacion> EvaluacionFumigacion { get; set; }
        public virtual DbSet<EvaluacionLimpieza> EvaluacionLimpieza { get; set; }
        public virtual DbSet<EvaluacionMensajeria> EvaluacionMensajeria { get; set; }
        public virtual DbSet<InmueblesCJF> InmueblesCJF { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DatabaseConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArchivosCedulas>(entity =>
            {
                entity.Property(e => e.FechaCarga).HasColumnType("date");

                entity.Property(e => e.contentType)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.fcNombreOriginal)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.fkNoCed)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatalogoServicios>(entity =>
            {
                entity.HasKey(e => e.pkCatalogo);

                entity.Property(e => e.Servicio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedFumFechas>(entity =>
            {
                entity.HasKey(e => e.pkFecha);

                entity.Property(e => e.fcComentarios)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaProgramada).HasColumnType("datetime");

                entity.Property(e => e.fdFechaRealizada).HasColumnType("datetime");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedFumFumigacion>(entity =>
            {
                entity.HasKey(e => e.pkFumigacion);

                entity.Property(e => e.fcComentarios)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaFumigacion).HasColumnType("datetime");

                entity.Property(e => e.fdFechaReaparacion).HasColumnType("datetime");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedFumHoras>(entity =>
            {
                entity.HasKey(e => e.pkHoras);

                entity.Property(e => e.fcComentarios)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaProgramada).HasColumnType("datetime");

                entity.Property(e => e.fdHoraEfectiva).HasColumnType("datetime");

                entity.Property(e => e.fdHoraProgramada).HasColumnType("datetime");

                entity.Property(e => e.fkCedula)
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedLimpActividades>(entity =>
            {
                entity.HasKey(e => e.pkActividad);

                entity.Property(e => e.fcArea)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.fcComentarios)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.fcTipo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaInci).HasColumnType("datetime");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedLimpEquipo>(entity =>
            {
                entity.HasKey(e => e.pkEquipo);

                entity.Property(e => e.fcComentarios)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.fcMaquina)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaInci).HasColumnType("datetime");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedMenAcuse>(entity =>
            {
                entity.HasKey(e => e.pkAcuse)
                    .HasName("PK_CedMenAcuses");

                entity.Property(e => e.fdFechaEfectiva).HasColumnType("datetime");

                entity.Property(e => e.fdFechaProgramada).HasColumnType("datetime");

                entity.Property(e => e.fiPena).HasColumnType("money");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedMenEntrega>(entity =>
            {
                entity.HasKey(e => e.pkEntrega);

                entity.Property(e => e.fcCodRastreo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoGuia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcTipoServicio)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaEfectiva).HasColumnType("datetime");

                entity.Property(e => e.fdFechaProgramada).HasColumnType("datetime");

                entity.Property(e => e.fiPena).HasColumnType("money");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedMenExtravio>(entity =>
            {
                entity.HasKey(e => e.pkExtraviado);

                entity.Property(e => e.fcCodRastreo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoGuia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcTipoServicio)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedMenMalEstado>(entity =>
            {
                entity.HasKey(e => e.pkMalEstado);

                entity.Property(e => e.fcCodRastreo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoGuia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcTipoServicio)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fiPena).HasColumnType("money");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedMenMaterial>(entity =>
            {
                entity.HasKey(e => e.pkMaterial)
                    .HasName("PK_CedMenMaterialUniforme");

                entity.Property(e => e.fiPenaMaterial).HasColumnType("money");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedMenRecoleccion>(entity =>
            {
                entity.HasKey(e => e.pkRecoleccion);

                entity.Property(e => e.fcCodRastreo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoGuia)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcTipoServicio)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaEfectiva).HasColumnType("datetime");

                entity.Property(e => e.fdFechaProgramada).HasColumnType("datetime");

                entity.Property(e => e.fiPena).HasColumnType("money");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedulasAreaInmueble>(entity =>
            {
                entity.HasKey(e => e.pkClave);
            });

            modelBuilder.Entity<ContratoCedula>(entity =>
            {
                entity.HasKey(e => e.pkContrato)
                    .HasName("PK_Contratos");

                entity.Property(e => e.fcDescrpcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoContrato)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaFin).HasColumnType("date");

                entity.Property(e => e.fdFechaInicio).HasColumnType("date");
            });

            modelBuilder.Entity<EntregablesFumigacion>(entity =>
            {
                entity.Property(e => e.fdActaCierre).HasColumnType("date");

                entity.Property(e => e.fdActaEntrega).HasColumnType("date");

                entity.Property(e => e.fdActaInicio).HasColumnType("date");

                entity.Property(e => e.fdCierreMes).HasColumnType("date");

                entity.Property(e => e.fdListadePersonal).HasColumnType("date");

                entity.Property(e => e.fdPersonal).HasColumnType("date");

                entity.Property(e => e.fdProgramaOperacion).HasColumnType("date");

                entity.Property(e => e.fdReporteServ).HasColumnType("date");

                entity.Property(e => e.fdSUAIMSS).HasColumnType("date");

                entity.Property(e => e.fdUniforme).HasColumnType("date");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntregablesLimpieza>(entity =>
            {
                entity.Property(e => e.fdActaCierre).HasColumnType("date");

                entity.Property(e => e.fdActaInicio).HasColumnType("date");

                entity.Property(e => e.fdActaServicios).HasColumnType("date");

                entity.Property(e => e.fdCierreMes).HasColumnType("date");

                entity.Property(e => e.fdGafeteUniforme).HasColumnType("date");

                entity.Property(e => e.fdProgramaOperacion).HasColumnType("date");

                entity.Property(e => e.fdSUAIMSS).HasColumnType("date");

                entity.Property(e => e.fdVisitaInstalaciones).HasColumnType("date");

                entity.Property(e => e.fkCedula)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EvaluacionFumigacion>(entity =>
            {
                entity.HasKey(e => e.pkPenalizacion);

                entity.Property(e => e.fcCorreoUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fcInmueble)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fcMes)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoCed)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.fcNumFactura)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcServicio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaEnviado).HasColumnType("datetime");

                entity.Property(e => e.fdFechaGuardado).HasColumnType("datetime");

                entity.Property(e => e.fiCalificacion).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.fiMontoFactura).HasColumnType("money");

                entity.Property(e => e.fiPenaCalificacion).HasColumnType("money");

                entity.Property(e => e.fiPenaEtiquetas).HasColumnType("money");

                entity.Property(e => e.fiPenaFechas).HasColumnType("money");

                entity.Property(e => e.fiPenaFumigacion).HasColumnType("money");

                entity.Property(e => e.fiPenaHorario).HasColumnType("money");

                entity.Property(e => e.fiPenaPO).HasColumnType("money");
            });

            modelBuilder.Entity<EvaluacionLimpieza>(entity =>
            {
                entity.HasKey(e => e.pkPenalizacion);

                entity.Property(e => e.fcCorreoUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fcInmueble)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fcMes)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoCed)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.fcNumFactura)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcPenaActividades).HasColumnType("money");

                entity.Property(e => e.fcPenaMaterial).HasColumnType("money");

                entity.Property(e => e.fcPenaUniforme).HasColumnType("money");

                entity.Property(e => e.fcServicio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaEnviado).HasColumnType("datetime");

                entity.Property(e => e.fdFechaGuardado).HasColumnType("datetime");

                entity.Property(e => e.fiCalificacion).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.fiMontoFactura).HasColumnType("money");

                entity.Property(e => e.fiPenaCalificacion).HasColumnType("money");

                entity.Property(e => e.fiPenaCapacitacion).HasColumnType("money");

                entity.Property(e => e.fiPenaEntregables).HasColumnType("money");

                entity.Property(e => e.fiPenaPO).HasColumnType("money");

                entity.Property(e => e.fiPenaVisita).HasColumnType("money");
            });

            modelBuilder.Entity<EvaluacionMensajeria>(entity =>
            {
                entity.HasKey(e => e.pkPenalizacion)
                    .HasName("PK_PenalizacionMensajeria");

                entity.Property(e => e.fcCorreoUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fcInmueble)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fcMes)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcNoCed)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.fcNumFactura)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcPenaAcuses)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fcPenaEntrega)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fcPenaEstado)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fcPenaExtravio)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fcPenaMaterial)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fcPenaRecoleccion)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fcPenaUniforme)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fcServicio)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.fcUsuario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.fdFechaEnviado).HasColumnType("datetime");

                entity.Property(e => e.fdFechaGuardado).HasColumnType("datetime");

                entity.Property(e => e.fdFechaPO).HasColumnType("datetime");

                entity.Property(e => e.fiCalificacion).HasColumnType("decimal(5, 1)");

                entity.Property(e => e.fiMontoFactura).HasColumnType("money");

                entity.Property(e => e.fiPenaCalificacion)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.fiPenaPO).HasColumnType("money");
            });

            modelBuilder.Entity<InmueblesCJF>(entity =>
            {
                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaFinVigencia).HasColumnType("datetime");

                entity.Property(e => e.FechaInicioVigencia).HasColumnType("datetime");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
