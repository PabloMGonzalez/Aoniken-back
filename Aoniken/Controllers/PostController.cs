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
        //REFERENCO A LA DB PARA PODER USARLA
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


        //ENDPOINT HOME LISTA LOS POST APROBADOS
        [HttpGet]
        [Route("list_approved_posts")]
        public dynamic listPostsApproved()
        {
            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO LA CONSULTA
            var sql = @"SELECT p.id, p.title, p.content, u.nombre FROM post p INNER JOIN user u WHERE u.id = p.user_id AND p.pending_approval = 2;";
            //RETORNO CON DAPPER
            return db.Query(sql);
        }


        //ENDPOINT CON AUTORIZACION LISTA POST PENDIENTES DE APROBAR, SOLO LO VE EL EDITOR
        [HttpGet]
        [Authorize]
        [Route("list_pending_approval_posts")]
        public dynamic listPostsPendingApproval()
        {
            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //MIRO SI EL ROL ES DE EDITOR (1) PARA POODER SEGUIR
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
                //HAGO LA CONEXION A LA DB
                var db = dbConnection();
                //HAGO LA CONSULTA
                var sql = @"select p.id, p.title, p.content, p.submit_date, u.nombre from post p, user u where p.user_id = u.id and p.pending_approval = 0;";
                //RETORNO CON DAPPER
                return db.Query(sql);
            }
        }


        //ENDPOINT CON AUTORIZACION LISTA POST QUE NO ESTAN APROBADOS SOLO EL WRITER (2) PUEDE VERLOS 
        [HttpGet]
        [Route("list_unapproved_posts")]
        [Authorize]
        public dynamic ListUnapprovedPosts()
        {
            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //MIRO SI EL ROL ES DE WRITER (2) PARA POODER SEGUIR
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
                //HAGO LA CONEXION A LA DB
                var db = dbConnection();
                //HAGO LA CONSULTA UTILIZANDO PARAMETROS
                var sql = @"select id, title, content, submit_date from post where pending_approval = 0 and user_id = @id";
                //RETORNO CON DAPPER UTILIZANDO PARAMETROS
                return db.Query(sql, new { id = usuario.id });
            }
        }


        //ENDPOINT CON AUTORIZACION PARA CREAR POST
        [HttpPost]
        [Route("create_post")]
        [Authorize]
        //RECIBO INFORMACION DESDE EL FRONT EN FORMA DE OBJETO POST
        public dynamic savePost(Post post)
        {
            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //ASIGNO A SUBMIT DATE EL DIA DE HOY
            post.submit_date = DateTime.Today;
            //ASIGNO A APPROVE_DATE NULL
            post.approve_date = null;

            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO UN INSERT A LA DB UTILIZANDO PARAMETROS
            var sql = @"INSERT INTO post(title, content, submit_date, pending_approval, approve_date, user_id)
                        VALUES(@title, @content, @submit_date, @pending_approval, @approve_date, @user_id)";
            //EJECUTO CON DAPPER UTILIZANDO EL MODELO POST
            var insert = db.Execute(sql, post);

            //RETORNO SUCCES PARA EL FRONT
            return new
            {
                success = true,
                message = "el post se creo con exito",
                result = ""
            };
        }


        //ENDPOINT CON AUTORIZACION PARA EDITAR POSTS
        [HttpPost]
        [Route("edit_post")]
        [Authorize]
        public dynamic editPost(Post post)
        {
            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //MIRO SI EL ROL NO ES DE WRITER (2) PARA POODER SEGUIR
            if (usuario.role == 1)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos para editar posts",
                    result = ""
                };
            }
            else
            {
                //ASIGNO A SUBMIT DATE EL DIA DE HOY
                post.submit_date = DateTime.Today;

                //HAGO LA CONEXION A LA DB
                var db = dbConnection();
                //HAGO UN UPDATE A LA DB UTILIZANDO PARAMETROS
                var sql = @"UPDATE post SET title=@title, content=@content, submit_date=@submit_date,pending_approval=0 WHERE id=@id";
                //EJECUTO CON DAPPER UTILIZANDO EL MODELO POST
                var update = db.Execute(sql, post);

                //RETORNO SUCCES PARA EL FRONT
                return new
                {
                    success = true,
                    message = "Post editado con exito",
                    result = ""
                };
            }

        }

        //ENDPOINT CON AUTORIZACION PARA ELIMINAR POSTS
        [HttpPost]
        [Route("delete_post")]
        [Authorize]
        public dynamic DeletePost([FromBody] Object optData)
        {
            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //MIRO SI EL ROL NO ES DE EDITOR (1) PARA POODER SEGUIR
            if (usuario.role == 2)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos para eliminar posts",
                    result = ""
                };
            }
            else
            {
                //DESERIALIZO EL OBJETO A UN JSON
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

                //HAGO LA CONEXION A LA DB
                var db = dbConnection();
                //HAGO UN SELECT EN LA TABLA PARA VER SI EXISTE EL POST UTILIZANDO PARAMETROS
                var sql = @" SELECT id FROM post WHERE id = @id";
                //RETORNO CON DAPPER UTILIZANDO PARAMETROS
                var select = db.QueryFirstOrDefaultAsync(sql, new { id = data.id });

                //SI EL RESULTADO NO ES NULO PROCEDO A ELIMINAR EL POST
                if (select.Result != null)
                {
                    //HAGO UN DELETE DEL POST
                    var sqlDelete = @" DELETE FROM post WHERE id = @id";
                    //EJECUTO DAPPER UTILIZANDO PARAMETROS
                    var delete = db.Execute(sqlDelete, new { id = data.id });

                    //RETORNO SUCCESS TRUE PARA EL FRONT END
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

        //ENDPOINT CON AUTORIZACION PARA APROBAR POSTS
        [HttpPost]
        [Route("approve_post")]
        [Authorize]
        public dynamic approvePost([FromBody] Object optData)
        {

            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //MIRO SI EL ROL NO ES DE WRITER (2) PARA POODER SEGUIR
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
                //DESERIALIZO EL OBJETO A UN JSON
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
                //HAGO LA CONEXION A LA DB
                var db = dbConnection();
                //HAGO LA CONSULTA UTILIZANDO PARAMETROS
                var sql = @" SELECT id FROM post WHERE id = @id";
                //RETORNO CON DAPPER UTILIZANDO PARAMETROS
                var select = db.QueryFirstOrDefaultAsync(sql, new { id = data.id });

                //SI EXISTE EL REGISTRO PROCEDE A APROBARLO
                if (select.Result != null)
                {
                    //ASIGNO A DATE EL DIA DE HOY PARA PODER USARLO EN APPROVE DATE
                    var date = DateTime.Today.ToString("yyyy-MM-dd");

                    //HAGO UPDATE UTILIZANDO PARAMETROS
                    var sqlUpdate = @"UPDATE post SET pending_approval = 2, approve_date = @approve_date WHERE id= @id";
                    //RETORNO CON DAPPER UTILIZANDO PARAMETROS
                    var update = db.QueryFirstOrDefaultAsync(sqlUpdate, new { approve_date = date, id = data.id });

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


        //ENDPOINT CON AUTORIZACION PARA RECHAZAR POSTS
        [HttpPost]
        [Route("reject_post")]
        [Authorize]
        public dynamic rejectPost([FromBody] Object optData)
        {
            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //MIRO SI EL ROL NO ES DE WRITER (2) PARA POODER SEGUIR
            if (usuario.role == 2)
            {
                return new
                {
                    success = false,
                    message = "No tiene permisos para rechazar posts",
                    result = ""
                };
            }
            else
            {
                //DESERIALIZO EL OBJETO A UN JSON
                var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
                //HAGO LA CONEXION A LA DB
                var db = dbConnection();
                //HAGO LA CONSULTA UTILIZANDO PARAMETROS
                var sql = @" SELECT id FROM post WHERE id = @id";
                //RETORNO CON DAPPER UTILIZANDO PARAMETROS
                var select = db.QueryFirstOrDefaultAsync(sql, new { id = data.id });

                //SI EXISTE EL REGISTRO PROCEDE A RECHAZARLO
                if (select.Result != null)
                {
                    //HAGO UPDATE UTILIZANDO PARAMETROS
                    var sqlUpdate = @"UPDATE post SET pending_approval = 1 WHERE id= @id";
                    //RETORNO CON DAPPER UTILIZANDO PARAMETROS
                    var update = db.QueryFirstOrDefaultAsync(sqlUpdate, new { id = data.id });

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


        //ENDPOINT CON AUTORIZACION PARA SELECCIONAR UN POST 
        [HttpPost]
        [Route("select_post")]
        [Authorize]
        //RECIBO INFORMACION DESDE EL FRONT EN FORMA DE OBJETO
        public dynamic selectPost([FromBody] Object optData)
        {
            //ME AUTENTICO CON EL TOKEN Y LE ASIGNO EL TOKEN AL USUARIO
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            //DESERIALIZO EL OBJETO A UN JSON
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());
            //HAGO LA CONEXION A LA DB
            var db = dbConnection();
            //HAGO LA CONSULTA UTILIZANDO PARAMETROS
            var sql = @"select title, content from post where id  =  @id";
            //RETORNO CON DAPPER UTILIZANDO PARAMETROS Y LA ID OBTENIDA DEL JSON
            return db.Query(sql, new { id = data.id });
        }
    }
}