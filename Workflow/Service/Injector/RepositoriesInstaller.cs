using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using WorkflowSample.Service.Client;
using WorkflowSample.Service.ServiceTasks;

namespace WorkflowSample.Service.Injector
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            /*container.Install(
         //       FromAssembly.Named("WorkflowSample.Service"),
                FromAssembly.Containing<IWorkflowServiceTaskListener>()
            );*/

        
            
            container.Register(Castle.MicroKernel
                .Registration.Component
                .For<IWorkflowServiceTaskListener>()
                .ImplementedBy<SampleTestServiceTask>().Named(nameof(SampleTestServiceTask)));

            container.Register(Castle.MicroKernel
                .Registration.Component
                .For<IWorkflowEngineClient>()
                .ImplementedBy<ActivitiEngineClient>().Named(nameof(ActivitiEngineClient)));

            #region WorkflowService

            container.Register(Castle.MicroKernel
                .Registration.Component
                .For<IWorkflowService>()
                .ImplementedBy<WorkflowService>().Named(nameof(WorkflowService)));

            #endregion
            
            
            container.Register(Castle.MicroKernel
                .Registration.Component
                .For<IActivitiEngineUserManagementService>()
                .ImplementedBy<ActivitiEngineUserManagementService>().Named(nameof(ActivitiEngineUserManagementService)));

            container.Register(Castle.MicroKernel
                .Registration.Component
                .For<ILoggedUserProviderService>()
                .ImplementedBy<LoggedUserProviderService>().Named(nameof(LoggedUserProviderService)));

            
            
        }
    }
}