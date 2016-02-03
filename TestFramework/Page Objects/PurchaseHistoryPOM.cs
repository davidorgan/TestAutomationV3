using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestFramework
{
    public class PurchaseHistoryPOM : BasePageObject
    {
        public PurchaseHistoryPOM() { }
        public PurchaseHistoryPOM(IWebDriver driver)
        {
            this.driver = driver;
            this.currentAccount = AccountHelper.accountDetails.setAccountDetails();
        }

        ///--Web Page Elements--///
        //Purchase History Elements
        IWebElement purchaseHistoryFromDate_InputField { get { return driver.FindElement(By.Id("PurchaseHistoryFrom")); } }
        IWebElement purchaseHistoryToDate_InputField { get { return driver.FindElement(By.Id("PurchaseHistoryTo")); } }
        IWebElement purchaseHistoryVIN_Dropdown { get { return driver.FindElement(By.Id("VehicleIdentificationNumber")); } } 
        IWebElement purchaseHistorySubmit_Button { get { return driver.FindElement(By.XPath("//input[@value='View']")); } }
        IWebElement purchaseHistoryEdit_Button { get { return driver.FindElement(By.LinkText("Edit Details")); } }
        IWebElement purchaseHistoryFromError_Span { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[2]/div/div/span")); } }
        IWebElement purchaseHistoryToError_Span { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[3]/div/div/span")); } }
        IWebElement purchaseHistory_Link { get { return driver.FindElement(By.XPath("//a[contains(@href, '/Account/PurchaseHistory')]")); } }

        IWebElement purchaseHistoryDashboard_Link { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[4]/div/a")); } }
        IWebElement purchaseHistoryBack_Link { get { return driver.FindElement(By.XPath("//*[@id='purchase-history']/div[2]/div/a")); } }

        //Purchase History Details Elements
        
        IWebElement purchaseHistoryFirstInvoice_Link { get { return driver.FindElement(By.XPath("//*[@id='purchase-history']/div[1]/div/div/div[1]/div[4]/div[1]/div/a")); } }
        IWebElement purchaseHistoryFirstDate_Span { get { return driver.FindElement(By.XPath("//*[@id='purchase-history']/div[1]/div/div/div[1]/div[4]/div[1]/div/span[1]")); } }
        //*[@id="purchase-history"]/div[1]/div/div/div[1]/div[4]/div[1]/div/span[1]
        IWebElement purchaseHistoryFirstPlanName_Span { get { return driver.FindElement(By.XPath("//*[@id='purchase-history']/div[1]/div/div/div[1]/div[4]/div[1]/div/span[2]")); } }
        IWebElement purchaseHistoryFirstPurchasedBy_Span { get { return driver.FindElement(By.XPath("//*[@id='purchase-history']/div[1]/div/div/div[1]/div[4]/div[1]/div/span[3]")); } }
        IWebElement purchaseHistoryFirstPrice_Span { get { return driver.FindElement(By.XPath("//*[@id='purchase-history']/div[1]/div/div/div[1]/div[4]/div[1]/div/span[4]")); } }

        public void enterPurchaseHistory(String fromDate, String toDate)
        {
            try
            {
                enterPurchaseHistoryFrom(fromDate);
                enterPurchaseHistoryTo(toDate);
                //selectVIN(Settings.Default.CurrentVIN);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void enterPurchaseHistoryFrom(String fromDate)
        {
            purchaseHistoryFromDate_InputField.Click();
            purchaseHistoryFromDate_InputField.Clear();
            purchaseHistoryFromDate_InputField.SendKeys(fromDate);
        }

        private void enterPurchaseHistoryTo(String toDate)
        {
            purchaseHistoryToDate_InputField.Click();
            purchaseHistoryToDate_InputField.Clear();
            purchaseHistoryToDate_InputField.SendKeys(toDate);
        }

        private void selectVIN(String VIN)
        {
            new SelectElement(purchaseHistoryVIN_Dropdown).SelectByText(VIN);
        }

        public void submitPurchaseHistory()
        {
            try
            {
                purchaseHistorySubmit_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void editDetailsPurchaseHistory()
        {
            try
            {
                purchaseHistoryEdit_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public bool assertFromDateErrorPurchaseHistory()
        {
            IWebElement fromValidation = purchaseHistoryFromError_Span;
            string fromValidationstring = fromValidation.Text;
            if (fromValidationstring.Equals(Settings.Default.FromDateErrorPurchaseHistory))
            {
                return true;
            }
            throw new Exception("The from date validation message is not showing the expected text.");
        }

        public bool assertToDateErrorPurchaseHistory()
        {
            IWebElement toValidation = purchaseHistoryToError_Span;
            string toValidationstring = toValidation.Text;
            if (toValidationstring.Equals(Settings.Default.ToDateErrorPurchaseHistory))
            {
                return true;
            }
            throw new Exception("The to date validation message is not showing the expected text.");
        }

        public void goToFirstInvoice()
        {
            purchaseHistoryFirstInvoice_Link.Click();
        }

        /// <summary>
        /// Asserts the purchase history details row.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">The Purchase History details are not displayed as expected.</exception>
        public bool assertPurchaseHistoryDetailsRow()
        {
            string date = purchaseHistoryFirstDate_Span.Text;
            string planName = purchaseHistoryFirstPlanName_Span.Text;
            string purchasedBy = purchaseHistoryFirstPurchasedBy_Span.Text;
            string price = purchaseHistoryFirstPrice_Span.Text;
            string invoiceText = purchaseHistoryFirstInvoice_Link.Text;

            if (!isValidDate(date))
            {
                throw new Exception("Expected Date format not displayed: " + date);
            }

            if (!planName.Equals(Settings.Default.AccountLocale + " 1 Day 50MB") && !planName.Equals("EU Roaming 1 Day 50MB") && !planName.Equals("Europe Border 1 Day 50MB"))
            {
                throw new Exception("Expected Plan Name not displayed. "+ planName +" displayed instead");
            }

            if (!purchasedBy.Contains(getCurrentFullName()))
            {
                throw new Exception("Expected Name not displayed.");
            }

            if (!IsValidCurrency(price))
            {
                throw new Exception("Price not displayed correctly.");
            }

            if (!invoiceText.Equals("Invoice"))
            {
                throw new Exception("Invoice Link Text not displayed correctly.");
            }

            return true;
        }


        //Nav Menu "GoTo" links
        public void goToPurchaseHistory()
        {
            //driver.Navigate().GoToUrl(Settings.Default.BaseURL + "Account/PurchaseHistory");
            //Javascript Click overide used for nav dropdown menu items
            IWebElement Link = purchaseHistory_Link;
            JavaScriptClick(Link);
        }
        
    }
}
