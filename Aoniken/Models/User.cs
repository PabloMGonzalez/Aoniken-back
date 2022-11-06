using System.Security.Claims;

namespace Aoniken.Models
{
    public class User
    {
        public int id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }
        //role 0 = admin
        //role 1 = editor
        //role 2 = writer
        public List<User> DB()
        {
            var list = new List<User>()
            {
                new User
                {
                    id = 1,
                    email = "Pablo",
                    password = "sarasa09",
                    role = 0
                },
                 new User
                {
                    id = 2,
                    email = "German",
                    password = "sarasa09",
                    role = 1
                },
                  new User
                {
                    id = 3,
                    email = "Jose",
                    password = "sarasa09",
                    role = 1
                },
                   new User
                {
                    id = 4,
                    email = "Martin",
                    password = "sarasa09",
                    role = 2
                },
                    new User
                {
                    id = 5,
                    email = "Andrea",
                    password = "sarasa09",
                    role = 1
                }
            };
            return list;
        }

        public static implicit operator User(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}