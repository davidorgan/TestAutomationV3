using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System.Drawing.Imaging;
using OpenQA.Selenium.Support.UI;

namespace TestFramework
{
    public class Wait
    {
        private const bool Debug = true;
        private const int ImplicitWaitInSeconds = 30;

        private readonly IWebDriver _driver;

        public Wait(IWebDriver webDriver)
        {
            _driver = webDriver;
        }

        public void Until(Func<IWebDriver, bool> waitCondition, int timeoutInSeconds = ImplicitWaitInSeconds)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(waitCondition);
        }

        public void UntilAnimationIsDone(string elementId, int timeoutInSeconds = ImplicitWaitInSeconds)
        {
            Until(driver =>
            {
                var javaScriptExecutor = (IJavaScriptExecutor)driver;
                var isAnimated = javaScriptExecutor
                    .ExecuteScript(string.Format("return $('#{0}').is(':animated')", elementId))
                    .ToString().ToLower();

                if (Debug)
                    Console.WriteLine(string.Format("Element: '{0}' Is Animated: {1}", elementId, isAnimated));

                // return true when finished animating
                return !bool.Parse(isAnimated);
            }, timeoutInSeconds);
        }
    }
}
