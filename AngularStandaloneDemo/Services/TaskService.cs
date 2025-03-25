namespace AngularStandaloneDemo.Services
{
    public class TaskService
    {
        public AngularStandaloneDemo.Enums.TaskStatus ConvertToCustomTaskStatus(System.Threading.Tasks.TaskStatus status)
        {
            switch (status)
            {
                case System.Threading.Tasks.TaskStatus.Created:
                case System.Threading.Tasks.TaskStatus.WaitingForActivation:
                case System.Threading.Tasks.TaskStatus.WaitingToRun:
                case System.Threading.Tasks.TaskStatus.Running:
                    return AngularStandaloneDemo.Enums.TaskStatus.NotStarted;
                case System.Threading.Tasks.TaskStatus.RanToCompletion:
                    return AngularStandaloneDemo.Enums.TaskStatus.Completed;
                case System.Threading.Tasks.TaskStatus.Canceled:
                    return AngularStandaloneDemo.Enums.TaskStatus.Overdue;
                case System.Threading.Tasks.TaskStatus.Faulted:
                    return AngularStandaloneDemo.Enums.TaskStatus.InProgress;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
