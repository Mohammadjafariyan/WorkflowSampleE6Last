using System.Collections.Generic;

namespace WorkflowSample.Service.Models.WorkflowEngineModels
{
    public class WorkflowContinueResponse:BaseWorkflowResponse
    {
        
    }
    
    public class WorkflowContinueRequest
    {
        public WorkflowContinueRequest()
        {
            variables=new Dictionary<string, string>();
        }

        public string Username { get; set; }
        public string Assignee { get; set; }
        
        public WorkflowContinueRequestStatus RequestStatus { get; set; }
        public Dictionary<string,string> variables { get; set; }
        public string TaskId { get; set; }
    }


    public enum WorkflowContinueRequestStatus
    {
       Complete,Claim,Delegate,Resolve
    }
}