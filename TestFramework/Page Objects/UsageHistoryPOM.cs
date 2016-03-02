using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestFramework
{
    public class UsageHistoryPOM : BasePageObject
    {
        public UsageHistoryPOM() {  }

        public UsageHistoryPOM(IWebDriver driver)
        {
            this.driver = driver;
            this.currentAccount = AccountHelper.accountDetails.setAccountDetails();
        }

        ///--Web Page Elements--///
        //Usage History Elements
        private IWebElement usageHistoryFromDate_InputField { get { return driver.FindElement(By.Id("UsageHistoryFrom")); } }

        private IWebElement usageHistoryToDate_InputField { get { return driver.FindElement(By.Id("UsageHistoryTo")); } }
        private IWebElement usageHistoryVIN_Dropdown { get { return driver.FindElement(By.Id("VehicleIdentificationNumber")); } }
        private IWebElement usageHistorySubmit_Button { get { return driver.FindElement(By.XPath("//input[@value='View']")); } }
        private IWebElement usageHistoryFromError_Span { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[2]/div/div/span")); } }
        private IWebElement usageHistoryToError_Span { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[3]/div/div/span")); } }
        private IWebElement usageHistory_Link { get { return driver.FindElement(By.XPath("//a[contains(@href, '/Account/UsageHistory')]")); } }

        private IWebElement usageHistoryDashboard_Link { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[4]/div/a")); } }
        private IWebElement usageHistoryBack_Link { get { return driver.FindElement(By.XPath("//*[@id='usage-history']/div/div/div[2]/div/a")); } }

        public void enterCheckUsageDates(String fromDate, String toDate)
        {
            try
            {
                usageHistoryFromDate_InputField.Click();
                usageHistoryFromDate_InputField.Clear();
                usageHistoryFromDate_InputField.SendKeys(fromDate);
                usageHistoryToDate_InputField.Click();
                usageHistoryToDate_InputField.Clear();
                usageHistoryToDate_InputField.SendKeys(toDate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void submitCheckUsage()
        {
            try
            {
                usageHistorySubmit_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void selectVIN(String VIN)
        {
            new SelectElement(usageHistoryVIN_Dropdown).SelectByText(VIN);
        }


        public bool assertFromDateErrorUsageHistory()
        {

            IWebElement fromValidation = usageHistoryFromError_Span;
            string fromValidationstring = fromValidation.Text;
            if (fromValidationstring.Contains(Settings.Default.FromDateErrorUsageHistory))
            {
                return true;
            }
            throw new Exception(fromValidationstring + " - This does not match the expected text of: '"+Settings.Default.FromDateErrorUsageHistory+"'");
        }

        public bool assertToDateErrorUsageHistory()
        {

            IWebElement toValidation = usageHistoryToError_Span;
            string toValidationstring = toValidation.Text;
            if (toValidationstring.Equals(Settings.Default.ToDateErrorUsageHistory))
            {
                return true;
            }
            throw new Exception(toValidationstring + " - This does not match the expected text of: '" + Settings.Default.ToDateErrorUsageHistory + "'");
        }

        //Nav Menu "GoTo" links
        public void goToCheckUsage()
        {
            //Javascript Click overide used for nav dropdown menu items
            //IWebElement usageLink = driver.FindElement(By.XPath("//a[contains(@href, '/Account/UsageHistory')]"));
            //JavaScriptClick(usageLink);
            Goto(Settings.Default.BaseURL + "Account/UsageHistory");
        }

        //Assert
        public bool assertUsageHistoryDisplayed(string from, string to)
        {
            string expected = "From "+from+" - "+to+":";
            string body = driver.FindElement(By.TagName("body")).Text;
            if(body.Contains(expected))
            {
                return true;
            }
            expected = Settings.Default.NoUsageHistoryText;
            if (body.Contains(expected))
            {
                return true;
            }
            throw new Exception("Expected text not displayed for usage history.");
        }
        
    }
}
