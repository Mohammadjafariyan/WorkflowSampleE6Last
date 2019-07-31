using Castle.Windsor;
using Castle.Windsor.Installer;
using WorkflowSample.Service.Injector;

namespace WorkflowSample.Service.Injector
{
    public class InjectorSingleTon
    {
        private static WindsorContainer _container;


        public static T Inject<T>(string name)
        {
            return Container.Resolve<T>(name);
        }
        
        public static T Inject<T>()
        {
            return Container.Resolve<T>();
        }
        
        public static WindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    Init();
                }

                return _container;
            }
            set {  _container = value; }
        }

        private static void Init()
        {
            // application starts...
            _container = new WindsorContainer();

            _container.Install(
                new RepositoriesInstaller()
                // and all your other installers
            );
        }
    }
}