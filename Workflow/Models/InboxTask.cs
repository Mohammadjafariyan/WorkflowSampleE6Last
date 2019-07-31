using System;
using WorkflowSample.Service.Models.WorkflowEngineModels;

namespace WorkflowSampleE6Last.Models
{
    public class InboxTaskViewModel<T> where T : IWorkflowModel
    {
        /// <summary>
        /// تایید کننده
        /// </summary>
        public string Assignee { get; set; }

        /// <summary>
        /// تایید کننده
        /// </summary>
        public string AssigneeTranslate { get; set; }

        public string Subject { get; set; }

        /// <summary>
        /// نام فرم
        /// </summary>
        public string FormName { get; set; }

        public DateTimeOffset RequestDate { get; set; }

        public ProcessInstanceStatus Status { get; set; }
        public string StatusMessage { get; set; }
        public T Record { get; set; }
        public long ProcessInstanceId { get; set; }
        public long TaskId { get; set; }
    }

    public enum ProcessInstanceStatus
    {
        Error,
        InProgress,
        Done
    }
}