using System;
using System.Threading.Tasks;
using System.Web.Http;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels.WorkflowSample.Service.Models.WorkflowEngineModels;

namespace Workflow.Controllers
{
    public class ProcessDefinitionController : ApiController
    {
        [HttpGet]
        public async Task<Result<EngineProcessDefinitionGrid>> GetAll()
        {
            try
            {
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();

                return new Result<EngineProcessDefinitionGrid>
                {
                    SingleRecord = await workflowService.GetProcessDefinitions(new ProcessDefinitionsRequest()),
                    Type = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new Result<EngineProcessDefinitionGrid>
                {
                    Type = ResultType.Fail,
                    Message = e.Message
                };
            }
        }
    }


   
}