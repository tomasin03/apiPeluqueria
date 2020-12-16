using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiPeluqueria.Models
{
    public class Cita
    {
        [Key]
        public int IdCita { get; set; }
        public string Fecha { get; set; }
        public int Hora { get; set; }
        public string Dia { get; set; }
        public string IdUsuario { get; set; }
        public int IdInvitado { get; set; }
    }
}
