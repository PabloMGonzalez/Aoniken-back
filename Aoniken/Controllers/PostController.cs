
using Aoniken.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;

namespace Aoniken.Controllers
{

    [ApiController]
    [Route("post")]
    public class PostController : ControllerBase
    {
        [HttpGet]
        [Route("listar")]
        public dynamic listarPosts()
        {
            //List<Post> posts = new List<Post>
            //{
            //    new Post
            //    {
            //        id = 1,
            //        title = "hola mundo",
            //    }
            //};
            return "gg wp";
        }


        [HttpPost]
        [Route("guardar")]
        public dynamic guardarPost(Post post)
        {
            post.id = 3 ;
                return new
                {
                    succes = true,
                    message = "cliente registrado",
                    result = post
                };
        }
    }
}