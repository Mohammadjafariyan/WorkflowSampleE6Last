using WorkflowSample.Service.Models;

namespace WorkflowSample.Service
{
    public class WorkflowTaskServiceListenerCaller
    {

        public static WorkflowServiceTaskResponse Call
            (WorkflowServiceTaskRequest request,string serviceTaskName)
        {
            IWorkflowServiceTaskListener listener= WorkflowServiceTaskFactory.GetService(serviceTaskName);
            return listener.Call(request);
        }
    }
}