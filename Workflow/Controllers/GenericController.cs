using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.Injector;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;
using WorkflowSampleE6Last.Models;

namespace Workflow.Controllers
{
    public class GenericController<TModel> : ApiController where TModel : class, IWorkflowModel
    {
        private IWorkflowService _workflowService;
        private OrgChartService _orgChartService = new OrgChartService();
        protected string thisControllerProcessDefinitionKey;
        protected string FormName;


        [HttpPost]
        public virtual async Task<Result<InboxTaskViewModel<TModel>>> Claim(string taskId)
        {
            return await DoAction(taskId, WorkflowContinueRequestStatus.Claim);
        }

        [HttpPost]
        public virtual async Task<Result<InboxTaskViewModel<TModel>>> UnClaim(string taskId)
        {
            return await DoAction(taskId, WorkflowContinueRequestStatus.Claim, true);
        }

        protected virtual async Task<Result<InboxTaskViewModel<TModel>>> DoAction(string taskId,
            WorkflowContinueRequestStatus requestStatus, bool notClaim = false)
        {
            try
            {
                var username = HttpContext.Current.Session["loggedInUsername"];
                if (username == null || string.IsNullOrEmpty(username.ToString()))
                {
                    throw new Exception("کاربر کنونی وارد نشده است");
                }

                var request = new WorkflowContinueRequest
                {
                    Username = username.ToString(),
                    RequestStatus = requestStatus,
                    Assignee = notClaim ? null : username.ToString(),
                    TaskId = taskId
                };
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();

                var tasks = await workflowService.Continue(request);

                return new Result<InboxTaskViewModel<TModel>> {Type = ResultType.Success};
            }
            catch (HttpRequestException e)
            {
                return new Result<InboxTaskViewModel<TModel>>
                {
                    Message = e.Message + e.InnerException?.Message + " سرور گردش کار فعال نیست خطا در اتصال ",
                    Type = ResultType.Fail
                };
            }
            catch (Exception e)
            {
                return new Result<InboxTaskViewModel<TModel>> {Message = e.Message, Type = ResultType.Fail};
            }
        }


        [HttpPost]
        public virtual async Task<Result<byte[]>> GetDiagram(long processInstanceId)
        {
            try
            {
                var username = HttpContext.Current.Session["loggedInUsername"];
                if (username == null || string.IsNullOrEmpty(username.ToString()))
                {
                    throw new Exception("کاربر کنونی وارد نشده است");
                }

                var request = new DiagramRequest()
                {
                    Username = username.ToString(),
                    processInstanceId = processInstanceId
                };
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();

                var diagram = await workflowService.GetDiagram(request);

                return new Result<byte[]> {Type = ResultType.Success, SingleRecord = diagram.Content};
            }
            catch (HttpRequestException e)
            {
                return new Result<byte[]>
                {
                    Message = e.Message + e.InnerException?.Message + " سرور گردش کار فعال نیست خطا در اتصال ",
                    Type = ResultType.Fail
                };
            }
            catch (Exception e)
            {
                return new Result<byte[]> {Message = e.Message, Type = ResultType.Fail};
            }
        }


        [HttpPost]
        public virtual async Task<Result<InboxTaskViewModel<TModel>>>
            Continue([FromUri] string taskId, [FromBody] Dictionary<string, string> vars)
        {
            try
            {
                var username = HttpContext.Current.Session["loggedInUsername"];
                if (username == null || string.IsNullOrEmpty(username.ToString()))
                {
                    throw new Exception("کاربر کنونی وارد نشده است");
                }

                var request = new WorkflowContinueRequest
                {
                    Username = username.ToString(),
                    variables = vars,
                    TaskId = taskId,
                    RequestStatus = WorkflowContinueRequestStatus.Complete,
                };
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();


                BeforeContinue(request, vars);

                var tasks = await workflowService.Continue(request);

                AfterContinue(request, vars);


                return new Result<InboxTaskViewModel<TModel>> {Type = ResultType.Success};
            }
            catch (HttpRequestException e)
            {
                return new Result<InboxTaskViewModel<TModel>>
                {
                    Message = e.Message + e.InnerException?.Message + " سرور گردش کار فعال نیست خطا در اتصال ",
                    Type = ResultType.Fail
                };
            }
            catch (Exception e)
            {
                OnContinueError(e, vars);
                return new Result<InboxTaskViewModel<TModel>> {Message = e.Message, Type = ResultType.Fail};
            }
        }

        protected virtual void AfterContinue(WorkflowContinueRequest request, Dictionary<string, string> vars)
        {
        }

        protected virtual void BeforeContinue(WorkflowContinueRequest request, Dictionary<string, string> vars)
        {
        }

        protected virtual void OnContinueError(Exception exception, Dictionary<string, string> vars)
        {
        }

        protected virtual string GetCurrentUsername()
        {
            var username = HttpContext.Current.Session["loggedInUsername"];
            if (username == null || string.IsNullOrEmpty(username.ToString()))
            {
                throw new Exception("کاربر کنونی وارد نشده است");
            }

            return username.ToString();
        }

        /// <summary>
        /// درخواست های مرجوع
        /// </summary>
        /// <param name="isClaimed">آیا کارتابل شخصی باشد ؟</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public virtual async Task<Result<InboxTaskViewModel<TModel>>> GetInboxTasks(bool isClaimed)
        {
            try
            {
                var username = HttpContext.Current.Session["loggedInUsername"];
                if (username == null || string.IsNullOrEmpty(username.ToString()))
                {
                    throw new Exception("کاربر کنونی وارد نشده است");
                }

                var request = new EngineInboxTaskRequest
                {
                    assignee = isClaimed ? username.ToString() : null,
                    candidateUser = isClaimed ? null : username.ToString(),
                    candidateGroup = isClaimed ? null : GetGroup(),
                    candidateGroups = isClaimed ? null : GetGroups(),
                };
                var workflowService = InjectorSingleTon.Inject<IWorkflowService>();

                var tasks = await workflowService.GetInboxTasks(request);


                List<InboxTaskViewModel<TModel>> list = new List<InboxTaskViewModel<TModel>>();
                foreach (var task in tasks.Data)
                {
                    var viewModel = new InboxTaskViewModel<TModel>();

                    //status
                    var status = task.Variables
                        .FirstOrDefault(v => v.Name == "status");
                    int val = 0;
                    if (int.TryParse(status?.Value, out val))
                    {
                        viewModel.StatusMessage += "<br>" + " اشکال در خواندن status " + status;
                    }
                    else
                    {
                        viewModel.Status = status?.Value != null ? (ProcessInstanceStatus) val : 0;
                    }

                    viewModel.Assignee = task.Assignee;

                    // assignee Translate
                    viewModel.AssigneeTranslate = _orgChartService.GetPersonnelNameByUsername(task.Assignee);

                    //form name
                    var formName = task.Variables.Where(v => v.Name == "formName").Select(v => v.Value)
                        .FirstOrDefault();
                    if (string.IsNullOrEmpty(formName))
                    {
                        viewModel.StatusMessage += "<br>" + " FormName  داده نشده است";
                    }
                    else
                    {
                        viewModel.FormName = formName;
                    }


                    //Id
                    var idstr = task.Variables.Where(v => v.Name == "id").Select(v => v.Value).FirstOrDefault();
                    long id = 0;
                    if (string.IsNullOrEmpty(idstr) || long.TryParse(idstr, out id) == false)
                    {
                        throw new Exception(" این گردش کار دارای Id نیست " + viewModel.StatusMessage);
                    }

                    var record = GetById(id).SingleRecord;
                    // subject
                    viewModel.Subject = record.Subject;

                    //record
                    viewModel.Record = record;

                    viewModel.RequestDate = task.CreateTime;


                    viewModel.ProcessInstanceId = task.ProcessInstanceId;
                    viewModel.TaskId = task.Id;

                    list.Add(viewModel);
                }


                return new Result<InboxTaskViewModel<TModel>> {Type = ResultType.Success, Grid = list};
            }
            catch (HttpRequestException e)
            {
                return new Result<InboxTaskViewModel<TModel>>
                {
                    Message = e.Message + e.InnerException?.Message + " سرور گردش کار فعال نیست خطا در اتصال ",
                    Type = ResultType.Fail
                };
            }
            catch (Exception e)
            {
                return new Result<InboxTaskViewModel<TModel>> {Message = e.Message, Type = ResultType.Fail};
            }
        }

        protected virtual string GetGroups()
        {
            return null;
        }


        protected virtual string GetGroup()
        {
            return _orgChartService.GetUpperUsername(User.Identity.Name);
        }

        protected virtual string GetUsername()
        {
            var username = HttpContext.Current.Session["loggedInUsername"];
            if (username == null || string.IsNullOrEmpty(username.ToString()))
            {
                throw new Exception("کاربر کنونی وارد نشده است");
            }

            return username.ToString();
        }

        /*public VacationController(WorkflowService workflowService)
        {
            this._workflowService = workflowService;
        "vacationRequest"}*/

        [HttpPost]
        public async Task<Result<VoidResult>> SaveAndSendToWorkflow(TModel model)
        {
            try
            {
                BeforeSave(model);
                Save(model);
                AfterSave(model);

                if (string.IsNullOrEmpty(FormName))
                    throw new Exception("نام فارسی این گردش کار داده نشده است");

                if (string.IsNullOrEmpty(thisControllerProcessDefinitionKey))
                    throw new Exception("thisControllerProcessDefinitionKey این گردش کار داده نشده است");

                if (string.IsNullOrEmpty(GetUsername()))
                    throw new Exception("username این گردش کار داده نشده است");


                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("formName", FormName);


                var ws = new WorkflowStart
                {
                    Id = model.Id.ToString(),
                    Username = GetUsername(),
                    Group = GetGroup(),
                    ProcessDefinitionKey = thisControllerProcessDefinitionKey,
                    variables = dic
                };

                _workflowService = InjectorSingleTon.Inject<IWorkflowService>();
                BeforeSendToWorkflow(ws,model);

                var res = await _workflowService.SendToWorkflow(ws);
                AfterSendToWorkflow(ws,model);

                return new Result<VoidResult>
                {
                    Type = res.Status == EngineResponseStatus.Success ? ResultType.Success : ResultType.Fail,
                    Message = res.Message,
                };
            }
            catch (Exception e)
            {
                ErrorSendToWorkflow(e,model);
                return new Result<VoidResult> {Message = e.Message, Type = ResultType.Fail};
            }
        }

        protected virtual void ErrorSendToWorkflow(Exception exception,TModel model)
        {
        }

        protected virtual void AfterSendToWorkflow(WorkflowStart ws,TModel model)
        {
        }


        protected virtual void BeforeSendToWorkflow(WorkflowStart ws,TModel model)
        {
        }

        protected virtual void AfterSave(TModel model)
        {
        }

        protected virtual void BeforeSave(TModel model)
        {
        }

        [HttpGet]
        public virtual Result<TModel> GetAll()
        {
            try
            {
                List<TModel> models;
                using (var db = new MyContext())
                {
                    var list = db.Set<TModel>();
                    models = list.ToList();
                }

                return new Result<TModel> {Grid = models, Type = ResultType.Success};
            }
            catch (Exception e)
            {
                return new Result<TModel> {Message = e.Message, Type = ResultType.Fail};
            }
        }

        [HttpGet]
        public virtual Result<TModel> GetById(long id)
        {
            try
            {
                using (var db = new MyContext())
                {
                    var list = db.Set<TModel>();

                    var model = list.Find(id);

                    return new Result<TModel> {SingleRecord = model, Type = ResultType.Success};
                }
            }
            catch (Exception e)
            {
                return new Result<TModel> {Message = e.Message, Type = ResultType.Fail};
            }
        }

        [HttpPost, ActionName("Delete")]
        public virtual Result<VoidResult> Delete(long id)
        {
            try
            {
                using (var db = new MyContext())
                {
                    if (id == 0)
                    {
                        throw new Exception("این رکورد موجود نیست و صفر ارسال شده است");
                    }
                    else
                    {
                        var list = db.Set<TModel>();
                        var record = list.Find(id);
                        if (record == null)
                            throw new Exception("این رکورد موجود نیست");

                        db.Entry(record).State = EntityState.Deleted;
                    }

                    db.SaveChanges();
                }

                return new Result<VoidResult> {Type = ResultType.Success};
            }
            catch (Exception e)
            {
                return new Result<VoidResult> {Message = e.Message, Type = ResultType.Fail};
            }
        }


        [HttpPost]
        public virtual Result<VoidResult> Save(TModel model)
        {
            try
            {
                using (var db = new MyContext())
                {
                    var list = db.Set<TModel>();

                    if (model.Id == 0)
                    {
                        list.Add(model);
                    }
                    else
                    {
                        var record = list.Find(model.Id);
                        if (record == null)
                            throw new Exception("این رکورد موجود نیست");

                        db.Entry(record).CurrentValues.SetValues(model);
                        db.Entry(record).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                }

                return new Result<VoidResult> {Type = ResultType.Success};
            }
            catch (Exception e)
            {
                return new Result<VoidResult> {Message = e.Message, Type = ResultType.Fail};
            }
        }
    }


    public class Result<T> : VoidResult
    {
        public List<T> Grid { get; set; }
        public T SingleRecord { get; set; }
    }

    public class VoidResult
    {
        public string Message { get; set; }
        public ResultType Type { get; set; }
    }

    public enum ResultType
    {
        Success,
        Fail
    }
}