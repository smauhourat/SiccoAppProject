using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace SiccoApp.Helpers
{
    public static class MvcExtensions
    {
        public static RouteValueDictionary ToRouteValues(this NameValueCollection col, Object obj = null)
        {
            var values = obj != null ? new RouteValueDictionary(obj) : new RouteValueDictionary();

            if (col == null) return values;

            foreach (string key in col)
            {
                //values passed in object are already in collection
                if (!values.ContainsKey(key)) values[key] = col[key];
            }
            return values;
        }
    }
}
