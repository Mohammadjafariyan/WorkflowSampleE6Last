using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSample.Service.Models.WorkflowEngineModels.WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSampleE6Last.Models;

namespace Workflow.Controllers
{
    public class WorkflowController : ApiController
    {
        private OrgChartService _orgChartService = new OrgChartService();

        [HttpGet]
        public async Task<Result<FormProperty>> GetModel(string key)
        {
            try
            {
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();

                var model = await workflowService.GetModel(key, GenericController<Vacation>.GetUsername());


                List<FormProperty> list = new List<FormProperty>();

                for (int i = 0; i < model?.Processes?.Length; i++)
                {
                    for (int j = 0; j < model?.Processes[i]?.FlowElements?.Length; j++)
                    {
                        for (int k = 0; k < model?.Processes[i]?.FlowElements[j]?.FormProperties?.Length; k++)
                        {
                            var prop = model?.Processes[i]?.FlowElements[j]?.FormProperties[k];
                            if ( !string.IsNullOrEmpty(prop.Name) && ( prop.Name.StartsWith("U_") || prop.Name.StartsWith("G_")))
                                list.Add(prop);
                        }
                    }
                }

                return new Result<FormProperty>
                {
                    Grid = list,
                    Type = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new Result<FormProperty>
                {
                    Type = ResultType.Fail,
                    Message = e.Message
                };
            }
        }

        [HttpGet]
        public async Task<Result<EngineProcessDefinition>> GetProcessDefinitionById(string processDefinitionId)
        {
            try
            {
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();

                var model = await workflowService.GetProcessDefinitionById(processDefinitionId);

                return new Result<EngineProcessDefinition>
                {
                    SingleRecord = model,
                    Type = ResultType.Success
                };
            }
            catch (Exception e)
            {
                return new Result<EngineProcessDefinition>
                {
                    Type = ResultType.Fail,
                    Message = e.Message
                };
            }
        }


        [HttpGet]
        public async Task<byte[]> GetPhoto(string key)
        {
            try
            {
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();

                var model = await workflowService.GetPhoto(new DiagramRequest
                {
                    Username = GenericController<Vacation>.GetUsername(),
                    processDefinitionKey = key
                });


                return model.Content;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class BpmnModelViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}