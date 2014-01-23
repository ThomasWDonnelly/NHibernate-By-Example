using System;
using System.Web.Mvc;
using NHibernate;
using WebApplication1.Code.Model;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession _session;

        public HomeController(ISession session)
        {
            _session = session;
        }

        public ActionResult Index()
        {
            // Truth is, this makes your code really messy, and what you really want is to follow the
            // session per request pattern - so lets get rid of all this tedious transaction code 
            // and move it into our framework classes.

            var customer = _session.Get<Customer>(1);
            customer.Name = "NameGoesHere_" + Guid.NewGuid();

            // If we want this to work, we'll have to add a few more things to our container registrations

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
}