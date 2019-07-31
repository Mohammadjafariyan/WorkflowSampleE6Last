using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkflowSample.Service.Models;

namespace WorkflowSample.Service.Client
{
    public interface IActivitiEngineUserManagementService
    {
        Task<EngineUser> FindById(string id);
        Task CreateUserIfNotExist(EngineUser user);
    }

    public class ActivitiEngineUserManagementService : IActivitiEngineUserManagementService
    {
        public async Task<EngineUser> FindById(string id)
        {
            using (var httpClient = new HttpClient())
            {
                SetDefaultHeaders(httpClient);
                
                var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
                var url = $@"{baseUrl}/identity/users/${id}";
                var resp = await httpClient.GetAsync(url);
                if ((int) resp.StatusCode == 200)
                {
                    return await ActivitiEngineClientHelper.Parse<EngineUser>(resp);
                }
                else if ((int) resp.StatusCode == 404)
                {
                    return await Task.FromResult<EngineUser>(null);
                }
                else
                {
                    var errorxml = await resp.Content.ReadAsStringAsync();
                    throw new Exception(errorxml);
                }
            }
        }
        public async Task CreateUserIfNotExist(EngineUser user)
        {
            using (var httpClient = new HttpClient())
            {
                SetDefaultHeaders(httpClient);

                var record= await FindById(user.Id);
                //وجود دارد
                if (record != null)
                    return;
                
                
                var baseUrl = WorkflowSettingSingleTon.WorkflowSetting.BaseUrl;
                var url = $@"{baseUrl}/identity/users";
                var resp = await httpClient.GetAsync(url);
                if ((int) resp.StatusCode == 201)
                {
                }
                else if ((int) resp.StatusCode == 401)
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

        private void SetDefaultHeaders(HttpClient httpClient)
        {

            var setting= WorkflowSettingSingleTon.GetAdministrator();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(
                            $"{setting.Username}:{setting.Password}")));
        }
    }
}