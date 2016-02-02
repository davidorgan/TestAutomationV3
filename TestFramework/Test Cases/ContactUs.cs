using NUnit.Framework;
using OpenQA.Selenium;
using System;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class ContactUs:ContactUsPOM
    {
        public ContactUsPOM ContactUsPage;
        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestContactUsReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }
        public ExtentReports extent = ExtentManager.Instance;
        public ExtentTest test;

        /// <summary>
        /// Setup the Test.
        /// </summary>
        [SetUp]
        public void SetupTest()
        {
            //Set Browser driver
            if (Settings.Default.Driver.Equals("Chrome"))
            {
                driver = useChrome();
            }
            else if (Settings.Default.Driver.Equals("Firefox"))
            {
                driver = useFirefox();
            }
            else if (Settings.Default.Driver.Equals("IE"))
            {
                driver = useIE();
            }

            ContactUsPage = new ContactUsPOM(driver);

            baseURL = Settings.Default.BaseURL;
           // verificationErrors = new StringBuilder();

            //// maximize window
            ContactUsPage.MaximizeWindow();

            //Set implicit wait
            ContactUsPage.setImplicitWait(30);

            extent.Config()
                    .DocumentTitle("Contact Us Report")
                    .ReportName("Contact Us")
                    .ReportHeadline("Report --- <a href='TestReport.html'>Back to Reports List</a>");

        }

        /// <summary>
        /// Teardowns the Test.
        /// </summary>
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                extent.EndTest(test);
                extent.Flush();
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            //Assert.AreEqual("", verificationErrors.ToString());
        }

        /// <summary>
        /// Thes the contact us Test.
        /// </summary>
        /// <exception cref="System.Exception">The expected Thank you message was not displayed</exception>
        [Test]
        public void B_TheContactUsTest()
        {
            try
            {
                test = extent.StartTest("Contact Us Test", "Test to check the Contact Us form.")
                                .AssignCategory(Settings.Default.Driver);

                //Got to base URL
                ContactUsPage.Goto(baseURL);

                //Login
                ContactUsPage.doLogin();

                //Test Steps go here
                ContactUsPage.waitForSpinnerDashboard();
                ContactUsPage.waitForPlansToLoadDashboard();
                ContactUsPage.goToContactUs();
                ContactUsPage.enterContactDetails();
                ContactUsPage.submitContactUs();

               //Any assertions if requried
                IWebElement thankYouMessage = driver.FindElement(By.XPath("//*[@id='active_part']/div/div"));
                Console.WriteLine(thankYouMessage.Text);
                bool isContact = ContactUsPage.assertThankYou(thankYouMessage);
                outputText = "Form Submitted and Thank You message displayed as expected.";

                if (isContact == false)
                {
                    Console.WriteLine("The expected Thank you message was not displayed");
                    throw new Exception("The expected Thank you message was not displayed");
                }
                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                ContactUsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ContactUsResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ContactUsResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ContactUsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ContactUsError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ContactUsError.png"));
                throw;
            }
        }

        /// <summary>
        /// Thes the contact us invalid Test.
        /// </summary>
        [Test]
        public void A_TheContactUsInvalidTest()
        {
            try
            {
                test = extent.StartTest("Invalid Details Contact Us Test", "Test to check the error validation for Contact Us form.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                ContactUsPage.Goto(baseURL);

                //Login
                ContactUsPage.doLogin();

                //Test Steps go here
                ContactUsPage.waitForSpinnerDashboard();
                //ContactUsPage.waitForPlansToLoadDashboard();
                System.Threading.Thread.Sleep(3000);
                ContactUsPage.goToContactUs();
                ContactUsPage.enterCustomContactDetails("","","","");
                ContactUsPage.submitContactUs();

                //Any assertions if requried
                bool errorName = ContactUsPage.assertContactNameValidation();
                bool errorEmail = ContactUsPage.assertContactEmailValidation();
                bool errorPhone = ContactUsPage.assertContactPhoneValidation();
                bool errorMessage = ContactUsPage.assertContactMessageValidation();

                if (errorName == false || errorEmail == false || errorPhone == false || errorMessage == false)
                {
                    throw new Exception("The following validation messages did not display as expected: \r" + verificationErrors);
                }
                else
                {
                    outputText = "Expected error validation messages were displayed.";
                }

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                ContactUsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ContactUsInvalidResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ContactUsInvalidResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ContactUsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ContactUsInvalidError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ContactUsInvalidError.png"));
                throw;
            }
        }

    }
}
