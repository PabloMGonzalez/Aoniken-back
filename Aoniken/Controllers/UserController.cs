using Aoniken.conn;
using Aoniken.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
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
        public dynamic Login([FromBody] Object optData)
        {

            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string email = "'" + data.email + "'";
            string password = "'" + data.password + "'";

            // que rol de usuario soy dependiendo el user y el pass
            var db = dbConnection();
            var sql = @"SELECT id,email,role FROM user WHERE email = " + email + "AND password = " + password;

            //retorno con Dapper  
            var usuario = db.QueryFirstOrDefaultAsync(sql);

            string id;
            string mail;
            string role;

            if (usuario.Result == null)
            {
                return new
                {
                    succes = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            }
            else
            {
                id = Convert.ToString(usuario.Result.id);
                mail = Convert.ToString(usuario.Result.email);
                role = Convert.ToString(usuario.Result.role);
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
                new Claim("email", mail),
                new Claim("role", role),
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
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: signIn
                );

            return new
            {
                succes = true,
                message = "exito",
                id = id,
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }


        [HttpPost]
        [Route("register")]
        public dynamic Register([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string email = "'" + data.email + "'";
            string password = "'" + data.password + "'";
            string nombre = "'" + data.nombre + "'";

            var db = dbConnection();
            var sql = @"INSERT INTO user (nombre, email, password, `role`) VALUES(" + nombre + "," + email + ", " + password + ", 2)";
            var insert = db.Execute(sql);

            return new
            {
                success = true,
                message = "el usuario se creo con exito",
                result = ""
            };
        }

    }
}