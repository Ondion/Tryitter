using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
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
        public Student AddStudent(Student studentInput)
        {
            var student = new Student
            {
                Name = studentInput.Name,
                Email = studentInput.Email,
                Status = studentInput.Status,
                Password = new Hash(SHA512.Create()).CriptografarSenha(studentInput.Password)
            };
            _context.Students.Add(student);
            _context.SaveChanges();
            return student;
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

        public List<Student> GetAllStudents()
        {
            var students = _context.Students.ToList();
            return students;
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
    }
}