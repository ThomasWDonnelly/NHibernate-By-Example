using System;
using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Utils;
using NHibernate;

namespace WebApplication1.Code.DataAccess
{
    /// <summary>
    /// This is the session provider for a single tenant application.
    /// You should have one of these for the entire lifecycle of your app.
    /// Here's one I made earlier!
    /// 
    /// This fixes the obvious bug: "newing up a new session factory each request is slow"
    /// 
    /// Scoping!
    /// 
    /// There's lots of information about storing nhibernate sessions in HTTP Context and the like
    /// but today? Just use your container.
    /// 
    /// You want to bind Instance.OpenSession() in your container "per request"
    /// </summary>
    public class SessionProvider
    {
        public static readonly Lazy<ISessionFactory> Instance = new Lazy<ISessionFactory>(CreateSessionFactory);

        private static ISessionFactory CreateSessionFactory()
        {
            var cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

            return
                Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(cs))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionProvider>())
                    .ExposeConfiguration(config =>
                    {
                        // We're forcefully adding a prefix to each of the class mappings to match our database here
                        // We can alternatively use Table("name"); in each classmap, or add a table naming convention.
                        config.ClassMappings.Each(m => m.Table.Name = "tbl_" + m.Table.Name);
                    })
                    .BuildSessionFactory();
        }
    }
}