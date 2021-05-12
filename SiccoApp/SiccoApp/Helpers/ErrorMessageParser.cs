using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiccoApp.Helpers
{
    public static class ErrorMessageParser
    {

        public static string Parse(string value)
        {
            if (value == "SP_MSG_EXIST_DOCUMENTATION")
                return Resources.Resources.SP_MSG_EXIST_DOCUMENTATION;
            if (value == "SP_MSG_EXIST_REQUIREMENT")
                return Resources.Resources.SP_MSG_EXIST_REQUIREMENT;
            return value;

        }

    }
}