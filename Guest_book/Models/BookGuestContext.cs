using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Guest_book.Models
{
    public class BookGuestContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Message> messages { get; set; }
        public BookGuestContext(DbContextOptions<BookGuestContext> options) : base(options)
        {
            if (Database.EnsureCreated()) { }
        }
    }
}
