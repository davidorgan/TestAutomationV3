using System;
using OpenQA.Selenium;

namespace TestFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAccountPOM : BasePageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountPOM"/> class.
        /// </summary>
        public CreateAccountPOM() {  }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAccountPOM"/> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public CreateAccountPOM(IWebDriver driver)
        {
            this.driver = driver;

        }

        ///--Web Page Elements--///
        //Create Account Elements
        IWebElement createAccountEmail_InputField { get { return driver.FindElement(By.Id("Email")); } }
        IWebElement createAccountPassword_InputField { get { return driver.FindElement(By.Id("Password")); } }
        IWebElement createAccountConfirmPassword_InputField { get { return driver.FindElement(By.Id("ConfirmPassword")); } }
        IWebElement createAccountFirstName_InputField { get { return driver.FindElement(By.Id("FirstName")); } }
        IWebElement createAccountLastName_InputField { get { return driver.FindElement(By.Id("LastName")); } }
        IWebElement createAccountTerms_Checkbox { get { return driver.FindElement(By.XPath("//*[@id='registration-form']/div[2]/div/div/label")); } }
        IWebElement createAccountSubmit_Button { get { return driver.FindElement(By.XPath("//*[@id='registration-form']/div[3]/div/input")); } }
        IWebElement createAccountSuccess_P { get { return driver.FindElement(By.XPath("//*[@id='registration-page']/div/div[1]/p")); } }

        IWebElement createAccountBackToLogin_Button { get { return driver.FindElement(By.XPath("//*[@id='registration-page']/div/div[2]/a")); } }
        IWebElement createAccount_Link { get { return driver.FindElement(By.LinkText("Create New Account")); } }

       // String _newEmail { get { return "AutoUser@Test.com"; } set { _newEmail = value; } }
       // String _newPassword { get { return "Cubic!!04"; } set { _newPassword = value; } }
        //String _newFirstName { get { return "AutoName"; } set { _newFirstName = value; } }
       // String _newLastName { get { return "AutoLast"; } set { _newLastName = value; } }

        /// <summary>
        /// Enters the new account details.
        /// </summary>
        public void enterNewAccountDetails()
        {
            try
            {
                createAccountEmail_InputField.Clear();
                createAccountEmail_InputField.SendKeys(newEmail);
                createAccountPassword_InputField.Clear();
                createAccountPassword_InputField.SendKeys(newPassword);
                createAccountConfirmPassword_InputField.Clear();
                createAccountConfirmPassword_InputField.SendKeys(newPassword);
                createAccountFirstName_InputField.Clear();
                createAccountFirstName_InputField.SendKeys(newFName);
                createAccountLastName_InputField.Clear();
                createAccountLastName_InputField.SendKeys(newLName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Enters the custom account details.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="fname">The fname.</param>
        /// <param name="lname">The lname.</param>
        public void setCustomAccountDetails(String email, String password, String fname, String lname)
        {
            try
            {
                newEmail = email;
                newPassword = password;
                newFName = fname;
                newLName = lname;               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Ticks agree to Terms and conditions.
        /// </summary>
        public void tickAgreeTandC()
        {
            createAccountTerms_Checkbox.Click();
        }

        /// <summary>
        /// Submits the create account.
        /// </summary>
        public void submitCreateAccount()
        {
            try
            {
                createAccountSubmit_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Asserts the account created.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">The confirmation of account creation page was not displayed correctly</exception>
        public bool assertAccountCreated()
        {
            IWebElement created = createAccountSuccess_P;
            if (created.Text.Contains(Settings.Default.CreateAccountConfirmed))
            {
                return true;
            }
            throw new Exception("The confirmation of account creation page was not displayed correctly");
        }

        /// <summary>
        /// Goes the back to login.
        /// </summary>
        public void goBackToLogin()
        {
            createAccountBackToLogin_Button.Click();
        }

        //Nav Menu "GoTo" links
        /// <summary>
        /// Goes to create account.
        /// </summary>
        /// <exception cref="System.Exception">Link to CreateAccount not found:  + e.Message</exception>
        public void goToCreateAccount()
        {
            try
            {
                createAccount_Link.Click();
            }
            catch (ElementNotVisibleException e)
            {
                throw new Exception("Link to CreateAccount not found: " + e.Message);
            }
        }

        /// <summary>
        /// Asserts the customer added to database.
        /// </summary>
        /// <param name="newEmail">The new email.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">The customer has not been added to the account. The latest customer in Database is:  + DBLatestCustomer[Email]</exception>
        public bool assertCustomerAddedtoDB(string newEmail)
        {
            DBLatestCustomer = getLatestCustomerDetailsDB();
            if (DBLatestCustomer["Email"].Equals(newEmail))
            {
                outputText = "The customer (" + DBLatestCustomer["Email"] + ") has been added to the database as expected.<br />";
                outputText += "Fname: " + DBLatestCustomer["FirstName"] + "<br />";
                outputText += "Lname: " + DBLatestCustomer["FirstName"];
                return true;
            }
            else
            {
                throw new Exception("The customer has not been added to the account. The latest customer in Database is: " + DBLatestCustomer["Email"]);
            }
        }

        /// <summary>
        /// Gets the current activate token.
        /// </summary>
        /// <returns></returns>
        public string getCurrentActivateToken()
        {
            DBActivateToken = getLatestTokenDB();
            string currentActivateToken = DBActivateToken["Token"];
            return currentActivateToken;
        }

        /// <summary>
        /// Goes to activate page.
        /// </summary>
        public void goToActivatePage()
        {
            string currentActivateToken = getCurrentActivateToken();
            Goto(baseURL + Settings.Default.ActivateAccountSubURL + currentActivateToken);
        }
        
    }
}
