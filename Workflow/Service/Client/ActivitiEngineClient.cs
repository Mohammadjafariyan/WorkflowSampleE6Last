using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSample.Service.Models.WorkflowEngineModels.WorkflowSample.Service.Models.WorkflowEngineModels;

namespace WorkflowSample.Service.Client
{
    public class ActivitiEngineClient : IWorkflowEngineClient
    {
        private HttpClient _httpClient;
        private EngineTask EngineTask { get; set; }


        public async Task<WorkflowStartResponse> SendToWorkflow(WorkflowStart startModel)
        {
            var rStart = new WorkflowStartResponse();

            using (_httpClient = new HttpClient())
            {
                var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;


                SetDefaultHeaders(_httpClient);

                // ایجاد کاربر در engine اگر وجود نداشته باشد
                IsUserExistsOrNotCreateIt();


                Variable[] variables = ToVariables(startModel.variables, startModel);

                var startReq = new EngineActivateProcessDefinitionRequest
                {
                    ProcessDefinitionKey = startModel.ProcessDefinitionKey,
                    Variables = variables,
                };


                var jsonInString = JsonConvert.SerializeObject(startReq);
                var url = $@"{baseUrl}/runtime/process-instances";
                var resp = await _httpClient.PostAsync(url,
                    new StringContent(jsonInString, Encoding.UTF8, "application/json"));


                var str = await resp.Content.ReadAsStringAsync();
                var activateResponse = JsonConvert.DeserializeObject<EngineActivateProcessDefinitionResponse>(str);

                if ((int) resp.StatusCode == 201)
                {
                    rStart.Status = EngineResponseStatus.Success;
                    rStart.Id = activateResponse.Id;

                    if (activateResponse.Suspended)
                    {
                        throw new ActivitiEngineClientException("این گردش کار در وضعیت suspended قرار دارد");
                    }
                }
                else if ((int) resp.StatusCode == 400)
                {
                    rStart.Status = EngineResponseStatus.Fail;
                    rStart.Message = "گردش کار با key داده شده یافت نشد یا variable های اشتباه داده شده یا :"
                                     + resp.ReasonPhrase;
                }
                else if (resp.StatusCode == HttpStatusCode.InternalServerError)
                {
                    // Try to read response, log error etc.
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new Exception(errorxml);
                }
                else
                {
                    throw new Exception("این کد رسپانس پیاده سازی نشده");
                }
            }

            return await Task.FromResult(rStart);
        }

        private void IsUserExistsOrNotCreateIt()
        {
            var userService = InjectorSingleTon.Inject<IActivitiEngineUserManagementService>();
            var loggedUserProvider = InjectorSingleTon.Inject<ILoggedUserProviderService>();

            var loggedUser = loggedUserProvider.GetCurrentLoggedInUserInfos();
            userService.CreateUserIfNotExist(
                new EngineUser
                {
                    Id = loggedUser.Id,
                    Password = loggedUser.Password,
                    FirstName = loggedUser.FirstName,
                    LastName = loggedUser.LastName
                });
        }

        public async Task<WorkflowContinueResponse> Continue(WorkflowContinueRequest continueRequest)
        {
            using (_httpClient = new HttpClient())
            {
                SetDefaultHeaders(_httpClient);

                // ایجاد کاربر در engine اگر وجود نداشته باشد
                IsUserExistsOrNotCreateIt();

                // چک میکند TaskId  اشتباه نباشد
                await CheckTaskExists(_httpClient, continueRequest);

                // چک میکند که این Task قبلا انحصار نشده باشد 
                // چک میکند نام کاربری اشتباه داده نشده باشد
                CheckNotClaimedAndNotDeligatedBefore(_httpClient, continueRequest);


                var rContinue = PostTaskAction(_httpClient, continueRequest);
                return await rContinue;
            }
        }

        public async Task<EngineInboxTaskList> GetInboxTasks(EngineInboxTaskRequest inboxTaskRequest)
        {
            using (_httpClient = new HttpClient())
            {
                SetDefaultHeaders(_httpClient);

                // ایجاد کاربر در engine اگر وجود نداشته باشد
                IsUserExistsOrNotCreateIt();


                var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
                var url = $@"{baseUrl}/runtime/tasks" + inboxTaskRequest.ToUrl();
                var resp = await _httpClient.GetAsync(url);
                if ((int) resp.StatusCode == 200)
                {
                    return await ActivitiEngineClientHelper.ParseTaskList(resp);
                }
                else if ((int) resp.StatusCode == 400)
                {
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new ActivitiEngineClientException("پارامتر های اشتباه " + errorxml);
                }
                else
                {
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new Exception(errorxml);
                }
            }
        }

        public async Task<EngineProcessDefinitionGrid> GetProcessDefinitions(ProcessDefinitionsRequest request)
        {
            using (_httpClient = new HttpClient())
            {
                SetDefaultHeaders(_httpClient);

                // ایجاد کاربر در engine اگر وجود نداشته باشد
                IsUserExistsOrNotCreateIt();


                var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
                var url = $@"{baseUrl}/repository/process-definitions";
                var resp = await _httpClient.GetAsync(url);
                if ((int) resp.StatusCode == 200)
                {
                    return await ActivitiEngineClientHelper.ParseProcessDefinitions(resp);
                }
                else if ((int) resp.StatusCode == 400)
                {
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new ActivitiEngineClientException("پارامتر های اشتباه " + errorxml);
                }
                else
                {
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new Exception(errorxml);
                }
            }
        }

        public async Task<EngineProcessDiagram> GetDiagram(DiagramRequest request)
        {
            using (_httpClient = new HttpClient())
            {
                SetDefaultHeaders(_httpClient);

                // ایجاد کاربر در engine اگر وجود نداشته باشد
                IsUserExistsOrNotCreateIt();


                _httpClient.DefaultRequestHeaders.Add("responseType", "blob");

                var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
                var url = $@"{baseUrl}/runtime/process-instances/{request.processInstanceId}/diagram";
                var resp = await _httpClient.GetAsync(url);
                if ((int) resp.StatusCode == 200)
                {
                    return new EngineProcessDiagram {Content = await resp.Content.ReadAsByteArrayAsync()};
                }
                else if ((int) resp.StatusCode == 400)
                {
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new ActivitiEngineClientException("پارامتر های اشتباه " + errorxml);
                }
                else
                {
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new Exception(errorxml);
                }
            }
        }

        public async Task<EngineQueryHistoricProcessInstanceGrid> QueryHistoricProcessInstances(
            QueryHistoricProcessInstanceRequest request)
        {
            var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
            var url = $@"{baseUrl}/query/historic-task-instances";
            return await QueryHistoric<EngineQueryHistoricProcessInstanceGrid>(url, request);
        }


        public async Task<EngineQueryHistoricTasksGrid> QueryHistoricTasks(QueryHistoricProcessInstanceRequest request)
        {
            var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
            var url = $@"{baseUrl}/query/historic-task-instances";

            /*if (string.IsNullOrEmpty(request.taskAssignee))
                url += "&taskAssignee=" + request.taskAssignee;

            if (string.IsNullOrEmpty(request.taskAssigneeLike))
                url += "&taskAssigneeLike=" + request.taskAssigneeLike;

            
            if (string.IsNullOrEmpty(request.taskInvolvedUser))
                url += "&taskInvolvedUser=" + request.taskInvolvedUser;
         
            
            if (request.finished.HasValue)
                url += "&finished=" + request.finished;

              
            if (request.processFinished.HasValue)
                url += "&processFinished=" + request.processFinished;

            if (string.IsNullOrEmpty(request.taskName))
                url += "&taskName=" + request.taskName;

            
            if (string.IsNullOrEmpty(request.taskNameLike))
                url += "&taskNameLike=" + request.taskNameLike;

            if (string.IsNullOrEmpty(request.processDefinitionKey))
                url += "&processDefinitionKey=" + request.processDefinitionKey;
*/


            if (string.IsNullOrEmpty(request.processDefinitionKey) &&
                string.IsNullOrEmpty(request.ProcessDefinitionId))
                throw new ActivitiEngineClientException(
                    "باید یکی از پارامتر ها مقدار دهی شده باشند processDefinitionKey , ProcessDefinitionId: ");

            return await QueryHistoric<EngineQueryHistoricTasksGrid>(url, request);
        }


        #region private

        private async Task<T>
            QueryHistoric<T>(string url, QueryHistoricProcessInstanceRequest request)
            where T : BaseWorkflowResponse, new()
        {
            var rContinue = new T();

            var jsonInString = JsonConvert.SerializeObject(request);
            using (_httpClient = new HttpClient())
            {
                SetDefaultHeaders(_httpClient);

                // ایجاد کاربر در engine اگر وجود نداشته باشد
                IsUserExistsOrNotCreateIt();


                var resp = await _httpClient.PostAsync(url,
                    new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                if ((int) resp.StatusCode == 200)
                {
                    rContinue.Status = EngineResponseStatus.Success;

                    return await ActivitiEngineClientHelper.Parse<T>(resp);
                }
                else if ((int) resp.StatusCode == 400)
                {
                    rContinue.Status = EngineResponseStatus.Fail;
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    rContinue.Message = "مقادیر ارسالی اشتباه است یا assignee خالی است"
                                        + errorxml;
                }
                else if ((int) resp.StatusCode == 404)
                {
                    rContinue.Status = EngineResponseStatus.Fail;
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    rContinue.Message = "Task مورد نظر یافت نشد"
                                        + errorxml;
                }
                else if ((int) resp.StatusCode == 409)
                {
                    rContinue.Status = EngineResponseStatus.Fail;
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    rContinue.Message =
                        "Task به دلیل کانفلیکت انجام نمیشود یا Task بطور همزمان آپدیت میشود یا این Task توسط کاربر دیگر در انحصار انجام قرار دارد"
                        + errorxml;
                }
                else
                {
                    throw new ActivitiEngineClientException("این کد رسپانس پیاده سازی نشده");
                }
            }

            return rContinue;
        }

        private async void CheckNotClaimedAndNotDeligatedBefore(HttpClient httpClient,
            WorkflowContinueRequest continueRequest)
        {
            if (EngineTask == null)
                EngineTask = await CheckTaskExists(httpClient, continueRequest);

            if (EngineTask.DelegationState == "pending")
                throw new AccessViolationException("این Task به یک نفر دیگر Deligate شده است " + EngineTask.Assignee);

            if (EngineTask.Suspended)
                throw new AccessViolationException("این Task به یک نفر دیگر Suspended شده است " + EngineTask.Assignee);

            if (string.IsNullOrEmpty(EngineTask.Assignee) && continueRequest.RequestStatus != WorkflowContinueRequestStatus.Claim)
                throw new AccessViolationException("باید قبل از انجام این Task ابتدا آن را در وضعیت انحصار قرار دهید");

            var isEq = string.Equals(EngineTask.Assignee, continueRequest.Username);
            if ( !isEq  &&  continueRequest.RequestStatus != WorkflowContinueRequestStatus.Claim)
            {
                throw new AccessViolationException(
                    "این Task توسط یک نفر دیگر در وضعیت انحصار انجام قرار گرفته است یا نام کاربری اشتباه است " +
                    EngineTask.Assignee);
            }
        }

        private async Task<EngineTask> CheckTaskExists(HttpClient httpClient, WorkflowContinueRequest continueRequest)
        {
            var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
            var url = $@"{baseUrl}/runtime/tasks/{continueRequest.TaskId}";
            var resp = await httpClient.GetAsync(url);
            if ((int) resp.StatusCode == 200)
            {
                return this.EngineTask = await ActivitiEngineClientHelper.ParseTask(resp);
            }
            else if ((int) resp.StatusCode == 404)
            {
                throw new ActivitiEngineClientException("این Task با TaskId داده شده یافت نشد");
            }
            else
            {
                var errorxml = await resp.Content.ReadAsStringAsync();
                throw new Exception(errorxml);
            }
        }


        private async Task<WorkflowContinueResponse> PostTaskAction(HttpClient httpClient,
            WorkflowContinueRequest continueRequest)
        {
            var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
            var rContinue = new WorkflowContinueResponse();
            Variable[] variables = ToVariables(continueRequest.variables, continueRequest);

            var actionReq = new EngineTaskActionRequest
            {
                Action = Enum.GetName(typeof(WorkflowContinueRequestStatus), continueRequest.RequestStatus).ToLower(),
                Assignee = continueRequest.Assignee,
                Variables = variables,
            };


            var jsonInString = JsonConvert.SerializeObject(actionReq);
            var url = $@"{baseUrl}/runtime/tasks/{continueRequest.TaskId}";
            var resp = await _httpClient.PostAsync(url,
                new StringContent(jsonInString, Encoding.UTF8, "application/json"));


            if ((int) resp.StatusCode == 200)
            {
                rContinue.Status = EngineResponseStatus.Success;
            }
            else if ((int) resp.StatusCode == 400)
            {
                rContinue.Status = EngineResponseStatus.Fail;
                var errorxml = await resp.Content.ReadAsStringAsync();
                rContinue.Message = "مقادیر ارسالی اشتباه است یا assignee خالی است"
                                    + errorxml;
            }
            else if ((int) resp.StatusCode == 404)
            {
                rContinue.Status = EngineResponseStatus.Fail;
                var errorxml = await resp.Content.ReadAsStringAsync();
                rContinue.Message = "Task مورد نظر یافت نشد"
                                    + errorxml;
            }
            else if ((int) resp.StatusCode == 409)
            {
                rContinue.Status = EngineResponseStatus.Fail;
                var errorxml = await resp.Content.ReadAsStringAsync();
                rContinue.Message =
                    "Task به دلیل کانفلیکت انجام نمیشود یا Task بطور همزمان آپدیت میشود یا این Task توسط کاربر دیگر در انحصار انجام قرار دارد"
                    + errorxml;
            }
            else
            {
                rContinue.Status = EngineResponseStatus.Fail;
                var errorxml = await resp.Content.ReadAsStringAsync();
                throw new ActivitiEngineClientException(" این کد رسپانس پیاده سازی نشده " +  errorxml);
            }

            return rContinue;
        }


        private Variable[] ToVariables(Variable[] startModelVariables, QueryHistoricProcessInstanceRequest rStart)
        {
            List<Variable> vars = new List<Variable>();
            foreach (var variable in startModelVariables)
            {
                vars.Add(new Variable
                {
                    Name = variable.Name,
                    Value = variable.Value
                });
            }

            return vars.ToArray();
        }

        private void SetDefaultHeaders(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            $"{WorkflowSettingSingleTon.WorkflowSetting.Username}:{WorkflowSettingSingleTon.WorkflowSetting.Password}")));
        }

        private Variable[] ToVariables
            (Dictionary<string, string> startModelVariables, WorkflowStart rStart)
        {
            startModelVariables.Add("id", rStart.Id.ToString());
            startModelVariables.Add("username", rStart.Username.ToString());

            List<Variable> vars = new List<Variable>();
            foreach (var variable in startModelVariables)
            {
                vars.Add(new Variable
                {
                    Name = variable.Key,
                    Value = variable.Value,
                    Scope = "global"
                });
            }


            return vars.ToArray();
        }


        private Variable[] ToVariables
            (Dictionary<string, string> startModelVariables, WorkflowContinueRequest rStart)
        {

            if (startModelVariables == null)
            {
                startModelVariables = new Dictionary<string, string>();
            }
            //     startModelVariables.Add("id", rStart.Id.ToString());
            startModelVariables.Add("username", rStart.Username.ToString());
           // startModelVariables.Add("upperUsername", rStart.UpperUsername.ToString());

            List<Variable> vars = new List<Variable>();
            foreach (var variable in startModelVariables)
            {
                vars.Add(new Variable
                {
                    Name = variable.Key,
                    Value = variable.Value,
                    Scope = "local"
                });
                /*if(variable.Key=="upperUsername"){
                 
                }*/
                /*else
                {
                    vars.Add(new Variable
                    {
                        Name = variable.Key,
                        Value = variable.Value,
                        Scope = "global"
                    });
                }*/
            }


            return vars.ToArray();
        }

        #endregion
    }

    public interface IWorkflowEngineClient
    {
        Task<WorkflowStartResponse> SendToWorkflow(WorkflowStart startModel);
        Task<WorkflowContinueResponse> Continue(WorkflowContinueRequest continueRequest);
        Task<EngineInboxTaskList> GetInboxTasks(EngineInboxTaskRequest inboxTaskRequest);
        Task<EngineProcessDefinitionGrid> GetProcessDefinitions(ProcessDefinitionsRequest request);

        Task<EngineProcessDiagram> GetDiagram(DiagramRequest request);

        Task<EngineQueryHistoricProcessInstanceGrid> QueryHistoricProcessInstances(
            QueryHistoricProcessInstanceRequest request);

        Task<EngineQueryHistoricTasksGrid> QueryHistoricTasks(
            QueryHistoricProcessInstanceRequest queryHistoricProcessInstanceRequest);
    }

    public class ActivitiEngineClientException : Exception
    {
        public ActivitiEngineClientException(string msg) : base(msg)
        {
        }
    }

    public class ActivitiEngineClientHelper
    {
        public static async Task<EngineTask> ParseTask(HttpResponseMessage resp)
        {
            return await Parse<EngineTask>(resp);
        }

        public static async Task<T> Parse<T>(HttpResponseMessage resp)
        {
            if (resp == null)
                throw new Exception("response از سروس در درخواست Task نال است");

            var str = await resp.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(str))
                throw new Exception("response از سروس در درخواست Task خالی است");

            var m = JsonConvert.DeserializeObject<T>(str);
            return m;
        }

        public static async Task<EngineInboxTaskList> ParseTaskList(HttpResponseMessage resp)
        {
            return await Parse<EngineInboxTaskList>(resp);
        }

        public static async Task<EngineProcessDefinitionGrid> ParseProcessDefinitions(HttpResponseMessage resp)
        {
            return await Parse<EngineProcessDefinitionGrid>(resp);
        }

        public static async Task<EngineQueryHistoricProcessInstanceGrid> ParseQueryHistoricProcessInstances(
            HttpResponseMessage resp)
        {
            return await Parse<EngineQueryHistoricProcessInstanceGrid>(resp);
        }

        public static async Task<EngineQueryHistoricTasksGrid> ParseQueryHistoricTasks(HttpResponseMessage resp)
        {
            return await Parse<EngineQueryHistoricTasksGrid>(resp);
        }
    }
}