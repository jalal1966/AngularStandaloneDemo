namespace AngularStandaloneDemo.Dtos
{
    public class TaskStatusUpdateDto
    {
        // public System.Threading.Tasks.TaskStatus Status { get; set; }
        public int TaskId { get; set; }
        public required string Status { get; set; }
    }
}
