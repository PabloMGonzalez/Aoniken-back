using Aoniken.conn;
using Aoniken.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Aoniken.Controllers
{
    [ApiController]
    [Route("comment")]
    public class CommentController : ControllerBase
    {
        //REFERENCIO A LA DB PARA PODER USARLA
        #region 
        private readonly MySQLConfiguration _connectionString;
        public CommentController(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        #endregion


        //ENDPOINT PARA CREAR COMENTARIOS
        [HttpPost]
        [Route("create_comment")]

        public dynamic saveComment(Comment comment)
        {
            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO UN INSERT A LA DB UTILIZANDO PARAMETROS
            var sql = @"INSERT INTO comment (content, user_id, post_id) 
                        VALUES(@content, @user_id, @post_id)";
            //EJECUTO CON DAPPER UTILIZANDO EL MODELO POST
            var insert = db.Execute(sql, comment);

            //RETORNO SUCCESS PARA EL FRONT
            return new
            {
                success = true,
                message = "el comentario se creo con éxito",
                result = ""
            };
        }

        [HttpPost]
        [Route("list_comments")]
        public dynamic getComments([FromBody] Object optData)
        {

            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            int id = data.id;
            var db = dbConnection();
            var sql = @"select c.content, u.nombre from comment c inner join user u where c.user_id = u.id and post_id =" + id;

            //retorno con Dapper
            return db.Query(sql);
        }


    }
}
