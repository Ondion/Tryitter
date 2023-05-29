using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tryitter.Models;
public class PostResponse
{
  public int PostId { get; set; }
  public string Content { get; set; } = default!;
  public DateTime? CreatAt { get; set; }
  public DateTime UpdatetAt { get; set; }
  public string? Image { get; set; }
  [ForeignKey("StudentId")]
  public int? StudentId { get; set; }

}