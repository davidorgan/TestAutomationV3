using System;
using OpenQA.Selenium;

namespace TestFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class CreditCardManagementPOM : BasePageObject
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardManagementPOM"/> class.
        /// </summary>
        public CreditCardManagementPOM() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardManagementPOM"/> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public CreditCardManagementPOM(IWebDriver driver)
        {
            this.driver = driver;
            this.currentAccount = AccountHelper.accountDetails.setAccountDetails();
        }

        ///--Web Page Elements--///
        //Create Account Elements
        IWebElement currentCard_Container { get { return driver.FindElement(By.XPath("//*[@id='creditCardManagement']/div[2]/div")); } }
        IWebElement deleteCard_Img { get { return driver.FindElement(By.XPath("//*[@id='creditCardManagement']/div[2]/div/div/div[2]/div/div[2]/a/img")); } }
        IWebElement sponsorWarningPopUpClose_Button { get { return driver.FindElement(By.Id("close-error-button")); } }
        IWebElement sponsorWarningPopUpMessage_Container { get { return driver.FindElement(By.Id("errors-summary")); } }


        //Nav Menu "GoTo" links
        /// <summary>
        /// Goes to credit card management.
        /// </summary>
        public void goToCreditCardManagement()
        {
            Goto(Settings.Default.BaseURL + "Account/CreditCardManagement");
        }

        //Do things
        /// <summary>
        /// Asserts the credit card is present.
        /// </summary>
        /// <param name="expected">The expected credit cardto be displayed.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">There was an issue finding the credit card details</exception>
        public bool assertCreditCardIsPresent(string expected)
        {
            string currentCCString = currentCard_Container.Text;
            if (currentCCString.Contains(expected))
            {
                return true;
            }
            throw new Exception("There was an issue finding the credit card details");
        }

        /// <summary>
        /// Asserts the credit card is not present.
        /// </summary>
        /// <param name="expected">The expected credit card to NOT be displayed.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">The expected text was not shown for user with no Credit Card on the account.</exception>
        public bool assertCreditCardIsNotPresent(string expected)
        {
            IWebElement noCard = currentCard_Container;
            string noCardString = noCard.Text;
            if (noCardString.Contains(expected))
            {
                return true;
            }
            throw new Exception("The expected text was not shown for user with no Credit Card on the account.");
        }

        /// <summary>
        /// Deletes the card.
        /// </summary>
        /// <exception cref="System.Exception">Could not click Delete link: + e.Message</exception>
        public void deleteCard()
        {
            try
            {
                deleteCard_Img.Click();
            }
            catch (Exception e)
            {
                throw new Exception("Could not click Delete link: "+ e.Message);
            }
        }

        /// <summary>
        /// Oks the sponsor warning.
        /// </summary>
        /// <exception cref="System.Exception">Could not click Ok to pop up warning: + e.Message</exception>
        public void okSponsorWarning()
        {
            try
            {
                sponsorWarningPopUpClose_Button.Click();
            }
            catch (Exception e)
            {
                throw new Exception("Could not click Ok to pop up warning: "+ e.Message);
            }
        }

        /// <summary>
        /// Asserts the sponsor warning.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected Warning message was not displayed.</exception>
        public bool assertSponsorWarning()
        {
            if (sponsorWarningPopUpMessage_Container.Text.Equals(Settings.Default.DeleteCardSponsorWarning))
            {
                return true;
            }
            throw new Exception("Expected Warning message was not displayed.");
        }

        public bool isSponsorPopUpDisplayed()
        {
            if (sponsorWarningPopUpMessage_Container.Displayed)
            {
                return true;
            }
            return false;
        }
    }
}
