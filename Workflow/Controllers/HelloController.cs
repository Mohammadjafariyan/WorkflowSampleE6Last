using System.Web.Http;

namespace Workflow.Controllers
{
    public class HelloController:ApiController
    {
        [HttpPost]
        public string GetString()
        {
            return "hi";
        }
    }
}