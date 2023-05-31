using System.ComponentModel.DataAnnotations;

namespace tryitter.Models;
public class Student
{
  [Key]
  public int StudentId { get; set; }
  public string Name { get; set; } = default!;
  public string Email { get; set; } = default!;
  public string Password { get; set; } = default!;
  public string Status { get; set; } = default!;
  public virtual ICollection<Post>? Posts { get; set; }
}