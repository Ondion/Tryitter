using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository
{
    public class PostRepository
    {
        private readonly TryitterContext _context;
        public PostRepository(TryitterContext context)
        {
            _context = context;
        }

    }
}