using System.Security.Claims;

namespace Aoniken.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int role { get; set; }

        public  List<User> DB()
        {
            var list = new List<User>()
            {
                new User
                {
                    id = 1,
                    name = "Pablo",
                    password = "sarasa09",
                    role = 0
                },
                 new User
                {
                    id = 2,
                    name = "German",
                    password = "sarasa09",
                    role = 1
                },
                  new User
                {
                    id = 3,
                    name = "Jose",
                    password = "sarasa09",
                    role = 1
                },
                   new User
                {
                    id = 4,
                    name = "Martin",
                    password = "sarasa09",
                    role = 2
                },
                    new User
                {
                    id = 5,
                    name = "Andrea",
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