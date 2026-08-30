using ToDoList.Models;

namespace ToDoList.ViewModels
{
    public class UserVM
    {
        public string? SearchName { get; set; }

        public User? User { get; set; }
    }
}