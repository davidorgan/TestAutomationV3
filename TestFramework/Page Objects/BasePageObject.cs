using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System.Drawing.Imaging;
using OpenQA.Selenium.Support.UI;
using System.Data.SqlClient;
using OpenQA.Selenium.Interactions;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Remote;


namespace TestFramework
{
    /// <summary>
    /// Age Object model for overall audi site
    /// </summary>
    public abstract class BasePageObject
    {       
        protected IWebDriver driver;
        protected string PageTitle { get { return driver.Title; } }
        protected string baseURL = Settings.Default.BaseURL;
        public AccountHelper.accountDetails currentAccount;

        protected string outputText;

        protected IWebElement CurrentPetName;
        private string CurrentDBConnection
        {
            get
            {
                if (Settings.Default.BaseURL.Contains("https://auto-qa.cubictelecom.com/"))
                {
                    return Settings.Default.DBConnectionStringQa;
                }
                else if (Settings.Default.BaseURL.Contains("https://auto-uat.cubictelecom.com/"))
                {
                    return Settings.Default.DBConnectionStringUAT;
                }
                return Settings.Default.DBConnectionStringQa;
            }

        }

        protected Dictionary<string, string> DBLatestCustomer;
        protected Dictionary<string, string> DBActivateToken;
        private Dictionary<string, string> DBCustomerDetails;

        protected static string mailNum;

        protected StringBuilder verificationErrors = new StringBuilder();

        protected BasePageObject() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePageObject"/> class.
        /// </summary>
        /// <param name="driver">The driver.</param>
        protected BasePageObject(IWebDriver driver)
        {
            this.driver = driver;
            this.currentAccount = AccountHelper.accountDetails.setAccountDetails();
        }

        ///--Web Page Elements--///
        //Login Elements
        IWebElement loginEmail_InputField { get { return driver.FindElement(By.Id("Email")); } }
        IWebElement loginPassword_InputField { get { return driver.FindElement(By.Id("Password")); } }
        IWebElement loginSubmit_Button { get {return driver.FindElement(By.Id("login-button"));} }
        IWebElement logout_Link { get { return driver.FindElement(By.XPath("//a[contains(@href, '/Account/Logout')]")); } }

        //Footer Elements
        IWebElement footerDataInside_Link { get { return driver.FindElement(By.LinkText("About Data Inside")); } }
        IWebElement footerTermsAndConditions_Link { get { return driver.FindElement(By.LinkText("Terms & Conditions"));} }
        IWebElement footerPrivacyPolicy_Link { get { return driver.FindElement(By.LinkText("Privacy Policy")); } }
        IWebElement footerContactUs_Link { get { return driver.FindElement(By.LinkText("Contact Us")); } }
        IWebElement footerCopyright_Container { get { return driver.FindElement(By.XPath("/html/body/footer/div/div/div/div")); } }
        IWebElement footerPoweredByAudi_Img { get {return driver.FindElement(By.XPath("/html/body/footer/div/div/div/a/img")); } }

        //Forgot Password Elements
        IWebElement forgotPassword_Link { get { return driver.FindElement(By.XPath("//*[@id='loginForm']/div/div[4]/a")); } }
        IWebElement forgotPasswordEmail_InputField { get {  return driver.FindElement(By.Id("Email")); } }
        IWebElement forgotPasswordSubmit_Button { get {  return driver.FindElement(By.XPath("//*[@id='form0']/div/div[2]/input")); } }
        IWebElement forgotPasswordSuccess_P { get {return driver.FindElement(By.XPath("//*[@id='forgot-password-wrapper']/div/div[1]/p"));} }
        IWebElement forgotPasswordMailSentBack_Button { get { return driver.FindElement(By.XPath("//*[@id='forgot-password-wrapper']/div/div[2]/a")); } }

        //Reset Password
        IWebElement resetPasswordEmail_InputField { get { return driver.FindElement(By.Id("Password")); } }
        IWebElement resetPasswordReEnterEmail_InputField { get { return driver.FindElement(By.Id("RePassword")); } }
        IWebElement resetPasswordSubmit_Button { get { return driver.FindElement(By.XPath("//*[@id='form0']/div/div/div/input[3]")); } }
        IWebElement resetPasswordSuccess_Container { get { return driver.FindElement(By.XPath("//*[@id='reset-password-page']/div[2]/div/div")); } }

        //Loading Spinner
        IWebElement planLoadSpinner_Container(string vin) { return driver.FindElement(By.XPath("//div[contains(@id, 'plans-placeholder-spinner-" + vin + "')]")); }

        IWebElement pageLoadSpinner_Container { get { return driver.FindElement(By.XPath("//*[@id='progress-indicator']")); } }

        //Nav Elements
        IWebElement navFirstName_Container { get { return driver.FindElement(By.XPath("//*[@id='nav-right']/ul/li")); } }


        /// <summary>
        /// Gets the page title.
        /// </summary>
        /// <returns></returns>
        public string getPageTitle()
        {
            string title = driver.Title;
            return title;

        }

        /// <summary>
        /// Asserts the page title.
        /// </summary>
        /// <param name="actualTitle">The actual title.</param>
        /// <param name="expectedTitle">The expected title.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">The expected page is not displayed!</exception>
        public bool assertPageTitle(string expectedTitle)
        {
            if (String.CompareOrdinal(PageTitle, expectedTitle) == 0)
            {
                return true;
            }
            throw new Exception("The expected page is not displayed! Expected: '"+expectedTitle+"', Actual: '"+PageTitle+"'");
        }

        /// <summary>
        /// Uses the chrome.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">There was an issue creating Chrome Driver: +
        ///                                       + e.Message</exception>
        protected IWebDriver useChrome()
        {
            IWebDriver Cdriver;
            try
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                Cdriver = new ChromeDriver(@Settings.Default.ChromeLocation);
            }
            catch (Exception e)
            {
                throw new Exception("There was an issue creating Chrome Driver:" +
                                    " " + e.Message);
            }
            return Cdriver;
        }

        /// <summary>
        /// Uses the firefox.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">There was an issue creating Firefox Driver: +
        ///                                       + e.Message</exception>
        protected IWebDriver useFirefox()
        {
            IWebDriver Fdriver;
            try
            {
                Fdriver = new FirefoxDriver();
            }
            catch (Exception e)
            {
                throw new Exception("There was an issue creating Firefox Driver:" +
                                    " " + e.Message);
            }
            return Fdriver;
        }

        /// <summary>
        /// Uses the ie.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">There was an issue creating Internet Explorer Driver: +
        ///                                       + e.Message</exception>
        protected IWebDriver useIE()
        {
            IWebDriver IEdriver;
            try
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                IEdriver = new InternetExplorerDriver(@Settings.Default.IELocation);
                
            }
            catch (Exception e)
            {
                throw new Exception("There was an issue creating Internet Explorer Driver:" +
                                    " " + e.Message);
            }
            return IEdriver;
        }

        public IWebDriver useBrowserStack(string browser, string browser_version, string os, string os_version, string resolution)
        {
            IWebDriver BrowserStackDriver;
            try
            {
                DesiredCapabilities capability = new DesiredCapabilities();
                capability.SetCapability("browserstack.user", "cubictelecom1");
                capability.SetCapability("browserstack.key", "c79ymuRgRUPWiEU9xr1H");
                capability.SetCapability("browser", browser);
                capability.SetCapability("browser_version", browser_version);
                capability.SetCapability("os", os);
                capability.SetCapability("os_version", os_version);
                capability.SetCapability("resolution", resolution);
                capability.SetCapability("browserstack.debug", "true");


                BrowserStackDriver = new RemoteWebDriver(
                  new Uri("http://hub.browserstack.com/wd/hub/"), capability
                );
            }
            catch (Exception e)
            {
                throw new Exception("There was an issue creating Browser Stack Driver:" +
                                    " " + e.Message);
            }
            return BrowserStackDriver;
        }

        public IWebDriver useBrowserStackPhone(string browserName, string platform, string device)
        {
            IWebDriver BrowserStackDriver;
            try
            {
                DesiredCapabilities capability = new DesiredCapabilities();
                capability.SetCapability("browserstack.user", "cubictelecom1");
                capability.SetCapability("browserstack.key", "c79ymuRgRUPWiEU9xr1H");
                capability.SetCapability("browserName", browserName);
                capability.SetCapability("platform", platform);
                capability.SetCapability("device", device);
                capability.SetCapability("browserstack.debug", "true");


                BrowserStackDriver = new RemoteWebDriver(
                  new Uri("http://hub.browserstack.com/wd/hub/"), capability
                );
            }
            catch (Exception e)
            {
                throw new Exception("There was an issue creating Browser Stack Phone Driver:" +
                                    " " + e.Message);
            }
            return BrowserStackDriver;
        }

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        public void MaximizeWindow()
        {
            driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Gets the inner HTML.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public string GetInnerHtml(IWebElement element)
        {
            var js = IJDriver;
            var result = "";

            try
            {
                if (js != null)
                {
                    result = (string)js.ExecuteScript("return arguments[0].innerHTML;", element);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return result;

        }

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <param name="saveLocation">The save location.</param>
        public void TakeScreenshot(string saveLocation)
        {
            ITakesScreenshot ssdriver = driver as ITakesScreenshot;
            if (ssdriver == null) return;
            Screenshot screenshot = ssdriver.GetScreenshot();
            screenshot.SaveAsFile(saveLocation, ImageFormat.Png);
        }

        /// <summary>
        /// Waits for load.
        /// </summary>
        internal void WaitForLoad()
        {
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30));

            try
            {
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PageLoadError.png"); 
                throw;
            }

        }

        /// <summary>
        /// Sets the implicit wait.
        /// </summary>
        /// <param name="time">The time.</param>
        public void setImplicitWait(int time) 
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(time));
        }


        protected void turnOffImplicitWaits()
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
        }

        protected void turnOnImplicitWaits()
        {
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
        }

        /// <summary>
        /// Sets the page load timeout.
        /// </summary>
        /// <param name="time">The time.</param>
        public void setPageLoadTimeout(int time)
        {
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(time));
        }

        /// <summary>
        /// Sets the script load timeout.
        /// </summary>
        /// <param name="time">The time.</param>
        public void setScriptLoadTimeout(int time)
        {
            driver.Manage().Timeouts().SetScriptTimeout(TimeSpan.FromSeconds(time));
        }

        /// <summary>
        /// Determines whether this instance is at.
        /// </summary>
        /// <returns></returns>
        public bool IsAt()
        {
            return driver.Title.Contains(PageTitle);
        }


        /// <summary>
        /// Goto the specified URL.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <exception cref="System.Exception"></exception>
        public void Goto(string URL)
        {
            try
            {
                driver.Navigate().GoToUrl(URL);
            }
            catch (Exception e)
            {
                throw new Exception(GetType().Name + " could not be loaded. Check page url is correct in app.config." +
                                    " " + e.Message);
            }
        }

        //"Do" Functionality 
        /// <summary>
        /// Does the login.
        /// </summary>
        /// <exception cref="System.Exception">The login process did not work: +
        /// + e.Message</exception>
        public void doLogin(AccountHelper.accountDetails currentAc)
        {
            try
            {
                loginEmail_InputField.Clear();
                loginEmail_InputField.SendKeys(currentAc.username);
                loginPassword_InputField.Clear();
                loginPassword_InputField.SendKeys(currentAc.password);
                loginSubmit_Button.Click();
            }
            catch (Exception e)
            {
                throw new Exception("The login process did not work:" +
                    " " + e.Message);
            }
        }

        /// <summary>
        /// Does the logout.
        /// </summary>
        /// <exception cref="System.Exception">The logout link did not work: +
        ///                      + e.Message</exception>
        public void doLogout()
        {
            try
            {
                logout_Link.Click();
            }
            catch (Exception e)
            {
                throw new Exception("The logout link did not work:" +
                   " " + e.Message);
            }
        }

        /// <summary>
        /// Determines whether [is element present] [the specified by].
        /// </summary>
        /// <param name="by">The by.</param>
        /// <returns></returns>
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is alert present].
        /// </summary>
        /// <returns></returns>
        public bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }


        //Go to footer links
        /// <summary>
        /// Goes to about data inside.
        /// </summary>
        /// <exception cref="System.Exception">
        /// The Data Inside link was not found: +   + e1.Message
        /// or
        /// The Data Inside link did not work: +   + e2.Message
        /// </exception>
        public void goToAboutDataInside()
        {
            try
            {
                JavaScriptClick(footerDataInside_Link);
            }
            catch (NoSuchElementException e1)
            {
                throw new Exception("The Data Inside link was not found:" + " " + e1.Message);
            }
            catch (Exception e2)
            {
                throw new Exception("The Data Inside link did not work:" + " " + e2.Message);
            }
        }

        /// <summary>
        /// Goes to tand c.
        /// </summary>
        public void goToTandC()
        {
            JavaScriptClick(footerTermsAndConditions_Link);
        }

        /// <summary>
        /// Goes to privacy policy.
        /// </summary>
        public void goToPrivacyPolicy()
        {
            JavaScriptClick(footerPrivacyPolicy_Link);
        }

        /// <summary>
        /// Goes to footer contact.
        /// </summary>
        public void goToFooterContact()
        {
            JavaScriptClick(footerContactUs_Link);
        }

        /// <summary>
        /// Goes to powered by.
        /// </summary>
        public void goToPoweredBy()
        {
            footerPoweredByAudi_Img.Click();
        }

        public void assertCopyrightText()
        {
            string span = footerCopyright_Container.Text;
            if (span.Contains(Settings.Default.FooterCopyrightText))
            {
                return;
            }
            throw new Exception("Expected Copyright text not displayed.");
        }

        //Required to use instead of selenium click when element is not visible
        /// <summary>
        /// Javas the script click.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <exception cref="System.Exception">Javascript click function failed.</exception>
        public void JavaScriptClick(IWebElement element)
        {
            try
            {
                IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                executor.ExecuteScript("arguments[0].click();", element);
            }
            catch (Exception e)
            {
                throw new Exception("Javascript click function failed. " + e.Message);
            }
        }

        /// <summary>
        /// Switches the tab.
        /// </summary>
        public void switchTab()
        {
            //Open a new tab using Ctrl + t 
            driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + Keys.Tab);
        }

        /// <summary>
        /// Switches to window.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public bool SwitchToWindow(string title)
        {
            var currentWindow = driver.CurrentWindowHandle;
            var availableWindows = new List<string>(driver.WindowHandles);
            foreach (string w in availableWindows)
            {
                if (w == currentWindow) continue;
                driver.SwitchTo().Window(w);
                if (driver.Title == title)
                    return true;
                driver.SwitchTo().Window(currentWindow);
            }
            return false;  
        }

        /// <summary>
        /// Switches to last window.
        /// </summary>
        public void switchToLastWindow()
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        /// <summary>
        /// Waits the until present.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void waitUntilPresent(String id)
        {
            while (IsElementPresent(By.Id(id)) != true)
            {
                Thread.Sleep(100);
            }
        }


        /// <summary>
        /// Waits the until element not displayed.
        /// </summary>
        /// <param name="web">The web.</param>
        public void waitUntilElementNotDisplayed(IWebElement web)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ddriver => !web.Displayed);
        }

        /// <summary>
        /// Waits the until element is displayed.
        /// </summary>
        /// <param name="web">The web.</param>
        public void waitUntilElementIsDisplayed(IWebElement web)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(ddriver => web.Displayed);
        }

        /// <summary>
        /// Waits the until title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <exception cref="System.Exception">Expected Page did not load: + e.Message</exception>
        public void waitUntilTitle(string title)
        {
            try
            {
                //Wait for page title
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(ExpectedConditions.TitleContains(title));
            }
            catch (Exception e)
            {
                throw new Exception("Expected Page did not load: "+ e.Message);
            }
        }

        /// <summary>
        /// Waits the until element exists.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <exception cref="System.Exception"></exception>
        public void waitUntilElementExists(string xpath)
        {
            try
            {
                //Wait for element with XPath exists
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                wait.Until(ExpectedConditions.ElementExists(By.XPath(xpath)));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " :Expected page not displayed");
            }
        }

        //Forgot Password POM
        /// <summary>
        /// Goes to forgot password.
        /// </summary>
        public void goToForgotPassword()
        {
            forgotPassword_Link.Click();
        }

        /// <summary>
        /// Enters the email forgot pw.
        /// </summary>
        /// <param name="email">The email.</param>
        public void enterEmailForgotPW(string email)
        {
            forgotPasswordEmail_InputField.Clear();
            forgotPasswordEmail_InputField.SendKeys(email);
        }

        /// <summary>
        /// Enters the new password.
        /// </summary>
        /// <param name="pw">The pw.</param>
        public void enterNewPassword(string pw)
        {
            resetPasswordEmail_InputField.Clear();
            resetPasswordEmail_InputField.SendKeys(pw);
        }

        /// <summary>
        /// Reenters the new password.
        /// </summary>
        /// <param name="pw">The pw.</param>
        public void reenterNewPassword(string pw)
        {
            resetPasswordReEnterEmail_InputField.Clear();
            resetPasswordReEnterEmail_InputField.SendKeys(pw);
        }


        /// <summary>
        /// Submits the new password.
        /// </summary>
        public void submitNewPassword()
        {
            resetPasswordSubmit_Button.Click();
        }

        public bool assertPasswordChangedSuccess()
        {
            IWebElement success = resetPasswordSuccess_Container;
            try
            {               
                if (success.Text.Equals(Settings.Default.PasswordChangedSuccessMessage))
                {
                    Console.WriteLine("The message displayed '"+success.Text+"' as Expected");
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("The expected message of '"+Settings.Default.PasswordChangedSuccessMessage+"'. The message was displayed as '"+success.Text+"'.\n"+e.Message);
                
            }
            return false;
        }

        /// <summary>
        /// Submits the forgot pw.
        /// </summary>
        public void submitForgotPW()
        {
            forgotPasswordSubmit_Button.Click();
        }

        /// <summary>
        /// Checks the forgot mail sent.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">The mail successfully sent page has not displayed correctly.</exception>
        public bool checkForgotMailSent()
        {
            string mailsent = forgotPasswordSuccess_P.Text;
            if (mailsent.Equals(Settings.Default.ForgotPWMailSent))
            {
                return true;
            }
            throw new Exception("The mail successfully sent page has not displayed correctly.");
        }

        /// <summary>
        /// Backs the forgot pw mail sent.
        /// </summary>
        public void backForgotPWMailSent()
        {
            forgotPasswordMailSentBack_Button.Click();
        }

        /// <summary>
        /// Checks the version.
        /// </summary>
        /// <param name="currentVersion">The current version.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">The Expected software version is not being used:  + currentVersion</exception>
        public bool checkVersion(String currentVersion)
        {
            bool isCorrectVersion = driver.PageSource.Contains(currentVersion);
            if (isCorrectVersion) 
            { 
                return true; 
            }
            throw new Exception("The Expected software version is not being used: " + currentVersion);
        }

        /// <summary>
        /// Gets or sets the ij driver.
        /// </summary>
        /// <value>
        /// The ij driver.
        /// </value>
        private static IJavaScriptExecutor IJDriver { get; set; }


        //SQL Database connections
        /// <summary>
        /// Gets the latest customer details database.
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string,string> getLatestCustomerDetailsDB()
        {

            Dictionary<string, string> customerDetails = new Dictionary<string,string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = CurrentDBConnection;
                conn.Open();

                // use the connection here
                //SqlCommand command = new SqlCommand("SELECT * FROM dbo.res_tblCustomerDetails", conn);

                // Create the command
                SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM dbo.res_tblCustomerDetails ORDER BY ID desc", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerDetails.Add("ID", Convert.ToString(reader[0]));
                        customerDetails.Add("FirstName", Convert.ToString(reader[1]));
                        customerDetails.Add("LastName", Convert.ToString(reader[2]));
                        customerDetails.Add("Email", Convert.ToString(reader[4]));
                    }
                }

            }
            return customerDetails;
        }

        /// <summary>
        /// Gets the latest activation token database.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getLatestTokenDB()
        {
            Dictionary<string, string> activateToken = new Dictionary<string, string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = CurrentDBConnection;
                conn.Open();

                // use the connection here
                // Create the command
                SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM dbo.res_tblTokens ORDER BY ID desc", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        activateToken.Add("ID", Convert.ToString(reader[0]));
                        activateToken.Add("Token", Convert.ToString(reader[1]));
                        activateToken.Add("ExpireAt", Convert.ToString(reader[2]));
                    }
                }

            }
            return activateToken;
        }

        /// <summary>
        /// Gets the customer details database.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> getCustomerDetailsDB()
        {
            Dictionary<string, string> customerDetails = new Dictionary<string, string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = CurrentDBConnection;
                conn.Open();

                // use the connection here
                // Create the command
                string sql = "SELECT TOP 1 * FROM dbo.res_tblCustomerDetails WHERE ContactEmail like '" + Settings.Default.User + "' ORDER BY ID desc";
                SqlCommand command = new SqlCommand(sql, conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerDetails.Add("ID", Convert.ToString(reader[0]));
                        customerDetails.Add("FirstName", Convert.ToString(reader[1]));
                        customerDetails.Add("LastName", Convert.ToString(reader[2]));
                    }
                }

            }
            return customerDetails;
        }

        protected string getCurrentFullName()
        {
            DBCustomerDetails = getCustomerDetailsDB();
            string DBCustomerDetailsFirstName = DBCustomerDetails["FirstName"];
            string DBCustomerDetailsLastName = DBCustomerDetails["LastName"];
            string FullName = DBCustomerDetailsFirstName + " " + DBCustomerDetailsLastName;
            return FullName;
        }


        /// <summary>
        /// Hovers the element
        /// </summary>
        public void hoverElement(IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }

        /// <summary>
        /// Waits for plans to load on dashboard.
        /// </summary>
        public void waitForPlansToLoadDashboard()
        {
            Thread.Sleep(500);
            IWebElement spinner = planLoadSpinner_Container(Settings.Default.CurrentVIN);
            waitUntilElementNotDisplayed(spinner);
        }

        public void waitForPlansToLoadDashboard(string vin)
        {
            Thread.Sleep(500);
            IWebElement spinner = planLoadSpinner_Container(vin);
            waitUntilElementNotDisplayed(spinner);
        }

        /// <summary>
        /// Waits for spinner overlay to no longer display on dashboard.
        /// </summary>
        public void waitForSpinnerDashboard()
        {
            Thread.Sleep(500);
            waitUntilElementNotDisplayed(pageLoadSpinner_Container);
        }

        /// <summary>
        /// Determines whether [the specified date][is valid date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public bool isValidDate(string date)
        {
            DateTime dt;
            CultureInfo enIE = new CultureInfo("en-IE");
            bool isValid = DateTime.TryParseExact(date, Settings.Default.DateFormat, enIE, DateTimeStyles.None, out dt);
            return isValid;
        }

        protected bool IsValidCurrency(string currency)
        {
            Regex regex = new Regex(@"^[€]?([0-9]{1,2})?,?([0-9]{3})?,?([0-9]{3})?(\.[0-9]{2})?[€]?€");
            return regex.IsMatch(currency);
        }

    }
}
