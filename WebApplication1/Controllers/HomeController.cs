using System;
using System.Diagnostics;
using System.Web.Mvc;
using NHibernate;
using WebApplication1.Code.Model;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly CustomerRepository _customerRepo;
        
        public HomeController(ISession session, CustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public ActionResult Index()
        {
            // At this point it's common for people to implement repositories over NHibernate.
            // It's not a good idea, but here's an example of the BAD CODE

            var customer = _customerRepo.Get(1);

            // This is sucky for several reasons.
            // 1. Useless indirection - You've just implemented the exact same features as were available on ISession
            // 2. Constructor explosion - All of a sudden, you need a repo per aggregate root, and often inject many of them at once
            // 3. You start to loose API features by abstracting away from the ORM - you end up implementing "save or update or saveorupdate" logic
            // 4. You realise that repositories are meant to be transaction boundaries and start implementing Flush()'s
            // 5. ... at this point, hindering NHibernates ability to do its job well.
            // 6. You end up writing loads of boilerplate repositories.
            // 7. You get angry and say NHibernate doesn't perform
            // 8. Actually it's just poor design.

            // People do this with the belief that "we can just migrate to another ORM trivially".
            // It's not trivial to migrate ORMS, you're not really protecting yourself, all you're doing
            // is restricting yourself to the subset of features of the ORM you're using that are common
            // with a hand rolled DAL.

            // The ISession is your repository. Use it as one.


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

    public interface IRepository<T>
    {
        T Get(int id);
        void Save(T obj);
    }

    public class CustomerRepository : IRepository<Customer>
    {
        private readonly ISession _session;

        public CustomerRepository(ISession session)
        {
            _session = session;
        }

        public Customer Get(int id)
        {
            return _session.Get<Customer>(id);
        }

        public void Save(Customer obj)
        {
            _session.SaveOrUpdate(obj);
            _session.Flush();
        }
    }

}