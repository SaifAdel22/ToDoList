using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
    public class EditVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Deadline is required")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; } = DateTime.Now.AddDays(1);

        public string? FilePath { get; set; }

        public bool IsCompleted { get; set; }


        public IFormFile? TaskFile { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
