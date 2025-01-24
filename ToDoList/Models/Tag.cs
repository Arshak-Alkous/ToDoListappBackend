namespace ToDoList.Models
{
    public class Tag
    {
        public int TagId { get; set; } 
        public string Name { get; set; }
        public List<Project> Projects { get; set; }=new ();
        public List<Models.Task> Tasks { get; set; }=new ();
    }
}
