using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EndTerm.Models
{
    public class Post: PostDto
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
    public class PostDto
    { 
        public string Title { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime DateOfCreation { get; set; }
        public string Author { get; set; } = null!;
    }
}
