using Guest_book.Models;

namespace Guest_book.Repository
{
    public interface IRepository
    {
        Task<List<Message>> GetListMessages();
        Task<Message> GetMessage(int id);
        Task Create(Message message);
        Task DeleteMessage(int id);
        void UpdateMessage(Message message);

        Task<List<User>> GetListUsers();
        Task<User> GetUser(User user);
        Task<User> GetUserById(int Id);
		Task CreateUser(User user);
        Task Save();
    }
}
