using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace TestFramework
{
    public abstract class PageObject
    {
        internal static string PageTitle { get; set; }
        internal static string Url { get; set; }

        protected PageObject()
        {
            Url = EnvironmentReader.get(GetType().Name).Url;
            PageTitle = EnvironmentReader.get(GetType().Name).PageTitle;
        }

        internal void WaitForLoad()
        {
            var wait = Browser.Wait();

            try
            {
                wait.Until(p => p.Title == PageTitle);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TakeScreenshot();
                throw;
            }

        }

        public bool IsAt()
        {
            return Browser.Title.Contains(PageTitle);
        }

        public void Goto()
        {
            try
            {
                Browser.Goto(Url);
                WaitForLoad();
            }
            catch (Exception e)
            {
                throw new Exception(GetType().Name + " could not be loaded. Check page url is correct in app.config." +
                                    " " + e.Message);
            }
        }
    }
}
