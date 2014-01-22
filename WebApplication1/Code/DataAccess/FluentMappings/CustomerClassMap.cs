using FluentNHibernate.Mapping;
using WebApplication1.Code.Model;

namespace WebApplication1.Code.DataAccess.FluentMappings
{
    /// <summary>
    /// This is the simplest possible mapping.
    /// It'll work because FluentNHibernate uses reasonably sane defaults.
    /// </summary>
    public class CustomerClassMap : ClassMap<Customer>
    {
        public CustomerClassMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Gender);
        }
    }
}