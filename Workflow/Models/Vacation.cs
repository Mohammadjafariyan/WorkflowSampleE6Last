using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WorkflowSample.Service.Models.WorkflowEngineModels;

namespace WorkflowSampleE6Last.Models
{
    public class Vacation : IWorkflowModel
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public VacationType Type { get; set; }
    }

    public class Account : IWorkflowModel
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public long? ParentId { get; set; }
        public string Username { get; set; }

        [NotMapped] public string UpperUsername { get; set; }


        public virtual Account Parent { get; set; }
        public virtual List<Account> Children { get; set; }
    }


    public interface IWorkflowModel
    {
        long Id { get; set; } 
        string Subject { get; set; }

    }


    public enum VacationType
    {
        استحقاقی,
        استعلاجی,
        اشعه
    }
}