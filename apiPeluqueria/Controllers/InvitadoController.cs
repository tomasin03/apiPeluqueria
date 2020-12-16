using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiPeluqueria.Context;
using apiPeluqueria.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiPeluqueria.Controllers
{
    [Produces("application/json")]
    [Route("api/login/[controller]")]
    public class InvitadoController : Controller
    {
        private readonly AppDbContext context;

        public InvitadoController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/<invitado>
        [HttpGet]
        public IEnumerable<Invitado> Get()
        {
            return this.context.invitados;
        }

        // GET api/<invitado>/5
        [HttpGet("{id}", Name = "GetInvitado")]
        public ActionResult Get(int id)
        {
            try
            {
                var invitado = context.invitados.FirstOrDefault(i => i.IdInvitado == id);
                return Ok(invitado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<invitado>
        [HttpPost]
        public ActionResult Post([FromBody] Invitado invitado)
        {
            try
            {
                context.invitados.Add(invitado);
                context.SaveChanges();
                return CreatedAtRoute("GetInvitado", new { id = invitado.IdInvitado }, invitado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<invitado>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Invitado invitado)
        {
            try
            {
                if (invitado.IdInvitado == id)
                {
                    context.Entry(invitado).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetInvitado", new { id = invitado.IdInvitado }, invitado);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<invitado>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var invitado = context.invitados.FirstOrDefault(i => i.IdInvitado == id);
                if (invitado != null)
                {
                    context.invitados.Remove(invitado);
                    context.SaveChanges();
                    return Ok(id);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
