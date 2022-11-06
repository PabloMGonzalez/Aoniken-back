namespace Aoniken.Models
{
    public class Post
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime approval_date { get; set; }
        public int pending_approval { get; set; }
        //pending_approval 0 = a la espera que se apruebe
        //pending_approval 1 = no se aprobo, se puede volver a editar
        //pending_approval 2 = se aprobo
    }
}