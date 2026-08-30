namespace ToDoList.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDoTask> ToDoTasks { get; set; } = new List<ToDoTask>();
    }
}
