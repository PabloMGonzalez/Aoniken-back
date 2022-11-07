using Aoniken.conn;
using Aoniken.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Aoniken.Controllers
{

    [ApiController]
    [Route("user")]

    public class UserController : ControllerBase
    {
        private readonly MySQLConfiguration _connectionString;
        public IConfiguration _configuration;

        public UserController(IConfiguration configuration, MySQLConfiguration connectionString)
        {
            _configuration = configuration;
            _connectionString = connectionString;

        }

        // referencio a la db
        #region 

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        #endregion

        [HttpPost]
        [Route("login")]


        public dynamic Login([FromBody] Object optData)       {


            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string email = "'" + data.email + "'";
            string password = "'" + data.password + "'";

            // que rol de usuario soy dependiendo el user y el pass
            var db = dbConnection();
            var sql = @"SELECT id FROM user WHERE email = " + email + "AND password = " + password;

            //retorno con Dapper  
            var usuario = db.QueryFirstOrDefaultAsync(sql);

            string id;

            if (usuario.Result == null)
            {
                return new
                {
                    succes = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            } else
            {
                id = Convert.ToString(usuario.Result.id);

            }

            // obtengo los datos del appsetting y los convierto a una clase
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();


            // defino los claims para el token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("id", id),
                new Claim("email", data.email),
            };

            // recupero la key del jwt y la defino
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            // encripto la jwt key
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256.ToString());

            // armo el token usando los claim el issuer la auidence y la expiracion luego lo recupero desde el post
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(4),
                signingCredentials: signIn
                );

            return new
            {
                succes = true,
                message = "exito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }


        [HttpPost]
        [Route("delete_post")]
        //token valido
        [Authorize]
        public dynamic DeletePost(Post post)
        {
            // validacion para eliminar posts
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken = Jwt.validateToken(identity);


            if (!rToken.success)
                return rToken;

            User usuario = rToken.result;


            if (usuario.role != 0)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos paara eliminar posts",
                    result = ""
                };
            }

            return new
            {
                succes = true,
                message = "post eliminado",
                result = post
            };
        }



        [HttpPost]
        [Route("approve_post")]
        [Authorize]
        public dynamic approvePost()
        {
            return "Hola este es el metodo para aprobar posts";
        }


        [HttpPost]
        [Route("comment_post")]
        [Authorize]
        public dynamic commentPost()
        {
            return "Hola este es el metodo para comentar posts";
        }
    }


}
