using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace TestFramework
{
    /// <summary>
    /// Page object model for Contact Us page extends the Base page object model
    /// </summary>
    public class ContactUsPOM : BasePageObject
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactUsPOM"/> class.
        /// </summary>
        public ContactUsPOM() {  }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactUsPOM"/> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        public ContactUsPOM(IWebDriver driver)
        {
            this.driver = driver;
        }

        ///--Web Page Elements--///
        //Contact Us Elements
        IWebElement contactUsName_InputField { get { return driver.FindElement(By.Id("Name")); } }
        IWebElement contactUsEmail_InputField { get { return driver.FindElement(By.Id("Email")); } }
        IWebElement contactUsMobileNumber_InputField { get { return driver.FindElement(By.Id("mobile-number")); } }
        IWebElement contactUsMessage_InputField { get { return driver.FindElement(By.Id("Message")); } }
        IWebElement contactUsSubmit_Button { get { return driver.FindElement(By.XPath("//*[@id='contact-form']/div/div/div/div[2]/div/input")); } }
        

        //Contact Us Error Validation Elements
        IWebElement contactUsNameError_Span { get { return driver.FindElement(By.Id("Name-error")); } }
        IWebElement contactUsEmailError_Span { get { return driver.FindElement(By.Id("Email-error")); } }
        IWebElement contactUsMobileError_Span { get { return driver.FindElement(By.Id("mobile-number-error")); } }
        IWebElement contactUsMessageError_Span { get { return driver.FindElement(By.Id("Message-error")); } }

        //Comtact Us Link Elements
        IWebElement contactUsNav_Dropdown { get { return driver.FindElement(By.XPath("//*[@id='nav-left']/ul/li[2]")); } }
        IWebElement contactUsNav_Link { get { return driver.FindElement(By.LinkText("Contact Us")); } }


        /// <summary>
        /// Enters the contact details.
        /// </summary>
        public void enterContactDetails()
        {
            try
            {
                contactUsName_InputField.Clear();
                contactUsName_InputField.SendKeys("AutoName");
                contactUsEmail_InputField.Clear();
                contactUsEmail_InputField.SendKeys("auto@address.com");
                contactUsMobileNumber_InputField.Clear();
                contactUsMobileNumber_InputField.SendKeys("353872743926");
                contactUsMessage_InputField.Clear();
                contactUsMessage_InputField.SendKeys("Auto Message to be sent");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Enters the custom contact details.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="num"></param>
        /// <param name="message">The message.</param>
        public void enterCustomContactDetails(String name, String email, String num, String message)
        {
            try
            {
                contactUsName_InputField.Clear();
                contactUsName_InputField.SendKeys(name);
                contactUsEmail_InputField.Clear();
                contactUsEmail_InputField.SendKeys(email);
                contactUsMobileNumber_InputField.Clear();
                contactUsMobileNumber_InputField.SendKeys(num);
                contactUsMessage_InputField.Clear();
                contactUsMessage_InputField.SendKeys(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Submits the contact us.
        /// </summary>
        public void submitContactUs()
        {
            try
            {
                contactUsSubmit_Button.Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Asserts the thank you.
        /// </summary>
        /// <param name="thankYou">The thank you.</param>
        /// <returns></returns>
        public bool assertThankYou(IWebElement thankYou)
        {
            if (thankYou.Text.Contains(Settings.Default.ContactThankYou))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Asserts the contact name validation.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected Error message for Name was not displayed.\r Expected: +Settings.Default.ContactNameError+\r Actual: +nameError</exception>
        public bool assertContactNameValidation()
        {
            string nameError = contactUsNameError_Span.Text;
            if (nameError.Contains(Settings.Default.ContactNameError))
            {
                return true;
            }
            verificationErrors.Append("Expected Error message for Name was not displayed.\r Expected: " + Settings.Default.ContactNameError + "\r Actual: " + nameError + "\r");
            return false;
        }

        /// <summary>
        /// Asserts the contact email validation.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected Error message for Email was not displayed.\r Expected:  + Settings.Default.ContactEmailError + \r Actual:  + emailError</exception>
        public bool assertContactEmailValidation()
        {
            string emailError = contactUsEmailError_Span.Text;
            if (emailError.Contains(Settings.Default.ContactEmailError))
            {
                return true;
            }
            verificationErrors.Append("Expected Error message for Email was not displayed.\r Expected: " + Settings.Default.ContactEmailError + "\r Actual: " + emailError + "\r");
            return false;
        }

        /// <summary>
        /// Asserts the contact phone validation.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected Error message for Email was not displayed.\r Expected:  + Settings.Default.ContactEmailError + \r Actual:  + phoneError</exception>
        public bool assertContactPhoneValidation()
        {
            string phoneError = contactUsMobileError_Span.Text;
            if (phoneError.Contains(Settings.Default.ContactPhoneError))
            {
                return true;
            }
            verificationErrors.Append("Expected Error message for Phone Number was not displayed.\r Expected: " + Settings.Default.ContactPhoneError + "\r Actual: " + phoneError + "\r");
            return false;
        }

        /// <summary>
        /// Asserts the contact message validation.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Expected Error message for Message Field was not displayed.\r Expected:  + Settings.Default.ContactMessageError + \r Actual:  + messageError</exception>
        public bool assertContactMessageValidation()
        {
            string messageError = contactUsMessageError_Span.Text;
            if (messageError.Contains(Settings.Default.ContactMessageError))
            {
                return true;
            }
            verificationErrors.Append("Expected Error message for Message Field was not displayed.\r Expected: " + Settings.Default.ContactMessageError + "\r Actual: " + messageError + "\r");
            return false;
        }

        //Nav Menu "GoTo" links
        /// <summary>
        /// Goes to contact us.
        /// </summary>
        /// <exception cref="System.Exception">Link to Contact Us not found:  + e.Message</exception>
        public void goToContactUs()
        {
            try
            {         
                Actions action = new Actions(driver);
                IWebElement we = contactUsNav_Dropdown;
                action.MoveToElement(we).MoveToElement(contactUsNav_Link).Click().Build().Perform();

            }
            catch (Exception e)
            {
                throw new Exception("Link to Contact Us not found: " + e.Message);
            }
        }
        
    }
}
