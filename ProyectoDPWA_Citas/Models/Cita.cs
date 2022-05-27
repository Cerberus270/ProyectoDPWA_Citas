using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    public partial class CIta
    {
        public CIta()
        {
            Diagnosticos = new HashSet<Diagnostico>();
        }

        [Key]
        [Display(Name = "Cod. Cita")]
        [Column("idCita")]
        public int IdCita { get; set; }

        [Required(ErrorMessage = "El Paciente no puede estar vacío")]
        [Display(Name = "Paciente")]
        [Column("idPaciente")]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "La fecha de la cita no puede estar vacía")]
        [Display(Name = "Fecha")]
        [Column("fecha", TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora de la cita no puede estar vacía")]
        [Display(Name = "Hora")]
        [Column("hora")]
        public TimeSpan Hora { get; set; }

        [Required(ErrorMessage = "El estado de la cita no puede estar vacío")]
        [Display(Name = "Estado Cita")]
        [RegularExpression("Confirmada|Pendiente|Cancelada|Finalizada", ErrorMessage = "Ingrese un estado valido de la cita")]
        [Column("estado")]
        [StringLength(15)]
        public string Estado { get; set; }

        [Display(Name = "Paciente")]
        [ForeignKey(nameof(IdPaciente))]
        [InverseProperty(nameof(Paciente.Cita))]
        public virtual Paciente IdPacienteNavigation { get; set; }
        [InverseProperty(nameof(Diagnostico.IdCitaNavigation))]
        public virtual ICollection<Diagnostico> Diagnosticos { get; set; }
    }
}
