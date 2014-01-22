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

            // At this point, our model won't load from the database because we've mapped it as an enum
            // while it's stored as an integer.
            // To prevent this nasty bug, we need to map it as it's specific type...

            Map(x => x.Gender).CustomType<Gender>();
        }
    }
}