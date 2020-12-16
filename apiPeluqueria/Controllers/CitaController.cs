using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiPeluqueria.Context;
using apiPeluqueria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiPeluqueria.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CitaController : Controller
    {
        private readonly AppDbContext _context;

        public CitaController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/cita
        [HttpGet]
        public IEnumerable<Cita> Get()
        {
            return _context.citas;
        }

        // GET api/cita/idUsuario
        [HttpGet("{idUsuario}")]
        public ActionResult Get(string idUsuario)
        {
            
            var cita = _context.citas.Where(c => c.IdUsuario.Contains(idUsuario));    
            if (cita != null)
            {
                return Ok(cita);
            } else
            {
            return BadRequest(new JObject()
                {
                    {"StatusCode", 400 },
                    {"Message", "El usuario no tienes citas." }
                });
            }                

        }
        // POST api/cita/disp
        [Route("disp")]
        [HttpPost]
        public ActionResult Disponible([FromBody] Cita cita)
        {

            var result = _context.citas.Where(c => c.Fecha == cita.Fecha && c.Hora == cita.Hora).SingleOrDefault();

            if (result != null)
            {
                return BadRequest(new JObject()
                {
                    {"StatusCode", 400 },
                    {"Message", "Esa cita ya está reservada, lo siento." }
                });
            }
            else
            {
                var result2 = _context.citas.Where(c => c.Fecha == cita.Fecha && c.Hora != cita.Hora);
                if (result2 != null)
                {
                    return Ok(cita);
                }
                else
                {
                    return BadRequest(new JObject()
                {
                    {"StatusCode", 400 },
                    {"Message", "Esa cita ya está reservada, lo siento." }
                });
                }

            }
        }

        // POST api/cita/create
        [Route("create")]
        [HttpPost]
        public ActionResult Register([FromBody] Cita cita)
        {
            var result = _context.citas.Where(c => c.Fecha == cita.Fecha && c.Hora == cita.Hora).SingleOrDefault();
            
            if (result != null)
            {
                return BadRequest(new JObject()
                {
                    {"StatusCode", 400 },
                    {"Message", "Esa cita ya está reservada, lo siento." }
                });
            } else
            {
                var result2 = _context.citas.Where(c => c.Fecha == cita.Fecha && c.Hora != cita.Hora);
                if (result2 != null)
                {
                    _context.citas.Add(cita);
                    _context.SaveChanges();
                    return Ok(cita);
                } else
                {
                    return BadRequest(new JObject()
                    {
                        {"StatusCode", 400 },
                        {"Message", "Esa cita ya está reservada, lo siento." }
                    });
                }
                
            }        
            
        }
    }
}
