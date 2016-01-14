using System;
using OpenQA.Selenium;

namespace TestFramework
{
    public class DeleteAccountPOM : BasePageObject
    {
        public DeleteAccountPOM() { }
        public DeleteAccountPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        ///--Web Page Elements--///
        //Delete Account Elements
        IWebElement deleteAccountConfirm_Checkbox { get { return driver.FindElement(By.XPath("//*[@id='HasConfirmedDataDeletion']")); } }
        IWebElement deleteAccountSubmit_Button { get { return driver.FindElement(By.XPath("//*[@id='content-page']/div[2]/form/div[2]/div/input")); } }
        IWebElement deleteAccountBack_Button { get { return driver.FindElement(By.LinkText("Back")); } }

        public void tickUnderstand()
        {
            try
            {
                deleteAccountConfirm_Checkbox.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void submitUDeleteAccount()
        {
            try
            {
                deleteAccountSubmit_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void backOutDelete()
        {
            try
            {
                deleteAccountBack_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //Nav Menu "GoTo" links
        public void goToDeleteAccount()
        {
            //Javascript Click overide used for nav dropdown menu items
            /*IWebElement deleteLink = driver.FindElement(By.XPath("//a[contains(@href, '/Account/Delete')]"));
            JavaScriptClick(deleteLink);*/
            Goto(Settings.Default.BaseURL + "Account/Delete");
        }

    }
}
