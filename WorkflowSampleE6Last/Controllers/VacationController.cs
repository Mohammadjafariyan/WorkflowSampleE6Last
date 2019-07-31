using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSampleE6Last.Models;

namespace WorkflowSampleE6Last.Controllers
{
    public class VacationController : GenericController<Vacation>
    {
        private WorkflowService _workflowService = InjectorSingleTon.Inject<WorkflowService>();
        private OrgChartService _orgChartService = new OrgChartService();

        [HttpPost]
        public async Task<Result<VoidResult>> SaveAndSendToWorkflow(Vacation model)
        {
            try
            {
                Save(model);
                var ws = new WorkflowStart
                {
                    Id = model.Id.ToString(),
                    Username = User.Identity.Name,
                    UpperUsername = _orgChartService.GetUpperUsername(User.Identity.Name),
                    ProcessDefinitionKey = "vacationRequest",
                };

                var res = await _workflowService.SendToWorkflow(ws);

                return new Result<VoidResult>
                {
                    Type = res.Status == EngineResponseStatus.Success ? ResultType.Success : ResultType.Fail,
                    Message = res.Message,
                };
            }
            catch (Exception e)
            {
                return new Result<VoidResult> {Message = e.Message, Type = ResultType.Fail};
            }
        }
    }
}

