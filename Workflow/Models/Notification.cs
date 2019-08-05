namespace WorkflowSampleE6Last.Models
{
    public class Notification : IWorkflowModel
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string ReceiverUsername { get; set; }
        public string Title{ get; set; }
        public string Description{ get; set; }
    }
}