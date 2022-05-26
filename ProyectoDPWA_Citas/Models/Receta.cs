using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    public partial class Receta
    {
        public Receta()
        {
            DetallesReceta = new HashSet<DetallesReceta>();
        }

        [Key]
        [Column("idReceta")]
        public int IdReceta { get; set; }
        [Column("idDiagnostico")]
        public int IdDiagnostico { get; set; }
        [Column("fechaPrescripcion", TypeName = "date")]
        public DateTime FechaPrescripcion { get; set; }

        [ForeignKey(nameof(IdDiagnostico))]
        [InverseProperty(nameof(Diagnostico.Receta))]
        public virtual Diagnostico IdDiagnosticoNavigation { get; set; }
        [InverseProperty(nameof(Models.DetallesReceta.IdRecetaNavigation))]
        public virtual ICollection<DetallesReceta> DetallesReceta { get; set; }
    }
}
