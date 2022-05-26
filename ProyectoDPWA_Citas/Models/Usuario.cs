using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ProyectoDPWA_Citas.Models
{
    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        [Required(ErrorMessage = "El Username no puede estar vacíoss")]
        [Column("username")]
        [StringLength(20)]
        public string Username { get; set; }
        [Required(ErrorMessage = "La contraseña no puede estar vacía")]
        [Column("contraseña")]
        [StringLength(32)]
        public string Contraseña { get; set; }

        [Required]
        [Column("tipoUsuario")]
        [StringLength(50)]
        public string TipoUsuario { get; set; }
    }
}
