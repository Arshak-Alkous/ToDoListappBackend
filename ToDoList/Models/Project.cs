namespace ToDoList.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public bool IsActive {  get; set; }
        public List<Models.Task> Tasks { get; set; }= new();
        public List<Tag> Tags { get; set; }= new();
    }
}
