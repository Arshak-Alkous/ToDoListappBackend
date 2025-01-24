namespace ToDoList.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Tag> Tags { get; set; }=new ();
    }
}
