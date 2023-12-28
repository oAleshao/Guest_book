using System.ComponentModel.DataAnnotations;

namespace Guest_book.Models
{
    public class Message
    {
        public int Id { get; set; }
		[Required(ErrorMessage = "обязательное поле")]
		public string? message { get; set; }
        public DateTime? time { get; set; }
        public int UserId { get; set; }
        public User? user { get; set; }

    }
}
