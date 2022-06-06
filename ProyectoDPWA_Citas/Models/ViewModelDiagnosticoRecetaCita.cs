using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDPWA_Citas.Models
{
    public class ViewModelDiagnosticoRecetaCita
    {
        public Paciente Paciente { get; set; }
        public Diagnostico diagnostico { get; set; }
        public Receta receta { get; set; }
        public DetallesReceta detallesReceta { get; set; }

        public List<Diagnostico> diagnosticos { get; set; }
        public List<Receta> recetas { get; set; }
        public List<DetallesReceta> detallesRecetas { get; set; }
        public List<ViewModelDiagnosticoReceta> modelDiagnosticoRecetas { get; set; }


    }
}
