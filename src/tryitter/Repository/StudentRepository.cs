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

    public string Login(StudentLogin studentLogin)
    {
      var studentdb = GetStudent(studentLogin.Name);
      if (studentdb.Name == studentLogin.Name && new Hash(SHA512.Create()).VerificarSenha(studentLogin.Password, studentdb.Password))
      {
        return new TokenGenerator().Generate(studentdb);
      }
      return "user not found";
    }

    public string UpdateStudent(int id, Student studentInput)
    {
      var currentStateofStudent = _context.Students.AsNoTracking().Where(c => c.StudentId == id).FirstOrDefault();
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

    public string DeleteStudent(Student student)
    {
      _context.Students.Remove(student);
      _context.SaveChanges();
      return "student remove";
    }
    public Student GetStudent(string name)
    {
      Student student = _context.Students.FirstOrDefault(x => x.Name == name);
      return student;
    }

    public Student GetStudentById(int id)
    {
      Student student = _context.Students.Find(id);
      return student;
    }

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