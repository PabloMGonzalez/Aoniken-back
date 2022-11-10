﻿using Aoniken.conn;
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
        // referencio a la db
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

        [HttpPost]
        [Route("create_comment")]
        [Authorize]

        public dynamic saveComment([FromBody] Object optData)
        {
            //autentico con el token y miro el rol
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validateToken(identity);
            if (!rToken.success)
                return rToken;
            User usuario = rToken.result;

            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            int user_id = data.user_id;
            int post_id = data.post_id;

            string content = data.content;

            var submit_date = DateTime.Today.ToString("yyyy-MM-dd");

            var db = dbConnection();
            var sql = @"INSERT INTO comment (content, user_id, post_id) VALUES('"+content+"', "+user_id+", "+post_id+")";
            var insert = db.Execute(sql);

            return new
            {
                success = true,
                message = "el comentario se creo con éxito",
                result = ""
            };
        }

        [HttpPost]
        [Route("get_comments")]
        public dynamic getComments([FromBody] Object optData)
        {

            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            int id = data.id;
            var db = dbConnection();
            var sql = @"select content from comment where post_id =" + id;

            //retorno con Dapper
            return db.Query(sql);
        }

    }
}
