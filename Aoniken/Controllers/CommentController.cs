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
            //EJECUTO CON DAPPER UTILIZANDO EL MODELO
            var insert = db.Execute(sql, comment);

            //RETORNO SUCCESS PARA EL FRONT
            return new
            {
                success = true,
                message = "el comentario se creo con éxito",
                result = ""
            };
        }

        //ENDPOINT PARA POST Y COMENTARIOS
        [HttpGet]
        [Route("list_comments")]
        public dynamic getComments()
        {
            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO UNA CONSULTA UTILIZANDO PARAMETROS
            var sql = @"select c.content, p.id, u.nombre from comment c inner join post p on c.post_id = p.id left join user u on c.user_id = u.id where p.pending_approval = 2";
            //RETORNO CON DAPPER UTILIZANDO PARAMETROS
            return db.Query(sql);
       
        }
    }
}
