using System.Threading.Tasks;
using Workflow.Controllers;
using WorkflowSample.Service.Models;

namespace WorkflowSample.Service.ServiceTasks
{
    public class SampleTestServiceTask:IWorkflowServiceTaskListener
    {
        public async Task<WorkflowServiceTaskResponse> Call(ListenerViewModel request)
        {
            return await Task.FromResult<WorkflowServiceTaskResponse>(null);
        }
    }
}