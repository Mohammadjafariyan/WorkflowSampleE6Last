using System.Threading.Tasks;
using Workflow.Controllers;
using WorkflowSample.Service.Models;

namespace WorkflowSample.Service
{
    public class WorkflowTaskServiceListenerCaller
    {

        public static async Task<WorkflowServiceTaskResponse> Call
            (ListenerViewModel request,string serviceTaskName)
        {
            IWorkflowServiceTaskListener listener= WorkflowServiceTaskFactory.GetService(serviceTaskName);
            return await listener.Call(request);
        }
    }
}