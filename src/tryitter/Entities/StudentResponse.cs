namespace tryitter.Entities;
public class StudentResponse
{
    public int StudentId { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Status { get; set; } = default!;
}