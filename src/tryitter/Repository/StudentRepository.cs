using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using tryitter.Entities;
using tryitter.Models;
using tryitter.Services;

namespace tryitter.Repository
{
  public class StudentRepository
  {
    private readonly TryitterContext _context;
    public StudentRepository(TryitterContext context)
    {
      _context = context;
    }
    // Create a new student after check if there isn't a student with same email
    public string AddStudent(Student studentInput)
    {
      Student studentDB = _context.Students.AsNoTracking().Where(c => c.Email == studentInput.Email).FirstOrDefault();
      if (studentDB != null) return "Email already exists";

      var newStudent = new Student
      {
        Name = studentInput.Name,
        Email = studentInput.Email,
        Status = studentInput.Status,
        Password = new Hash(SHA512.Create()).CriptografarSenha(studentInput.Password)
      };
      _context.Students.Add(newStudent);
      _context.SaveChanges();
      return "student created";
    }
    // Login a student and return a token
    public string Login(StudentLogin studentLogin)
    {
      var studentdb = GetStudent(studentLogin.Name);
      if (studentdb.Name == studentLogin.Name && new Hash(SHA512.Create()).VerificarSenha(studentLogin.Password, studentdb.Password))
      {
        return new TokenGenerator().Generate(studentdb);
      }
      return "Student not found";
    }
    //Update a student register after check if the email passed not exist in DB and if student to be update is the same student do the request
    public string UpdateStudent(int id, Student studentInput)
    {
      var currentStateofStudent = _context.Students.AsNoTracking().Where(c => c.StudentId == id).FirstOrDefault();
      if (currentStateofStudent == null) return "Student not found";
      var hasStudantWithThisEmail = _context.Students.AsNoTracking().Where(c => c.Email == studentInput.Email).FirstOrDefault();

      if (currentStateofStudent.Email != studentInput.Email && hasStudantWithThisEmail != null)
      {
        return "Email already exists";
      }

      var student = new Student
      {
        StudentId = id,
        Name = studentInput.Name,
        Email = studentInput.Email,
        Status = studentInput.Status,
        Password = new Hash(SHA512.Create()).CriptografarSenha(studentInput.Password)
      };
      _context.Students.Update(student);
      _context.SaveChanges();
      return "Student updated";
    }
    //Cascate delete - delete student and yours posts
    public string DeleteStudent(Student student)
    {
      var posts = _context.Posts.Where(p => p.StudentId == student.StudentId);
      _context.Students.Remove(student);
      _context.Posts.RemoveRange(posts);
      _context.SaveChanges();
      return "student remove";
    }
    //get the student using the studant name
    public Student GetStudent(string name)
    {
      Student student = _context.Students.FirstOrDefault(x => x.Name == name);
      return student;
    }
    //get the student using the studant id
    public Student GetStudentById(int id)
    {
      Student student = _context.Students.Find(id);
      return student;
    }
    //get all students register
    public List<StudentResponse> GetAllStudents()
    {
      var listStudents = new List<StudentResponse>();
      var students = _context.Students.ToList();

      foreach (Student student in students)
      {
        listStudents.Add(new StudentResponse
        {
          StudentId = student.StudentId,
          Name = student.Name,
          Email = student.Email,
          Status = student.Status
        });
      }
      return listStudents;
    }
  }
}