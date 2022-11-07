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

                // recupero el usuario por el id para identificarlo
                User usuario = new User();
                usuario.id = int.Parse(id);
                //usuario = usuario.DB().FirstOrDefault(x => x.id == int.Parse(id));


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
