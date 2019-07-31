using System;
using WorkflowSample.Service.ServiceTasks;

namespace WorkflowSample.Service
{
    public class WorkflowServiceTaskFactory
    {
        public static IWorkflowServiceTaskListener GetService(string ServiceTaskName)
        {
            switch (ServiceTaskName)
            {
                case nameof(SampleTestServiceTask):
                    return Injector.InjectorSingleTon.Inject<IWorkflowServiceTaskListener>(
                        nameof(SampleTestServiceTask));
                default:
                    throw new WorkflowServiceTaskFactoryException(
                        ServiceTaskName + "وب سرویس با نام درخواستی ثبت نشده است : ");
            }
        }

        public class WorkflowServiceTaskFactoryException : Exception
        {
            public WorkflowServiceTaskFactoryException(string empty) : base(empty)
            {
            }
        }
    }
}