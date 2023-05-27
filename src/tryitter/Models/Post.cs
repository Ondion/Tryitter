using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tryitter.Models;
public class Post
{
    [Key]
    public int PostId { get; set; }
    [MaxLength(300, ErrorMessage = "Content precisa ter no maximo 300 caracteres")]
    public string Content { get; set; } = default!;
    public string? Image { get; set; }
    [ForeignKey("StudentId")]
    public int StudentId { get; set; }
    public virtual Student? Student { get; set; }

}