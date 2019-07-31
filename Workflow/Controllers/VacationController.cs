using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSampleE6Last.Models;

namespace Workflow.Controllers
{
    public class VacationController : GenericController<Vacation>
    {
        public VacationController()
        {
            thisControllerProcessDefinitionKey = "vacationRequest2";
            this.FormName = "فرم درخواست مرخصی";
        }

        protected override string GetGroup()
        {
            return new FakeGroupUserProvider().GetGroupByUser(GetCurrentUsername());
        }


        protected override void BeforeSendToWorkflow(WorkflowStart ws, Vacation model)
        {
            ws.variables.Add("vacationType", ((int) model.Type).ToString());
        }
    }

    public class FakeGroupUserProvider
    {
        private Dictionary<string, string> GroupUsers = new Dictionary<string, string>();

        public FakeGroupUserProvider()
        {
            GroupUsers.Add("mansur", "secendary");
            GroupUsers.Add("asghar", "mali");
        }

        public string GetGroupByUser(string getCurrentUsername)
        {
            var res = GroupUsers.Where(g => g.Key.ToLower() == getCurrentUsername.ToLower()).Select(g => g.Value)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(res))
            {
                return "notfound";
            }

            return res;
        }
    }
}