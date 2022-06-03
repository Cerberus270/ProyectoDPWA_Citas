using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ProyectoDPWA_Citas.Models.Metadata;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    [MetadataType(typeof(CIta_Metadata))]
    public partial class CIta
    {
        public String FechaShort
        {
            get
            {
                return Fecha.ToShortDateString();
            }
        }

        public String HoraShort
        {
            get
            {
                return Hora.ToString(@"hh\:mm");
            }
        }

        public Diagnostico DiagnosticoCurrent
        {
            get
            {
                if(Diagnosticos.Count > 0)
                {
                    return Diagnosticos.FirstOrDefault();
                }
                else
                {
                    var diag = new Diagnostico();
                    Diagnosticos.Add(diag);
                    return diag;
                }
            }
        }
    }
}
