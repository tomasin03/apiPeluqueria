using apiPeluqueria.Context;
using apiPeluqueria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace apiPeluqueria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase

    {
        private readonly AppDbContext _config;
        private readonly IConfiguration configuration;

        public LoginController(AppDbContext config, IConfiguration configuration)
        {
            _config = config;
            this.configuration = configuration;
        }

        // POST: api/login/create
        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody] Usuario usuario)
        {
            IActionResult response = Unauthorized();
            var result = _config.usuarios.Where(u => u.NomUsuario == usuario.NomUsuario).SingleOrDefault();
            if (result != null)
            {
                return BadRequest(new JObject()
                {
                    {"StatusCode", 400 },
                    {"Message", "El usuario ya existe." }
                });
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("");
                }
                else
                {
                    var expiration = DateTime.UtcNow.AddDays(7);
                    _config.usuarios.Add(usuario);
                    _config.SaveChanges();
                    CreatedAtRoute("GetUsuario", new { nombre = usuario.NomUsuario }, usuario);
                    var tokenString = GenerarTokenJWT(usuario);
                    response = Ok(new
                    {
                        token = tokenString,
                        expiration = expiration,
                        userDetails = usuario,
                    });

                    return response;
                }
            }          

        }

        // POST: api/Login
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] Usuario usuarioLogin)
        {
            IActionResult response = Unauthorized();
            var expiration = DateTime.UtcNow.AddDays(7);

            Usuario user = AuthenticateUser(usuarioLogin);

            if (user != null)
            {
                var tokenString = GenerarTokenJWT(user);
                response = Ok(new
                {
                    token = tokenString, 
                    expiration = expiration,
                    userDetails = user,
                });
            }           
            
            return response;
        }
        

        Usuario AuthenticateUser(Usuario loginCredentials)
        {
            Usuario user = _config.usuarios.SingleOrDefault(x => x.NomUsuario == loginCredentials.NomUsuario && x.Password == loginCredentials.Password);
            return user;
        }


        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(Usuario usuarioInfo)
        {
            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JWT:ClaveSecreta"])
                );
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {              
                new Claim("NomUsuario", usuarioInfo.NomUsuario),
                new Claim("Nombre", usuarioInfo.Nombre),
                new Claim("Apellido", usuarioInfo.Apellido),
                new Claim("Email", usuarioInfo.Email)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    // Exipra a la 7 dias.
                    expires: DateTime.UtcNow.AddDays(7)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}
