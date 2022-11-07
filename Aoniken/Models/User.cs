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

        public static implicit operator User(ClaimsPrincipal v)
        {
            throw new NotImplementedException();
        }
    }
}