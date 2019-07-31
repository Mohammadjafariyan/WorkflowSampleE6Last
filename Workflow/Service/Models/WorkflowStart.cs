using System.Collections.Generic;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSampleE6Last.Models;

namespace WorkflowSample.Service.Models
{
    /// <summary>
    /// ابجکت استارت گردش کار
    /// </summary>
    public class WorkflowStart : BaseWorkflowResponse
    {
        public WorkflowStart()
        {
            variables = new Dictionary<string, string>();
        }

        /// <summary>
        /// کد دیتای فرم ثبت شده 
        /// </summary>
        public string Id { get; set; }

        public string Username { get; set; }
        public string ProcessDefinitionKey { get; set; }
        public string Group { get; set; }
        
        
       
        public Dictionary<string,string> variables { get; set; }
    }

    
}