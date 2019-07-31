namespace WorkflowSampleE6Last.Models
{
    public class Vacation:IModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public VacationType Type { get; set; }
        
    }
    
    public interface IModel
    {
         long Id { get; set; }
    }


    public enum VacationType
    {
        استحقاقی,استعلاجی,اشعه
    }
}