using WorkflowSample.Service.Models;

namespace WorkflowSample.Service.Client
{
    public interface ILoggedUserProviderService
    {
        EngineUser GetCurrentLoggedInUserInfos();
    }

    public class LoggedUserProviderService : ILoggedUserProviderService
    {
        public EngineUser GetCurrentLoggedInUserInfos()
        {
            return new EngineUser
            {
                Id = WorkflowSettingSingleTon.WorkflowSetting.Username,
                Password = WorkflowSettingSingleTon.WorkflowSetting.Password,
                FirstName = WorkflowSettingSingleTon.WorkflowSetting.Username,
                LastName = WorkflowSettingSingleTon.WorkflowSetting.Username
            };
        }
    }
}