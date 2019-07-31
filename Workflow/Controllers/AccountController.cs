using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using WorkflowSample.Service;
using WorkflowSample.Service.Injector;
using WorkflowSampleE6Last.Models;

namespace Workflow.Controllers
{
    public class AccountController : GenericController<Account>
    {
        public override Result<Account> GetAll()
        {
            try
            {
                using (var db = new MyContext())
                {
                    var list = db.Accounts.ToList();
                    var models = list.Select(a => new Account
                    {
                        Id = a.Id, Username = a.Username, ParentId = a.ParentId,
                        UpperUsername = a.ParentId.HasValue ? a.Parent.Username : "",
                    }).ToList();
                    return new Result<Account> {Grid = models, Type = ResultType.Success};
                }
            }
            catch (Exception e)
            {
                return new Result<Account> {Message = e.Message, Type = ResultType.Fail};
            }
        }

        [HttpGet]
        public VoidResult Login(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    throw new Exception("نام کاربری ارسالی خالی است");
                }

                using (var db = new MyContext())
                {
                    var find = db.Accounts.FirstOrDefault(a => a.Username == username);
                    if (find == null)
                        throw new Exception("نام کاربری یافت نشد");

                    HttpContext.Current.Session["loggedInUsername"] = username;


                    return new VoidResult
                    {
                        Type = ResultType.Success,
                    };
                }
            }
            catch (Exception e)
            {
                return new VoidResult
                {
                    Type = ResultType.Fail,
                    Message = e.Message
                };
            }
        }
    }
}