using Aoniken.conn;
using Aoniken.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NHibernate.Mapping;
using System.Security.Claims;
using Array = System.Array;

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
        [Route("listar_post")]
        [Authorize]
        public dynamic listarPosts()
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
                    message = "No tiene permisos paara listar posts",
                    result = ""
                };
            }
            else
            {
                var db = dbConnection();
                var sql = @"select p.id, p.submit_date, u.email from post p, user u where p.user_id = u.id;";

                //retorno con Dapper
                return db.Query(sql);
            }

        }


        [HttpPost]
        [Route("create_post")]
        public dynamic savePost(Post post)
        {
            return "aca guardo los posts pa";
        }

        [HttpPost]
        [Route("edit_post")]
        public dynamic editPost(Post post)
        {
            return "aca edito los posts pa";
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
            if (usuario.role == 2)
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
                // hago un select de la tabla para ver si existe el post realmente, si existe lo elimino, sino mando un msj
                var db = dbConnection();
                var sql = @" SELECT id FROM Post WHERE id =" + id + " AND pending_approval = 0";
                var select = db.QueryFirstOrDefaultAsync(sql);
                if (select.Result != null)
                {
                    //parseo el datetime en algo q pueda guardar en la DB
                    var date = DateTime.Today.ToString("yyyy-MM-dd");
                    date = "'" + date + "'";

                    var sqlUpdate = @"UPDATE Post SET pending_approval = 2, approve_date = " + date + " WHERE id=" + id;
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
                var sql = @" SELECT id FROM Post WHERE id =" + id + " AND pending_approval = 0";
                var select = db.QueryFirstOrDefaultAsync(sql);
                if (select.Result != null)
                {
                    //parseo el datetime en algo q pueda guardar en la DB
                    var date = DateTime.Today.ToString("yyyy-MM-dd");
                    date = "'" + date + "'";

                    var sqlUpdate = @"UPDATE Post SET pending_approval = 1, approve_date = " + date + " WHERE id=" + id;
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

    }
}