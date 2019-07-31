using System;
using Newtonsoft.Json;

namespace WorkflowSample.Service.Models.WorkflowEngineModels
{

    public class BaseWorkflowResponse
    {
        public string Message { get; set; }
        public EngineResponseStatus Status { get; set; }

    }
    public class WorkflowStartResponse:BaseWorkflowResponse
    {
        /// <summary>
        /// کد Process definition ایجاد شده
        /// </summary>
        public long Id { get; set; }
       
    }

    public enum EngineResponseStatus
    {
        Success,Fail
    }
}