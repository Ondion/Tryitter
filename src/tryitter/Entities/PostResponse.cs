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

  public PostResponse(int postId, string content, DateTime creatAt, DateTime updatetAt, string image, int studentId)
  {
    PostId = postId;
    Content = content;
    CreatAt = creatAt;
    UpdatetAt = updatetAt;
    Image = image;
    StudentId = studentId;
  }

}