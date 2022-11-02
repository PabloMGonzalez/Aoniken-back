namespace Aoniken.Models
{
    public class Post
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime approval_date { get; set; }
        public int user_id { get; set; }
        public bool pending_approval { get; set; }
    }
}