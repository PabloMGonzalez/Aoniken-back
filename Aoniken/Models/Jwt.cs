using System.Net.Mail;
using System.Security.Claims;

namespace Aoniken.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }


        // verificar el token

        public static dynamic validateToken(ClaimsIdentity identity)

        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Verificar token valido",
                        result = ""
                    };
                }

                var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;


                string mail = identity.Claims.FirstOrDefault(
                             c => c.Type == ClaimTypes.Email)?.Value;
             

                string role = identity.Claims.FirstOrDefault(
                        c => c.Type == ClaimTypes.Role)?.Value;
             

                // recupero el usuario por el id para identificarlo
                User usuario = new User();
                usuario.id = int.Parse(id);
                usuario.email = mail;
                usuario.role = int.Parse(role);


                return new
                {
                    success = true,
                    message = "exito",
                    result = usuario
                };
            }
            catch (Exception error)
            {

                return new
                {
                    success = false,
                    message = "Catch: " + error.Message,
                    result = ""
                };
            }
        }
    }
}
