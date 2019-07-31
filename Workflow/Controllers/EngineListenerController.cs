using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WorkflowSample.Service;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;

namespace Workflow.Controllers
{
    public class EngineListenerController : ApiController
    {
        [HttpPost]
        public async Task<WorkflowServiceTaskResponse> Notify(ListenerViewModel model)
        {
            try
            {
                if (model?.Description == null)
                {
                    throw new Exception("مدل یا نام لیسنر نال است");
                }

                return await WorkflowTaskServiceListenerCaller.Call(model, model.Description);
            }
            catch (Exception e)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    //   Content = new StringContent(string.Format("No product with ID = {0}", model.Description)),
                    Content = new StringContent(e.Message),
                    ReasonPhrase = e.Message
                };
                throw new HttpResponseException(resp);
            }
        }
    }


    public class ListenerViewModel
    {
        public string Id { get; set; }
        public string Assignee { get; set; }
        public string Candidates { get; set; }
        public string DelegationState { get; set; }
        public string ExecutionId { get; set; }
        public string ProcessDefinitionId { get; set; }
        public string ProcessInstanceId { get; set; }
        public string Name { get; set; }
        public string TaskDefinitionKey { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public List<Variable> Variables { get; set; }
    }
}