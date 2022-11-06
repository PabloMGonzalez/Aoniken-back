namespace Aoniken.Models
{
    public class Comment
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int user_post { get; set; }
        public string content { get; set; }

    }
}