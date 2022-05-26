using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    public partial class DetallesReceta
    {
        [Key]
        [Column("idDetalleReceta")]
        public int IdDetalleReceta { get; set; }
        [Key]
        [Column("idReceta")]
        public int IdReceta { get; set; }

        [Required]
        [Column("medicamento")]
        [StringLength(500)]
        public string Medicamento { get; set; }

        [Required(ErrorMessage = "Las indicaciones del medicamento no pueden estar vacias")]
        [Display(Name = "Indicaciones Medicamento")]
        [Column("indicaciones")]
        [StringLength(200)]
        public string Indicaciones { get; set; }

        [ForeignKey(nameof(IdReceta))]
        [InverseProperty(nameof(Receta.DetallesReceta))]
        public virtual Receta IdRecetaNavigation { get; set; }
    }
}
