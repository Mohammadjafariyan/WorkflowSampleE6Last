using WorkflowSample.Service.Models;

namespace WorkflowSample.Service
{
    public interface IWorkflowServiceTaskListener
    {
        WorkflowServiceTaskResponse Call(WorkflowServiceTaskRequest request);
    }
}