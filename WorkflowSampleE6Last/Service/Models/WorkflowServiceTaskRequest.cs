namespace WorkflowSample.Service.Models
{
    /// <summary>
    /// آبجکتی که از سمت engine گردش کار ، سرویس های بیزینس را فراخوانی میکند
    /// </summary>
    public class WorkflowServiceTaskRequest
    {
        
        /// <summary>
        /// کد دیتای فرم ثبت شده 
        /// </summary>
        public long Id { get; set; }

        public string Username { get; set; }
        public string UpperUsername { get; set; }
        public string TaeedKonandeUsername { get; set; }
    }
    /// <summary>
    /// آبجکتی که در پاسخ فراخوانی سرویس های بیزینس بر میگردد
    /// </summary>
    public class WorkflowServiceTaskResponse
    {
        
    }
}