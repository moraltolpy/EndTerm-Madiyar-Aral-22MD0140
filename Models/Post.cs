namespace EndTerm.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime DateOfCreation { get; set; }
        public string Author { get; set; } = null!;
    }
}
