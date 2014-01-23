using System;
using System.Diagnostics;
using System.Web.Mvc;
using NHibernate;
using WebApplication1.Code.Model;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession _session;
        private readonly DataAccess1 _da1;
        private readonly DataAccess2 _da2;

        public HomeController(ISession session, DataAccess1 da1, DataAccess2 da2)
        {
            _session = session;
            _da1 = da1;
            _da2 = da2;
        }

        public ActionResult Index()
        {
            // Now lets talk a little about the nhibernate cache.
            // Because we're using session per request, the exact same session
            // gets injected into all your various classes.

            // Check this out!

            // We're going to inject two classes, that both have an issession, and get the same object, renaming in one.
            // We know we're still in the same transaction boundary, so what do we expect to see?

            var customerThatWasRenamed = _da1.GetAndRename();
            var customerThatWasJustReturned = _da2.JustGet();

            if (customerThatWasRenamed.Name == customerThatWasJustReturned.Name)
            {
                Debug.WriteLine("This is clearly the same object, because the ISession instance has it's own internal cache");
            }

            // You're now starting to see the power of the ISession and Session Per Request
            // You can cleverly segment your code functionality, while not worrying about various classes
            // "round tripping" to the database - because NHibernates change tracking is smart enough to make sure
            // you get the right object inside a transaction boundary.

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class DataAccess1
    {
        private readonly ISession _session;

        public DataAccess1(ISession session)
        {
            _session = session;
        }

        public Customer GetAndRename()
        {
            var customer = _session.Get<Customer>(1);
            customer.Name = "NameGoesHere_" + Guid.NewGuid();
            return customer;
        }
    }

    public class DataAccess2
    {
        private readonly ISession _session;

        public DataAccess2(ISession session)
        {
            _session = session;
        }

        public Customer JustGet()
        {
            return _session.Get<Customer>(1);
        }
    }
}