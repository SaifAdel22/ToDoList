using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ToDoList.ViewModels
{
    public class CreateTaskVM
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Deadline is required")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; } = DateTime.Now.AddDays(1);

        public IFormFile? TaskFile { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}