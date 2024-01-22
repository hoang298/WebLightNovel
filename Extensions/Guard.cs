using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebLightNovel.Extensions
{
    public class Guard
    {
        public static void NotNull(object arg, string argName)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }
    }
}