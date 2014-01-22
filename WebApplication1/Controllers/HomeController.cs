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
            // So we want to modify some data?
            // This is how you might think you should do it...

            
            var customer = _session.Get<Customer>(1);
            customer.Name = "NameGoesHere_" + Guid.NewGuid();
            _session.Save(customer);
            _session.Flush();

            // Hey look! That worked.
            // Except it's not what you're meant to do.

            // Flush:

            /* If you happen to be using the ITransaction API, you don't need to worry about this step. 
             * It will be performed implicitly when the transaction is committed. 
             * Otherwise you should call ISession.Flush() to ensure that all changes are synchronized with the database.
             */

            // Flush basically just round-trips to the database, and actually, nhibernate is way smarter than that.
            // Actually, if you use a transaction, nhibernate better optimises it's round-tripping, and you
            // should never call flush manually.

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