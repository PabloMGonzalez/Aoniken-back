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

        //ENDPOINT PARA LISTAR 
        [HttpPost]
        [Route("list_comments")]
        public dynamic getComments([FromBody] Object optData)
        {
            //DESERIALIZO EL OBJETO A UN JSON
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO UNA CONSULTA UTILIZANDO PARAMETROS
            var sql = @"select * from post p 
                        left join comment c 
                        on p.id = c.post_id";
            //RETORNO CON DAPPER UTILIZANDO PARAMETROS


            var diccionarioPost = new Dictionary<int, Post>();

            //ARMO UN LISTADO DE POSTS Y SUS COMMENTS PARA DEVOLVER AL FRONT
            var listado = db.Query<Post, Comment, Post>(sql, (post, comment) =>
            {
                Post postTemp;

                if (!diccionarioPost.TryGetValue(post.id, out postTemp))
                {
                    postTemp = post;
                    postTemp.Comments = new List<Comment>();
                    diccionarioPost.Add(postTemp.id, post);
                }

                if (comment != null)
                {
                    postTemp.Comments.Add(comment);
                }
                return postTemp;
            }).Distinct().ToList();

            return listado;
        }
    }
}
