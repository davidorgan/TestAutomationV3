using System;
using OpenQA.Selenium;

namespace TestFramework
{
    /// <summary>
    /// Account Details Page Object Model Extends the Base Page object model
    /// </summary>
    public class AccountDetailsPOM : BasePageObject
    {
        public AccountHelper.accountDetails altAccount;
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountDetailsPOM"/> class.
        /// </summary>
        public AccountDetailsPOM() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountDetailsPOM"/> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public AccountDetailsPOM(IWebDriver driver)
        {
            this.driver = driver;
            this.currentAccount = AccountHelper.accountDetails.setAccountDetails();
            this.altAccount = new AccountHelper.accountDetails(
                currentAccount.username,
                Settings.Default.AltPassword,
                currentAccount.vin,
                currentAccount.acType,
                currentAccount.firstName,
                currentAccount.lastName
                );
        }

        ///--Web Page Elements--///
        //Account Details Elements
        IWebElement accountDetailsFirstName_InputField { get { return driver.FindElement(By.Id("FirstName")); } }
        IWebElement accountDetailsLastName_InputField { get { return driver.FindElement(By.Id("LastName")); } }
        IWebElement accountDetailsAddress1_InputField { get { return driver.FindElement(By.Id("Address_StreetAddress1")); } }
        IWebElement accountDetailsAddress2_InputField { get { return driver.FindElement(By.Id("Address_StreetAddress2")); } }
        IWebElement accountDetailsAddressCity_InputField { get { return driver.FindElement(By.Id("Address_City")); } }
        IWebElement accountDetailsAddressState_InputField { get { return driver.FindElement(By.Id("Address_State")); } }
        IWebElement accountDetailsAddressPostCode_InputField { get { return driver.FindElement(By.Id("Address_PostCode")); } }
        IWebElement accountDetailsSubmit_Button { get { return driver.FindElement(By.XPath("//*[@id='form0']/div[2]/div/input")); } }
        IWebElement accountDetailsSuccess_P { get { return driver.FindElement(By.XPath("//*[@id='updateAccountDetails']/p")); } }

        //Account links
        IWebElement accountDetails_Link { get { return driver.FindElement(By.XPath("//a[contains(@href, '/Account/AccountDetails')]")); } }

        //Update Password
        IWebElement updatePasswordCurrent_InputField { get { return driver.FindElement(By.Id("CurrentPassword")); } }
        IWebElement updatePasswordNew_InputField { get { return driver.FindElement(By.Id("NewPassword")); } }
        IWebElement updatePasswordConfirmNew_InputField { get { return driver.FindElement(By.Id("NewPasswordConfirm")); } }
        IWebElement updatePasswordSubmit_Button { get { return driver.FindElement(By.XPath("//*[@id='form1']/div[2]/div/input")); } }
        IWebElement updatePasswordSuccess_P { get { return driver.FindElement(By.XPath("//*[@id='account-password-page']/div/div/p")); } }
        
        /// <summary>
        /// Enters the account updates.
        /// </summary>
        public void enterAccountUpdates()
        {
            try
            {
                accountDetailsFirstName_InputField.SendKeys("1");
                accountDetailsLastName_InputField.SendKeys("2");
                accountDetailsAddress1_InputField.SendKeys("3");
                accountDetailsAddress2_InputField.SendKeys("4");
                accountDetailsAddressCity_InputField.SendKeys("5");
                accountDetailsAddressState_InputField.SendKeys("6");
                accountDetailsAddressPostCode_InputField.SendKeys("7");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Submits the update account.
        /// </summary>
        public void submitUpdateAccount()
        {
            try
            {
                accountDetailsSubmit_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
        public bool assertAccountUpdatedMessage()
        {
            IWebElement success = accountDetailsSuccess_P;
            try
            {
                if (success.Text.Equals(Settings.Default.AccountUpdatedSuccessMessage))
                {
                    Console.WriteLine("The message displayed '" + success.Text + "' as Expected");
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("The expected message of '" + Settings.Default.AccountUpdatedSuccessMessage + "'. The message was displayed as '" + success.Text + "'.\n" + e.Message);

            }
            return false;
        }

        //Nav Menu "GoTo" links
        /// <summary>
        /// Goes to account details.
        /// </summary>
        public void goToAccountDetails()
        {
            //Javascript Click overide used for nav dropdown menu items
            IWebElement usageLink = accountDetails_Link;
            JavaScriptClick(usageLink);
        }


        public void accountEnterCurrentPassword(string currentPW)
        {
            updatePasswordCurrent_InputField.Clear();
            updatePasswordCurrent_InputField.SendKeys(currentPW);
        }

        public void accountEnterNewPassword(string newPW)
        {
            updatePasswordNew_InputField.Clear();
            updatePasswordNew_InputField.SendKeys(newPW);
        }

        public void accountEnterConfirmNewPassword(string newPW)
        {
            updatePasswordConfirmNew_InputField.Clear();
            updatePasswordConfirmNew_InputField.SendKeys(newPW);
        }

        public void submitUpdatePassword()
        {
            updatePasswordSubmit_Button.Click();
        }

        public bool assertUpdatePasswordSuccess()
        {
            string successText = updatePasswordSuccess_P.Text;
                if (successText.Contains(Settings.Default.PasswordUpdateSuccess))
                {
                    return true;
                }
            throw new Exception("The expected message was not displayed for a successful password update.");
        }
    }
}