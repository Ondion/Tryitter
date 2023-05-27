using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tryitter.Models;
public class StudentLogin
{
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
}