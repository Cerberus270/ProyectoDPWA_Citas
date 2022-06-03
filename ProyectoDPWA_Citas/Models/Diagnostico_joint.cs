using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    [ModelMetadataType(typeof(Diagnostico_Metadata))]
    public partial class Diagnostico
    {
        public String IdDiagnosticoString
        {
            get
            {
                return IdDiagnostico > 0 ? IdDiagnostico.ToString() : "Será generado";
            }
        }

        public Receta RecetaCurrent
        {
            get
            {
                if (Receta.Count > 0)
                {
                    return this.Receta.FirstOrDefault();
                }
                else
                {
                    var diag = new Receta();
                    Receta.Add(diag);
                    return diag;
                }
            }
        }
    }
}
