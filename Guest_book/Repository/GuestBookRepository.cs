using Guest_book.Models;
using Microsoft.EntityFrameworkCore;

namespace Guest_book.Repository
{
    public class GuestBookRepository : IRepository
    {
        private readonly BookGuestContext context;
        public GuestBookRepository(BookGuestContext context)
        {
            this.context = context;
        }

        public async Task<List<Message>> GetListMessages()
        {
            return await context.messages.Include("user").ToListAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await context.messages.FindAsync(id);
        }

        public async Task DeleteMessage(int id)
        {
            Message? m = await context.messages.FindAsync(id);
            if (m != null)
                context.messages.Remove(m);
        }

        public void UpdateMessage(Message message)
        {
            context.Entry(message).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Create(Message message)
        {
            await context.messages.AddAsync(message);
        }

        public async Task<List<User>> GetListUsers()
        {
            return await context.users.ToListAsync();
        }

        public async Task<User> GetUser(User user)
        {
            var tmp = await context.users.Where(u=> u.login == user.login).FirstOrDefaultAsync();
            return tmp;
        }

        public async Task CreateUser(User user)
        {
            await context.users.AddAsync(user);
        }

		public async Task<User> GetUserById(int Id)
		{
            return await context.users.FindAsync(Id);
		}
	}
}
