using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProyectoDPWA_Citas.Models;

#nullable disable

namespace ProyectoDPWA_Citas.Data
{
    public partial class ClinicaModContext : DbContext
    {
        public ClinicaModContext(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public ClinicaModContext(DbContextOptions<ClinicaModContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CIta> Cita { get; set; }
        public virtual DbSet<DetallesReceta> DetallesReceta { get; set; }
        public virtual DbSet<Diagnostico> Diagnosticos { get; set; }
        public virtual DbSet<Paciente> Pacientes { get; set; }
        public virtual DbSet<Receta> Receta { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(local);initial catalog=ClinicaMod; trusted_connection=yes;");
            }

            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<CIta>(entity =>
            {
                entity.HasKey(e => e.IdCita)
                    .HasName("PK__Cita__814F31262E66CF3A");

                entity.Property(e => e.Estado).IsUnicode(false);

                entity.HasOne(d => d.IdPacienteNavigation)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.IdPaciente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_paciente_cita");
            });

            modelBuilder.Entity<DetallesReceta>(entity =>
            {
                entity.HasKey(e => new { e.IdDetalleReceta, e.IdReceta })
                    .HasName("PK__Detalles__7DADBCB18EFC182C");

                entity.Property(e => e.IdDetalleReceta).ValueGeneratedOnAdd();

                entity.Property(e => e.Indicaciones).IsUnicode(false);

                entity.Property(e => e.Medicamento).IsUnicode(false);

                entity.HasOne(d => d.IdRecetaNavigation)
                    .WithMany(p => p.DetallesReceta)
                    .HasForeignKey(d => d.IdReceta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_receta_detalle");
            });

            modelBuilder.Entity<Diagnostico>(entity =>
            {
                entity.HasKey(e => e.IdDiagnostico)
                    .HasName("PK__Diagnost__F1C0667A38003785");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.HasOne(d => d.IdCitaNavigation)
                    .WithMany(p => p.Diagnosticos)
                    .HasForeignKey(d => d.IdCita)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cita_diagnostico");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.IdPaciente)
                    .HasName("PK__Paciente__F48A08F247C3EC7A");

                entity.Property(e => e.Apellidos).IsUnicode(false);

                entity.Property(e => e.Direccion).IsUnicode(false);

                entity.Property(e => e.Nombres).IsUnicode(false);

                entity.Property(e => e.Telefono).IsUnicode(false);
            });

            modelBuilder.Entity<Receta>(entity =>
            {
                entity.HasKey(e => e.IdReceta)
                    .HasName("PK__Receta__7D03FC81CAFF90DF");

                entity.HasOne(d => d.IdDiagnosticoNavigation)
                    .WithMany(p => p.Receta)
                    .HasForeignKey(d => d.IdDiagnostico)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_diagnostico_receta");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__Usuario__F3DBC57327DD0E24");

                entity.Property(e => e.Username).IsUnicode(false);

                entity.Property(e => e.Contraseña).IsUnicode(false);

                entity.Property(e => e.TipoUsuario).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
