namespace ToDoList.ViewModels
{
    public class ToDoTaskVM
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime Deadline { get; set; } = DateTime.Now.AddDays(1);

        public bool IsCompleted { get; set; }

        public int UserId { get; set; }

        public IFormFile? File { get; set; }

        public string? ExistingFilePath { get; set; }
    }
}
