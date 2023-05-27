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
            if (studentInput.Name == null || studentInput.Email == null || studentInput.Status == null || studentInput.Password == null)
            {
                throw new Exception("all fields are not filled in");
            }
            else
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

        }
        public Student GetStudent(string name)
        {
            Student student = _context.Students.FirstOrDefault(x => x.Name == name);
            return student;
        }

        public string Login(StudentLogin studentLogin)
        {
            if (studentLogin.Name == null || studentLogin.Password == null)
            {
                throw new Exception("all fields are not filled in");
            }

            var studentdb = GetStudent(studentLogin.Name);
            if (studentdb.Name == studentLogin.Name && new Hash(SHA512.Create()).VerificarSenha(studentLogin.Password, studentdb.Password))
            {
                return new TokenGenerator().Generate(studentdb);
            }
            return "user not found";

        }
    }
}