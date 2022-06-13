using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using ProyectoDPWA_Citas.Models.Metadata;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    [MetadataType(typeof(Receta_Metadata))]
    public partial class Receta
    {
        [ValidateNever]
        public String IdRecetaString
        {
            get
            {
                return IdReceta > 0 ? IdReceta.ToString() : "Será generado";
            }
        }
    }
}
