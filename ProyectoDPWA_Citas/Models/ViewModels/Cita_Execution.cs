using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDPWA_Citas.Models.ViewModels
{
    public class Cita_Execution
    {
        public CIta cita { get; set; }
        public Diagnostico diagnostico {get; set;}
        public Receta receta { get; set; }
        public IEnumerable<DetallesReceta> detallesReceta { get; set; }
    }
}
