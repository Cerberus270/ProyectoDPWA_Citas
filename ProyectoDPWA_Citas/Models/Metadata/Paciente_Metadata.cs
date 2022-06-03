using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProyectoDPWA_Citas.Models.Metadata
{
    [Table("Paciente")]
    public class Paciente_Metadata
    {
        public Paciente_Metadata()
        {
            Cita = new HashSet<CIta>();
        }

        [Key]
        [Display(Name = "Cod. Paciente")]
        [Column("idPaciente")]
        public int IdPaciente { get; set; }
        [Required(ErrorMessage = "Los Nombres del paciente no puede estar vacío")]
        [Display(Name = "Nombres Paciente")]
        [Column("nombres")]
        [StringLength(50)]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los Apellidos del paciente no puede estar vacío")]
        [Display(Name = "Apellidos Paciente")]
        [Column("apellidos")]
        [StringLength(50)]
        public string Apellidos { get; set; }

        [Display(Name = "Fec. Nacimiento")]
        [Column("fechaNacimiento", TypeName = "date")]
        public DateTime FechaNacimiento { get; set; }

        [Column("edad")]
        public int Edad { get; set; }

        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "La dirección del paciente no puede estar vacía")]
        [Column("direccion")]
        [StringLength(150)]
        public string Direccion { get; set; }

        [Column("telefono")]
        [Required(ErrorMessage = "El teléfono del paciente no puede estar vacío")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "Teléfono en formato 0000-0000")]
        [Display(Name = "Teléfono")]
        [StringLength(9)]
        public string Telefono { get; set; }

        [InverseProperty(nameof(CIta.IdPacienteNavigation))]
        public virtual ICollection<CIta> Cita { get; set; }
    }
}
