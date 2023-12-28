namespace Guest_book.Models
{
	public class ActiveUserModel
	{
		public User? user { get; set; }
		public IEnumerable<Message>? messages { get; set; }

	}
}
