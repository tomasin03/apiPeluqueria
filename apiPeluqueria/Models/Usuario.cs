using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiPeluqueria.Models
{
    public class Usuario
    {
        [Key]
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El usuario debe tener al menos 3 caracteres")]
        public string NomUsuario { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "La contraseña deber tener entre 8 y 20 caracteres")]
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
