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
    [Route("post")]
    public class PostController : ControllerBase
    {
        // referencio a la db
        #region 
        private readonly MySQLConfiguration _connectionString;

        public PostController(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        #endregion



        [HttpGet]
        [Route("list_approved_posts")]
        public dynamic listPostsApproved()
        {
            var db = dbConnection();
            var sql = @"SELECT p.id, p.title, p.content, u.nombre FROM post p INNER JOIN user u WHERE u.id  = p.user_id AND p.pending_approval = 2;";
            //retorno con Dapper
            return db.Query(sql);
        }

        [HttpGet]
        [Authorize]
        [Route("list_pending_approval_posts")]
        public dynamic listPostsPendingApproval()
        {
            //autentico con el token y miro el rol
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            // me fijo q el role sea 0 asi puedo listar posts para aprobar
            if (usuario.role == 2)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos paara listar posts",
                    result = ""
                };
            }
            else
            {
                var db = dbConnection();
                var sql = @"select p.id, p.title, p.content, p.submit_date, u.nombre from post p, user u where p.user_id = u.id and p.pending_approval = 0;";

                //retorno con Dapper
                return db.Query(sql);
            }
        }

        [HttpGet]
        [Route("list_unapproved_posts")]
        [Authorize]
        public dynamic ListUnapprovedPosts()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            if (usuario.role == 1)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos paara listar posts",
                    result = ""
                };
            }
            else
            {
                var db = dbConnection();
                var sql = @"select id, title, content, submit_date from post where pending_approval = 0 and user_id =" + usuario.id;

                //retorno con Dapper
                return db.Query(sql);
            }
        }

        [HttpPost]
        [Route("create_post")]
        [Authorize]
        public dynamic savePost([FromBody] Object optData)
        {
            //autentico con el token y miro el rol
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());


            string title = data.title;
            string content = data.content;
            var submit_date = DateTime.Today.ToString("yyyy-MM-dd");
            int user_id = data.user_id;

            var db = dbConnection();
            var sql = @"INSERT INTO post(title, content, submit_date, pending_approval, approve_date, user_id)
                        VALUES('" + title + "', '" + content + "', '" + submit_date + "', 0, NULL, " + user_id + ")";
            var insert = db.Execute(sql);

            return new
            {
                success = true,
                message = "el post se creo con exito",
                result = ""
            };
        }



        [HttpPost]
        [Route("edit_post")]
        [Authorize]
        public dynamic editPost([FromBody] Object optData)
        {
            // validacion para editar posts
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            // me fijo q el role sea 0 asi puedo editar posts
            if (usuario.role == 1)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos paara editar posts",
                    result = ""
                };
            }
            else
            {
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

                int id = data.id;
                string title = "'" + data.title + "'";
                string content = "'" + data.content + "'";

                var submit_date = DateTime.Today.ToString("yyyy-MM-dd");
                submit_date = "'" + submit_date + "'";

                var db = dbConnection();
                var sql = @"UPDATE post SET title=" + title + ", content=" + content + ", submit_date=" + submit_date + ",pending_approval=0 WHERE id=" + id;


                //a dapper no le gusta q sean var tienen q ser string o int si o si para q te tome la consulta
                var update = db.QueryFirstOrDefaultAsync(sql);

                return new
                {
                    success = true,
                    message = "Post editado con exito",
                    result = ""
                };
            }

        }

        [HttpPost]
        [Route("delete_post")]
        //token valido
        [Authorize]
        public dynamic DeletePost([FromBody] Object optData)
        {
            // validacion para eliminar posts
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            // me fijo q el role sea 0 asi puedo eliminar posts
            if (usuario.role == 1)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos paara eliminar posts",
                    result = ""
                };
            }
            else
            {
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

                int id = data.id;
                // hago un select de la tabla para ver si existe el post realmente, si existe lo elimino, sino mando un msj
                var db = dbConnection();
                var sql = @" SELECT id FROM Post WHERE id =" + id;
                var select = db.QueryFirstOrDefaultAsync(sql);

                if (select.Result != null)

                {
                    var sqlDelete = @" DELETE FROM Post WHERE id =" + id;
                    var delete = db.QueryFirstOrDefaultAsync(sqlDelete);

                    return new
                    {
                        success = true,
                        message = "se elimino el post con exito",
                        result = select
                    };
                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "no existe el post seleccionado",
                        result = ""
                    };

                }
            }
        }

        [HttpPost]
        [Route("approve_post")]
        [Authorize]
        public dynamic approvePost([FromBody] Object optData)
        {

            //autentico con el token y miro el rol
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            // me fijo q el role sea 0 asi puedo listar posts
            if (usuario.role == 2)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos para aprobar posts",
                    result = ""
                };
            }
            else
            {
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

                int id = data.id;
                var db = dbConnection();
                var sql = @" SELECT id FROM post WHERE id =" + id;
                var select = db.QueryFirstOrDefaultAsync(sql);
                if (select.Result != null)
                {
                    //parseo el datetime en algo q pueda guardar en la DB
                    var date = DateTime.Today.ToString("yyyy-MM-dd");
                    date = "'" + date + "'";

                    var sqlUpdate = @"UPDATE post SET pending_approval = 2, approve_date = " + date + " WHERE id=" + id;
                    var update = db.QueryFirstOrDefaultAsync(sqlUpdate);

                    return new
                    {
                        success = true,
                        message = "se aprobo el post con exito",
                        result = select
                    };
                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "no hay existe el post para aprobar",
                        result = ""
                    };

                }
            }
        }

        [HttpPost]
        [Route("reject_post")]
        [Authorize]
        public dynamic rejectPost([FromBody] Object optData)
        {
            //autentico con el token y miro el rol
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            // me fijo q el role sea 0 asi puedo listar posts
            if (usuario.role == 2)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos para aprobar posts",
                    result = ""
                };
            }
            else
            {
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

                int id = data.id;
                // hago un select de la tabla para ver si existe el post realmente, si existe lo elimino, sino mando un msj
                var db = dbConnection();
                var sql = @" SELECT id FROM post WHERE id =" + id;
                var select = db.QueryFirstOrDefaultAsync(sql);
                if (select.Result != null)
                {
                    //parseo el datetime en algo q pueda guardar en la DB
                    var date = DateTime.Today.ToString("yyyy-MM-dd");
                    date = "'" + date + "'";

                    var sqlUpdate = @"UPDATE post SET pending_approval = 1, approve_date = " + date + " WHERE id=" + id;
                    var update = db.QueryFirstOrDefaultAsync(sqlUpdate);

                    return new
                    {
                        success = true,
                        message = "se rechazo el post ahora se puede editar",
                        result = select
                    };
                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "no hay existe el post para rechazar",
                        result = ""
                    };

                }
            }
        }

        [HttpPost]
        [Route("select_post")]
        [Authorize]
        public dynamic selectPost([FromBody] Object optData)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;


            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            int id = data.id;
            var db = dbConnection();
            var sql = @"select content from comment c inner join post p where c.post_id  = " + id;

            //retorno con Dapper
            return db.Query(sql);
        }


        
    }
}