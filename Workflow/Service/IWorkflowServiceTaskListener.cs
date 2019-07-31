using System.Threading.Tasks;
using Workflow.Controllers;
using WorkflowSample.Service.Models;

namespace WorkflowSample.Service
{
    public interface IWorkflowServiceTaskListener
    {
        Task<WorkflowServiceTaskResponse> Call(ListenerViewModel request);
    }
}