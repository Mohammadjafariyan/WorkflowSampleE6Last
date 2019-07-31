using WorkflowSample.Service.Models;

namespace WorkflowSample.Service
{
    public class WorkflowSettingSingleTon
    {
        private static WorkflowSetting _workflowSetting;

        public static WorkflowSetting WorkflowSetting
        {
            get
            {
                if (_workflowSetting == null)
                {
                    _workflowSetting = Init();
                }

                return _workflowSetting;
            }
            set { _workflowSetting = value; }
        }

        public static WorkflowSetting Init()
        {
            return new WorkflowSetting
            {
                BaseUrl = "http://localhost:8090",
                Username = "admin",
                Password = "admin",
            };
        }
        
        
        public static WorkflowSetting GetAdministrator()
        {
            return new WorkflowSetting
            {
                BaseUrl = "http://localhost:8090",
                Username = "admin",
                Password = "admin",
            };
        }
    }
}