using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    [Table("Diagnostico")]
    public partial class Diagnostico
    {
        public Diagnostico()
        {
            Receta = new HashSet<Receta>();
        }

        [Key]
        [Column("idDiagnostico")]
        public int IdDiagnostico { get; set; }
        [Column("idCita")]
        public int IdCita { get; set; }
        [Required(ErrorMessage = "La descripción del diagnóstico no puede estar vacia")]
        [Display(Name = "Descripción diagnóstico")]
        [Column("descripcion")]
        [StringLength(250)]
        public string Descripcion { get; set; }

        [ForeignKey(nameof(IdCita))]
        [InverseProperty(nameof(CIta.Diagnosticos))]
        public virtual CIta IdCitaNavigation { get; set; }
        [InverseProperty(nameof(Models.Receta.IdDiagnosticoNavigation))]
        public virtual ICollection<Receta> Receta { get; set; }
    }
}
