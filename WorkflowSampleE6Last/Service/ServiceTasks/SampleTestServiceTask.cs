using WorkflowSample.Service.Models;

namespace WorkflowSample.Service.ServiceTasks
{
    public class SampleTestServiceTask:IWorkflowServiceTaskListener
    {
        public WorkflowServiceTaskResponse Call(WorkflowServiceTaskRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}