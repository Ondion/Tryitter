using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository
{
    public class StudentRepository
    {
        private readonly TryitterContext _context;
        public StudentRepository(TryitterContext context)
        {
            _context = context;
        }

    }
}