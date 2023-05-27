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

    }
}