using System.Data;
using System.Diagnostics;
using NHibernate;
using WebApplication1.Code.DataAccess;

[assembly: WebActivator.PreApplicationStartMethod(typeof(WebApplication1.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(WebApplication1.App_Start.NinjectWebCommon), "Stop")]

namespace WebApplication1.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        public static IKernel Kernel { get; set; }
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            var kernel = CreateKernel();
            bootstrapper.Initialize(()=>kernel);

            Kernel = kernel;
        }


        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISession>().ToMethod(x =>
            {
                var session = SessionProvider.Instance.Value.OpenSession();
                // We'll now start a transaction when the request begins
                session.BeginTransaction(IsolationLevel.ReadCommitted);
                return session;
            })
            .InRequestScope()
            .OnDeactivation(x =>
            {
                // Using Ninject, because it's awesome, we can hook on "deactivation.
                // And we'll close the active transaction on the request end.
                if (x.Transaction != null && x.Transaction.IsActive)
                {
                    x.Transaction.Commit();
                    x.Transaction.Dispose();
                }

                Debug.WriteLine("Now we can forget about transactions in our code!");
            });

        }        
    }
}
