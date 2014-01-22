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
            // There is a possible gotcha here though. Make sure you remember to commit your transaction...

            using (var tx = _session.BeginTransaction())
            {
                var customer = _session.Get<Customer>(1);
                customer.Name = "NameGoesHere_" + Guid.NewGuid();

                //tx.Commit();
            }

            // Notice how without that commit, your changes aren't saved.

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