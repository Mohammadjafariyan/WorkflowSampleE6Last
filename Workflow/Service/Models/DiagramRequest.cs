namespace WorkflowSample.Service.Models
{
    public class DiagramRequest
    {
        public long processInstanceId { get; set; }
        public string processDefinitionKey { get; set; }
        public string Username { get; set; }
    }
}