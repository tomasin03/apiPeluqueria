using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiPeluqueria.Context;
using apiPeluqueria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using ServiceStack.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiPeluqueria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext context;
        public UsuarioController(AppDbContext context)
        {
            this.context = context;
        }

        
        // GET: api/<UsuarioController>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(context.usuarios.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
  
        
        // GET api/<UsuarioController>/5
        [HttpGet("{nombre}", Name = "GetUsuario")]
        public ActionResult Get(string nombre)
        {
            try
            {
                //var usuario = context.usuarios.Find(nombre);
                var usuario = context.usuarios.FirstOrDefault(u => u.NomUsuario == nombre);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        // POST api/<UsuarioController>
        [HttpPost]
        public ActionResult Post([FromBody] Usuario usuario)
        {
            try
            {
                context.usuarios.Add(usuario);
                context.SaveChanges();
                return CreatedAtRoute("GetUsuario", new { nombre = usuario.NomUsuario }, usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{nombre}")]
        public ActionResult Put(string nombre, [FromBody] Usuario usuario)
        {
            try
            {
                if (usuario.NomUsuario == nombre)
                {
                    context.Entry(usuario).State = EntityState.Modified;
                    context.SaveChanges();
                    return CreatedAtRoute("GetUsuario", new { nombre = usuario.NomUsuario }, usuario);
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

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{nombre}")]
        public ActionResult Delete(string nombre)
        {
            try
            {
                var usuario = context.usuarios.FirstOrDefault(u => u.NomUsuario == nombre);
                if (usuario != null)
                {
                    context.usuarios.Remove(usuario);
                    context.SaveChanges();
                    return Ok(nombre);
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
