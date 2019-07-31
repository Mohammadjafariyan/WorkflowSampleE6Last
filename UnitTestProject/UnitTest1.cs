using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.Models;
using WorkflowSample.Service.Models.WorkflowEngineModels;

namespace Tests
{

    [TestClass]
    public class Tests
    {
        private WorkflowService workflowService;
        private WorkflowStart model;
        private WorkflowContinueRequest cont_wrongTaskId;
        private WorkflowContinueRequest cont_notClaimed;
        private WorkflowContinueRequest cont_forClaim;
        private WorkflowContinueRequest cont_wrongUsername;
        private WorkflowContinueRequest cont_notClaimedBefore;
        private WorkflowContinueRequest cont;
        private ProcessDefinitionsRequest pdr;
        private EngineInboxTaskRequest vm;
        private QueryHistoricProcessInstanceRequest _queryHistoricProcessInstanceRequest;


        public Tests()
        {
            Initialize();
        }

        public void Initialize()
        {
            Setup();
            Setup2();
            Setup3();
            Setup4();
            Setup5();
            Setup6();
            Setup7();
            Setup9();
        }

        public void Setup7()
        {
            cont_notClaimedBefore = new WorkflowContinueRequest();
            cont_notClaimedBefore.Username = "noori";
            cont_notClaimedBefore.UpperUsername = "amin";
            cont_notClaimedBefore.variables.Add("upper", "amin");
            cont_notClaimedBefore.TaskId = new Random().Next().ToString();
            cont_notClaimedBefore.RequestStatus = WorkflowContinueRequestStatus.Complete;
        }


        
        public void Setup6()
        {
            cont_forClaim = new WorkflowContinueRequest();
            cont_forClaim.Username = "noori";
            cont_forClaim.UpperUsername = "amin";
            cont_forClaim.variables.Add("upper", "amin");
            cont_forClaim.TaskId = new Random().Next().ToString();
            cont_forClaim.RequestStatus = WorkflowContinueRequestStatus.Claim;
        }

        
        public void Setup5()
        {
            cont_notClaimed = new WorkflowContinueRequest();
            cont_notClaimed.Username = "noori";
            cont_notClaimed.UpperUsername = "amin";
            cont_notClaimed.variables.Add("upper", "amin");
            cont_notClaimed.TaskId = new Random().Next().ToString();
            cont_notClaimed.RequestStatus = WorkflowContinueRequestStatus.Complete;
        }

        
        public void Setup4()
        {
            cont_wrongUsername = new WorkflowContinueRequest();
            cont_wrongUsername.Username = "noori2";
            cont_wrongUsername.UpperUsername = "amin";
            cont_wrongUsername.variables.Add("upper", "amin");
            cont_wrongUsername.TaskId = new Random().Next().ToString();
            cont_wrongUsername.RequestStatus = WorkflowContinueRequestStatus.Complete;
        }

        
        public void Setup2()
        {
            cont = new WorkflowContinueRequest();
            cont.Username = "noori";
            cont.UpperUsername = "amin";
            cont.variables.Add("upper", "amin");
            cont.TaskId = new Random().Next().ToString();
            cont.RequestStatus = WorkflowContinueRequestStatus.Complete;
        }

        
        public void Setup3()
        {
            cont_wrongTaskId = new WorkflowContinueRequest();
            cont_wrongTaskId.Username = "noori";
            cont_wrongTaskId.UpperUsername = "amin";
            cont_wrongTaskId.variables.Add("upper", "amin");
            cont_wrongTaskId.TaskId = new Random().Next().ToString();
            cont_wrongTaskId.RequestStatus = WorkflowContinueRequestStatus.Complete;
        }
        
        public void Setup9()
        {
            pdr = new ProcessDefinitionsRequest();
            
            vm= new EngineInboxTaskRequest
            {
                assignee = "noori"
            };
        }

        public void Setup()
        {
            workflowService = new WorkflowService();
            model = new WorkflowStart();
            model.ProcessDefinitionKey = "vacationRequest";
            model.Username = "mohammad";
            model.Id = new Random().Next().ToString();
            model.UpperUsername = "noori";
            model.variables.Add("vacationType", new Random().Next(1, 3).ToString());
            model.variables.Add("title", new Random().Next(1, 10).ToString());
            model.variables.Add("days", new Random().Next(1, 10).ToString());
            model.variables.Add("upper", "noori");
        }

        [TestMethod()]
        public async Task SendToWorkflowTest()
        {
            var resp = await workflowService.SendToWorkflow(model);

            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.Id!=0);
            Assert.IsTrue(resp.Status == EngineResponseStatus.Success);

           // Assert.Pass();
            // return await Task.FromResult(1);
        }


        
        public async Task setUpqueryHistoricProcessInstanceRequest()
        {
            var processDefinitions= await workflowService.GetProcessDefinitions(pdr);

            List<Variable> vars=new List<Variable>();
            vars.Add(new Variable{Name = "vacationType",Value = "3",Operation = "equals"});

            var vr= processDefinitions.Data.First(d => d.Key == "vacationRequest");
            
            _queryHistoricProcessInstanceRequest=new QueryHistoricProcessInstanceRequest
            {
                ProcessDefinitionId =  vr.Id,
                Variables =vars.ToArray(),
                
            };
        }

        [TestMethod()]
        public async Task QueryHistoricProcessInstancesTest()
        {
            var resp = await workflowService.QueryHistoricProcessInstances(_queryHistoricProcessInstanceRequest);
            
            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.Status==EngineResponseStatus.Success);
        }
        
        
        
        [TestMethod()]
        public async Task QueryHistoricTasksTest()
        {
            var resp = await workflowService.QueryHistoricTasks(_queryHistoricProcessInstanceRequest);


            _queryHistoricProcessInstanceRequest.Variables[0].Value = "2";
            resp = await workflowService.QueryHistoricTasks(_queryHistoricProcessInstanceRequest);

            
            _queryHistoricProcessInstanceRequest.Variables[0].Value = "3";
            resp = await workflowService.QueryHistoricTasks(_queryHistoricProcessInstanceRequest);

            
            _queryHistoricProcessInstanceRequest.Variables[0].Value = "3";
            _queryHistoricProcessInstanceRequest.Variables[0].Type = "Integer";
            resp = await workflowService.QueryHistoricTasks(_queryHistoricProcessInstanceRequest);

            
            _queryHistoricProcessInstanceRequest.Variables[0].Value = "3";
            _queryHistoricProcessInstanceRequest.Variables[0].Type = "integer";
            resp = await workflowService.QueryHistoricTasks(_queryHistoricProcessInstanceRequest);

            
            _queryHistoricProcessInstanceRequest.Variables[0].Name = "title";
            _queryHistoricProcessInstanceRequest.Variables[0].Value = "*";
            resp = await workflowService.QueryHistoricTasks(_queryHistoricProcessInstanceRequest);

            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.Status==EngineResponseStatus.Success);
        }
        [TestMethod()]
        public async Task ContinueTest_Inbox()
        {
            

            var resp = await workflowService.GetInboxTasks(vm);
        }

        [TestMethod()]
        public async Task ContinueTest_wrongTaskId()
        {
            try
            {
                var resp = await workflowService.Continue(cont_wrongTaskId);
                Assert.IsTrue(false);
            }
            catch (ActivitiEngineClientException e)
            {
                Console.WriteLine(e);
            }

            // return await Task.FromResult(1);
        }

        
        [TestMethod()]
        public async Task GetProcessDefinitionsTest()
        {
            var resp = await workflowService.GetProcessDefinitions( pdr);

            Assert.IsNotNull(resp);
            // return await Task.FromResult(1);
        }
        [TestMethod()]
        public async Task GetDiagramTest()
        {
           
               var tasks= await workflowService.GetInboxTasks(vm);

               if (tasks.Data.Length == 0)
               {
                   await this.SendToWorkflowTest();
               }
               
                tasks= await workflowService.GetInboxTasks(vm);

                var dr=new DiagramRequest();
                dr.processInstanceId = dr.processInstanceId = tasks.Data[0].ProcessInstanceId;
                
                var resp = await workflowService.GetDiagram(dr);
            

            // return await Task.FromResult(1);
        }
        [TestMethod()]
        public async Task ContinueTest_notClaimed()
        {
            try
            {
               
                var resp = await workflowService.Continue(cont_notClaimed);
                Assert.IsTrue(false);
            }
            catch (WorkflowServiceException e)
            {
                Console.WriteLine(e);
            }

            // return await Task.FromResult(1);
        }

        [TestMethod()]
        public async Task ContinueTest_wrongUsername()
        {
            try
            {
                
                 
                var vm = new EngineInboxTaskRequest
                {
                    assignee = "noori"
                };

                var resp1 = await workflowService.GetInboxTasks(vm);


                cont_wrongUsername.TaskId = resp1.Data[0].Id.ToString();
                var resp = await workflowService.Continue(cont_wrongUsername);
                Assert.IsTrue(false);
            }
            catch (WorkflowServiceException e)
            {
                Console.WriteLine(e);
            }

            // return await Task.FromResult(1);
        }

        [TestMethod()]
        public async Task ContinueTest()
        {
            var vm = new EngineInboxTaskRequest
            {
                assignee = "noori"
            };

            var resp1 = await workflowService.GetInboxTasks(vm);


            cont.TaskId = resp1.Data[0].Id.ToString();

            var resp = await workflowService.Continue(cont);


            // return await Task.FromResult(1);
        }
    }
}