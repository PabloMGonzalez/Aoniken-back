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
        //REFERENCIO A LA DB PARA PODER USARLA
        private readonly MySQLConfiguration _connectionString;
        public IConfiguration _configuration;
        public UserController(IConfiguration configuration, MySQLConfiguration connectionString)
        {
            _configuration = configuration;
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }

        //ENDPOINT LOGIN
        [HttpPost]
        [Route("login")]
        public dynamic Login(User user)
        {
            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO LA CONSULTA UTILIZANDO PARAMETROS
            var sql = @"SELECT id,email,role FROM user WHERE email = @email AND password = @password";
            //RETORNO CON DAPPER UTILIZANDO PARAMETROS
            var usuario = db.QueryFirstOrDefaultAsync(sql, user);

            if (usuario.Result == null)
            {
                return new
                {
                    succes = false,
                    message = "Credenciales incorrectas",
                    result = ""
                };
            }

            //OBTENGO LOS DATOS DEL APPSETTING Y DECLARO EN UNA VARIABLE
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            //DEFINO LOS CLAIMS PARA LOS JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("id", usuario.Result.id.ToString()),
                new Claim("email", usuario.Result.email.ToString()),
                new Claim("role", usuario.Result.role.ToString()),
            };

            //RECUPERO LA KEY DEL JWT Y LA DEFINO
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            //ENCRIPTO LA JWT
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256.ToString());

            //ARMO EL TOKEN USANDO LOS CLAIMS EL ISSUER LA AUDIENCE Y LA EXPIRACION LUEGO LO RECUPERO DESDE EL POST
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
                );

            return new
            {
                success = true,
                message = "exito",
                id = usuario.Result.id,
                role = usuario.Result.role,
                //MANDO EL RESULTADO CON EL TOKEN VALIDO
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        //ENDPOINT REGISTER
        [HttpPost]
        [Route("register")]
        public dynamic Register([FromBody] Object optData)
        {
            //DESERIALIZO EL OBJETO A UN JSON
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO UN INSERT UTILIZANDO PARAMETROS
            var sql = @"INSERT INTO user (nombre, email, password, `role`) VALUES(@nombre, @email, @password, 2)";
            //EJECUTO DAPPER UTILIZANDO PARAMETROS
            var insert = db.Execute(sql, new { nombre = data.nombre, email = data.email, password = data.password, role = data.role });

            return new
            {
                success = true,
                message = "el usuario se creo con exito",
                result = ""
            };
        }

    }
}