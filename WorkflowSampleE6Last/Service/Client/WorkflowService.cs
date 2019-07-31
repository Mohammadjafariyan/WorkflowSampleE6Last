using System;
using System.Linq;
using System.Threading.Tasks;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSample.Service.Models.WorkflowEngineModels.WorkflowSample.Service.Models.WorkflowEngineModels;

namespace WorkflowSample.Service.Client
{
    public interface IWorkflowService
    {
        Task<WorkflowStartResponse> SendToWorkflow(WorkflowStart startModel);
        Task<WorkflowContinueResponse> Continue(WorkflowContinueRequest continueRequest);
        Task<EngineInboxTaskList> GetInboxTasks(EngineInboxTaskRequest inboxTaskRequest);
        Task<EngineProcessDefinitionGrid> GetProcessDefinitions(ProcessDefinitionsRequest request);
        Task<EngineProcessDiagram> GetDiagram(DiagramRequest request);
        Task<EngineQueryHistoricProcessInstanceGrid> QueryHistoricProcessInstances(QueryHistoricProcessInstanceRequest request);
    }

    public class WorkflowService : IWorkflowService
    {
        public async Task<WorkflowStartResponse> SendToWorkflow(WorkflowStart startModel)
        {
            Validate(startModel);

            var engineClient = InjectorSingleTon.Inject<IWorkflowEngineClient>();

            return await engineClient.SendToWorkflow(startModel);
        }

        public async Task<WorkflowContinueResponse> Continue(WorkflowContinueRequest continueRequest)
        {
            ValidateContinueRequest(continueRequest);

            var engineClient = InjectorSingleTon.Inject<IWorkflowEngineClient>();


            return await engineClient.Continue(continueRequest);
        }

        public async Task<EngineInboxTaskList> GetInboxTasks(EngineInboxTaskRequest inboxTaskRequest)
        {
            ValidateInboxTaskRequest(inboxTaskRequest);

            var engineClient = InjectorSingleTon.Inject<IWorkflowEngineClient>();


            return await engineClient.GetInboxTasks(inboxTaskRequest);
        }


        public async Task<EngineProcessDefinitionGrid> GetProcessDefinitions(ProcessDefinitionsRequest request)
        {
            ValidateGetProcessDefinitions(request);

            var engineClient = InjectorSingleTon.Inject<IWorkflowEngineClient>();


            return await engineClient.GetProcessDefinitions(request);
        }

    

        public async Task<EngineProcessDiagram> GetDiagram(DiagramRequest request)
        {
            ValidateGetDiagram(request);

            var engineClient = InjectorSingleTon.Inject<IWorkflowEngineClient>();


            return await engineClient.GetDiagram(request);
        }

        public async Task<EngineQueryHistoricProcessInstanceGrid> QueryHistoricProcessInstances(QueryHistoricProcessInstanceRequest request)
        {
            ValidateQueryHistoricProcessInstancesRequest(request);

            var engineClient = InjectorSingleTon.Inject<IWorkflowEngineClient>();


            return await engineClient.QueryHistoricProcessInstances(request);
        }
        public async Task<EngineQueryHistoricTasksGrid> 
            QueryHistoricTasks(QueryHistoricProcessInstanceRequest queryHistoricProcessInstanceRequest)
        {
            ValidateHistoricTasks(queryHistoricProcessInstanceRequest);

            var engineClient = InjectorSingleTon.Inject<IWorkflowEngineClient>();


            return await engineClient.QueryHistoricTasks(queryHistoricProcessInstanceRequest);

        }

     


        #region private
        private void ValidateHistoricTasks(object request)
        {
        }
        private void ValidateQueryHistoricProcessInstancesRequest(QueryHistoricProcessInstanceRequest request)
        {
            
        }
        private void ValidateGetProcessDefinitions(ProcessDefinitionsRequest request)
        {
        }
        private void ValidateInboxTaskRequest(EngineInboxTaskRequest inboxTaskRequest)
        {
            if (string.IsNullOrEmpty(inboxTaskRequest.assignee) &&
                string.IsNullOrEmpty(inboxTaskRequest.assigneeLike) &&
                string.IsNullOrEmpty(inboxTaskRequest.candidateGroup) &&
                string.IsNullOrEmpty(inboxTaskRequest.candidateGroups) &&
                string.IsNullOrEmpty(inboxTaskRequest.candidateUser) &&
                inboxTaskRequest.unassigned == false &&
                string.IsNullOrEmpty(inboxTaskRequest.candidateOrAssigned) &&
                string.IsNullOrEmpty(inboxTaskRequest.involvedUser)
            )
                throw new WorkflowServiceException("گیرنده مشخص نشده است");
        }

        private static void ValidateContinueRequest(WorkflowContinueRequest continueRequest)
        {
            if (string.IsNullOrEmpty(continueRequest.TaskId))
                throw new WorkflowServiceException("TaskId نال است");

            if (continueRequest.RequestStatus == WorkflowContinueRequestStatus.Delegate)
                if (string.IsNullOrEmpty(continueRequest.Assignee))
                    throw new WorkflowServiceException("در صورت Deligate باید Assignee داده شود");

            if (string.IsNullOrEmpty(continueRequest.Username))
                throw new WorkflowServiceException("ارسال username برای هر Task لازم است");

            if (string.IsNullOrEmpty(continueRequest.UpperUsername))
                throw new WorkflowServiceException("ارسال username برای هر Task لازم است");

            if (string.IsNullOrEmpty(continueRequest.TaskId))
                throw new WorkflowServiceException("TaskId نال است");
        }


        private void Validate(WorkflowStart startModel)
        {
            if (startModel == null)
                throw new WorkflowServiceException("ابجکت نال ارسال شده است");

            if (string.IsNullOrEmpty(startModel.Username))
                throw new WorkflowServiceException($@"نام کاربری خالی است");

            if (string.IsNullOrEmpty(startModel.UpperUsername))
                throw new WorkflowServiceException($@"نام کاربری تایید کننده است");

            if (string.IsNullOrEmpty(startModel.Id))
                throw new WorkflowServiceException($@"کد رکورد  صفر است");


            if (string.IsNullOrEmpty(startModel.ProcessDefinitionKey))
                throw new WorkflowServiceException($@"ProcessDefinitionKey ارسال نشده است");

            //todo: باید بررسی شود آیا این key وجورد دارد یا خیر
        }
        private void ValidateGetDiagram(object inboxTaskRequest)
        {
        }

        private void ValidateVariable(string id, WorkflowStart startModel)
        {
            if (!startModel.variables.ContainsKey(id))
                throw new WorkflowServiceException($@" {id} ارسال نشده است");

            if (startModel.variables[id] == null)
                throw new WorkflowServiceException($@"{id} ارسال نشده است");
        }

        #endregion

        
    }

    public class WorkflowServiceException : Exception
    {
        public WorkflowServiceException(string msg) : base(msg)
        {
        }
    }
}