using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class ForgotPassword : UsageHistoryPOM
    {
        public DashboardPOM ForgotPasswordPage;

        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestForgotPasswordReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }
        public ExtentReports extent = ExtentManager.Instance;
        public ExtentTest test;

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

            ForgotPasswordPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            ForgotPasswordPage.MaximizeWindow();

            //Set implicit wait
            ForgotPasswordPage.setImplicitWait(30);

            extent.Config()
                    .DocumentTitle("Forgot Password Report")
                    .ReportName("Forgot Password")
                    .ReportHeadline("Report --- <a href='TestReport.html'>Back to Reports List</a>");

        }

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
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void A_TheForgotPasswordTest()
        {
            try
            {
                test = extent.StartTest("Forgot Password Test", "Test forgot password functionality is working.")
.AssignCategory(Settings.Default.Driver);
                //Go to base URL
                ForgotPasswordPage.Goto(baseURL);

                //Test Steps go here
                ForgotPasswordPage.goToForgotPassword();
                System.Threading.Thread.Sleep(1000);
                ForgotPasswordPage.enterEmailForgotPW(Settings.Default.User);
                ForgotPasswordPage.submitForgotPW();

                //Any assertions if requried
                System.Threading.Thread.Sleep(1000);
                
                ForgotPasswordPage.checkForgotMailSent();
                outputText = "Forgot password mail sent.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                ForgotPasswordPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ForgotPasswordResults1.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ForgotPasswordResults1.png"));

                ForgotPasswordPage.backForgotPWMailSent();
                outputText = "Login page displayedas expected.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                assertPageTitle(Settings.Default.TitleHome);
                ForgotPasswordPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ForgotPasswordResults2.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ForgotPasswordResults2.png"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ForgotPasswordPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ForgotPasswordError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ForgotPasswordError.png"));
                throw;
            }
        }

        [Test]
        public void B_TheResetPasswordTest()
        {
            try
            {
                test = extent.StartTest("Reset Password Test", "Test reset password functionality is working after forgot password form is submitted.")
.AssignCategory(Settings.Default.Driver);
                //Get Token for Reset URL 
                DBActivateToken = ForgotPasswordPage.getLatestTokenDB();
                string currentResetToken = DBActivateToken["Token"];

                //Go to Reset URL
                ForgotPasswordPage.Goto(baseURL + Settings.Default.ResetPasswordSubURL + currentResetToken);

                //Test Steps go here
                ForgotPasswordPage.enterNewPassword(Settings.Default.Password);
                ForgotPasswordPage.reenterNewPassword(Settings.Default.Password);
                ForgotPasswordPage.submitNewPassword();

                //Any assertions if requried
                ForgotPasswordPage.assertPasswordChangedSuccess();

                Console.WriteLine("The message displayed '" + driver.FindElement(By.XPath("//*[@id='reset-password-page']/div[2]/div/div")).Text + "' as Expected");
                outputText = "The message displayed '" + driver.FindElement(By.XPath("//*[@id='reset-password-page']/div[2]/div/div")).Text + "' as Expected";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                ForgotPasswordPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ChangePasswordResults1.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ChangePasswordResults1.png"));


                //Take screenshot after Test
                //System.Threading.Thread.Sleep(1000);
               
                //ForgotPasswordPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ChangePasswordResults2.png");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ForgotPasswordPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ChangePasswordError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ChangePasswordError.png"));
                throw;
            }
        }
    }
}