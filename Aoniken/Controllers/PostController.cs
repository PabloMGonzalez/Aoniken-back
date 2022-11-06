
using Aoniken.conn;
using Aoniken.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using MySql.Data.MySqlClient;

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
        public dynamic listarPosts()
        {
            // me conecto a la db y hago el select
            var db = dbConnection();
            var sql = @" SELECT * FROM Post";

            //retorno con Dapper
            return db.QueryAsync(sql);
        }


        [HttpPost]
        [Route("create_post")]
        public dynamic savePost()
        {
            return "aca guardo los posts pa";
        }

        [HttpPost]
        [Route("edit_post")]
        public dynamic editPost()
        {
            return "aca edito los posts pa";
        }

    }
}