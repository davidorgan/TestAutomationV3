using System;
using OpenQA.Selenium;

namespace TestFramework.Helpers
{
    class FileUploadSelector
    {
        public static void FileSelector(string xpath, string filePath, IWebDriver driver)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            String script = "document.getElementByXPath('" + xpath + "').value='" + filePath + "';";
            executor.ExecuteScript(script);
        }
    }
}
