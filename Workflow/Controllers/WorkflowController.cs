using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSampleE6Last.Models;

namespace Workflow.Controllers
{
    public class WorkflowController : ApiController
    {
        
        private OrgChartService _orgChartService=new OrgChartService();
       
    }
}