using System;
using System.Diagnostics;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
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
            // Back to our original sample, the more astute of you will start to think about queries.
            // Given the general advice of "don't use repositories" with NHibernate, you'll realise that
            // organising queries logically could be a problem.

            // Before we get into that, lets look at the most popular ways to query with NHibernate...

            // First lets take a look at query over...

            var results =
                _session.QueryOver<Customer>()
                    .WhereRestrictionOn(x => x.Name)
                    .IsLike("Rich", MatchMode.Anywhere)
                    .List<Customer>();

            // You'll notice that QueryOver is not a Linq provider. This is often a source of confusion and frustration
            // as people expect things like this to work:

            try
            {
                var throwsAnException = _session.QueryOver<Customer>().Where(x => x.Name.StartsWith("Rich")).List<Customer>();
            } catch { }

            // While this compiles, and uses IQueryable expressions, most of the operations are not implemented.
            // It's especially confusing, because very simple operations do work, such as this:

            var worksBecauseItsSimple =
                _session.QueryOver<Customer>().Where(x => x.Name == "Richard Whitney").List<Customer>();

            // Unfortunately, if you don't know what is and isn't implemented, you'll get caught out and it's not obvious
            // which operations are happening in app memory, and which are running on the Db.
            // Avoid using straight Linq for all but the simplest of queries.

            // Then there's the classic Criteria API - this was the "original" query syntax of N/Hibernate, and it's
            // what query-over generates under the hood with the help of a few expression trees.

            var classic = _session.CreateCriteria<Customer>()
                .Add(Restrictions.Eq("Name", "Richard Whitney"))
                .List<Customer>();

            // It's reliable, but the syntax is full of magic strings, and you should do your best to 
            // ALWAYS prefer the QueryOver<> API.
            

            // Then there's HQL. Don't use it. It's powerful. Don't use it. 
            // We'll come back later when dealing with "advanced queries"



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