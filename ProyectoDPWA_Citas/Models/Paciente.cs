using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    [Table("Paciente")]
    public partial class Paciente
    {
        public Paciente()
        {
            Cita = new HashSet<CIta>();
        }

        public int IdPaciente { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public int Edad { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        [InverseProperty(nameof(CIta.IdPacienteNavigation))]
        public virtual ICollection<CIta> Cita { get; set; }
    }
}
