using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using WorkflowSample.Service.Injector;

namespace Workflow
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private  IWindsorContainer _container;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        //    ConfigureWindsor();
        }
        public  void ConfigureWindsor()
        {

            _container = InjectorSingleTon.Container;
            _container.Install(FromAssembly.This());

            GlobalConfiguration.Configuration.DependencyResolver =
                new DependencyResolver(_container.Kernel);


            
        } 
        
        public override void Init()
        {
            this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(
                SessionStateBehavior.Required);
        }
        public override void Dispose()
        {
            this._container.Dispose();
            base.Dispose();
        }
    }
}
