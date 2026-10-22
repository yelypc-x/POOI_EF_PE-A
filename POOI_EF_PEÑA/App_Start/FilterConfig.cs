using System.Web;
using System.Web.Mvc;

namespace POOI_EF_PEÑA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
