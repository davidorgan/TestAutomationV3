using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class UpdateAccount : AccountDetailsPOM
    {
        private AccountDetailsPOM AccountDetailsPage;

        private class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestUpdateAccountReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }

        private ExtentReports extent = ExtentManager.Instance;
        private ExtentTest test;

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

            AccountDetailsPage = new AccountDetailsPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            AccountDetailsPage.MaximizeWindow();

            //Set implicit wait
            AccountDetailsPage.setImplicitWait(30);

            extent.Config()
                    .DocumentTitle("Update Account Report")
                    .ReportName("Update Account")
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
        public void C_TheUpdateAccountDetailsTest()
        {
            try
            {
                test = extent.StartTest("Update Account Test", "Test the Update account details functionality.")
                                .AssignCategory(Settings.Default.Driver);

                //Got to base URL
                AccountDetailsPage.Goto(baseURL);

                //Login
                AccountDetailsPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                AccountDetailsPage.goToAccountDetails();
                AccountDetailsPage.enterAccountUpdates();
                AccountDetailsPage.submitUpdateAccount();
                
                //Any assertions if requried
                AccountDetailsPage.assertAccountUpdatedMessage();
                outputText = "Account updated and success message displayed.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                AccountDetailsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UpdateAccountResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/UpdateAccountResults.png"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                AccountDetailsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UpdateAccountError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/UpdateAccountError.png"));
                throw;
            }
        }

        [Test]
        public void A_TheUpdatePasswordTest()
        {
            try
            {
                test = extent.StartTest("Update Password Test", "Test the Update password functionality.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                AccountDetailsPage.Goto(baseURL);

                //Login
                AccountDetailsPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                AccountDetailsPage.waitForSpinnerDashboard();
                AccountDetailsPage.waitForPlansToLoadDashboard();

                AccountDetailsPage.goToAccountDetails();
                AccountDetailsPage.accountEnterCurrentPassword(Settings.Default.Password);
                AccountDetailsPage.accountEnterNewPassword(Settings.Default.AltPassword);
                AccountDetailsPage.accountEnterConfirmNewPassword(Settings.Default.AltPassword);

                AccountDetailsPage.submitUpdatePassword();

                //Any assertions if requried
                System.Threading.Thread.Sleep(1000);
                AccountDetailsPage.assertUpdatePasswordSuccess();
                outputText = "Password updated as expected and success message displayed.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                AccountDetailsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UpdatePasswordResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/UpdatePasswordResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                AccountDetailsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "UpdatePasswordError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/UpdatePasswordError.png"));
                throw;
            }
        }

        [Test]
        public void B_TheResetPasswordTest()
        {
            try
            {
                test = extent.StartTest("Reset Password Test", "Test to reset the password to original default value.")
                            .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                AccountDetailsPage.Goto(baseURL);

                //Login
                AccountDetailsPage.doLogin(Settings.Default.User, Settings.Default.AltPassword);

                //Test Steps go here
                AccountDetailsPage.waitForSpinnerDashboard();
                AccountDetailsPage.waitForPlansToLoadDashboard();

                AccountDetailsPage.goToAccountDetails();
                AccountDetailsPage.accountEnterCurrentPassword(Settings.Default.AltPassword);
                AccountDetailsPage.accountEnterNewPassword(Settings.Default.Password);
                AccountDetailsPage.accountEnterConfirmNewPassword(Settings.Default.Password);

                AccountDetailsPage.submitUpdatePassword();

                //Any assertions if requried
                System.Threading.Thread.Sleep(1000);
                AccountDetailsPage.assertUpdatePasswordSuccess();
                outputText = "Password updated as expected and success message displayed.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                AccountDetailsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ResetPasswordResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ResetPasswordResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                AccountDetailsPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ResetPasswordError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ResetPasswordError.png"));
                throw;
            }
        }

    }
}
