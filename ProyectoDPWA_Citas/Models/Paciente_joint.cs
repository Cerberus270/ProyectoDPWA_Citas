using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDPWA_Citas.Models.Metadata;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    [ModelMetadataType(typeof(Paciente_Metadata))]
    public partial class Paciente
    {

    }
}
