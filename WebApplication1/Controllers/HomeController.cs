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
            // This is how you're actually meant to modify data...

            using (var tx = _session.BeginTransaction())
            {
                var customer = _session.Get<Customer>(1);
                customer.Name = "NameGoesHere_" + Guid.NewGuid();

                tx.Commit();
            }

            // Notice how that's much less code.
            // Because we "got" the entity from the session, NHibernates change tracking knows about the
            // object and is tracking its state.
            // When the transaction is committed, everything is cleared out and saved down.

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