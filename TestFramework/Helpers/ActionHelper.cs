using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Drawing.Imaging;
using OpenQA.Selenium.Support.UI;
using System.Data.SqlClient;
using OpenQA.Selenium.Interactions;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Remote;

namespace TestFramework
{
    public static class ActionHelper
    {
       /* public abstract TResult ExecuteWithExceptionHandling<TParam, TResult>(TParam parameter, Func<TParam, TResult> func)
        {
            int attempts = 0;
            while (attempts < 2)
            {
                try
                {
                    return func(parameter);
                }
                catch (StaleElementReferenceException e)
                {
                }
                attempts++;
            }
            return default(TResult);
        }*/

        public static bool retryingFindClick(IWebElement element)
        {
            bool result = false;
            int attempts = 0;
            while (attempts < 2)
            {
                try
                {
                    element.Click();
                    result = true;
                    break;
                }
                catch (StaleElementReferenceException e)
                {
                }
                attempts++;
            }
            return result;
        }
    }
}
