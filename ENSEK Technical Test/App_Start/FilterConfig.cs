﻿using System.Web;
using System.Web.Mvc;

namespace ENSEK_Technical_Test
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
