using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nutshell.Test.Support
{
    public static class ArgumentExceptionHelper
    {
        public static void CheckArgumentNullException(string parameterName, Action action)
        {
            try
            {
                action();
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual(parameterName, ex.ParamName, ex.Message);
                return;
            }
            Assert.Fail("{0} not throw ArgumentNullException!", parameterName);
        }

        public static void CheckArgumentException(string parameterName, Action action)
        {
            try
            {
                action();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(parameterName, ex.ParamName, ex.Message);
                return;
            }
            Assert.Fail("{0} not throw ArgumentException!", parameterName);
        }
    }
}
