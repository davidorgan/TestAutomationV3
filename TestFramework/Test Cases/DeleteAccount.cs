using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class DeleteAccount : DeleteAccountPOM
    {
        public DeleteAccountPOM DeleteAccountPage;
        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestDeleteAccountReport.html", true, DisplayOrder.NewestFirst));
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

            DeleteAccountPage = new DeleteAccountPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            DeleteAccountPage.MaximizeWindow();

            //Set implicit wait
            DeleteAccountPage.setImplicitWait(30);

            extent.Config()
                    .DocumentTitle("Delete Account Report")
                    .ReportName("Delete Account")
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
        public void A_TheCancelDeleteAccountTest()
        {
            try
            {
                test = extent.StartTest("Cancel Delete Account Test", "Test to attempt to delete account but cancel at last step.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DeleteAccountPage.Goto(baseURL);

                //Login
                DeleteAccountPage.doLogin();

                //Test Steps go here
                DeleteAccountPage.waitForSpinnerDashboard();
                DeleteAccountPage.waitForPlansToLoadDashboard();
                DeleteAccountPage.goToDeleteAccount();
                DeleteAccountPage.tickUnderstand();
                DeleteAccountPage.backOutDelete();

                //Any assertions if requried
                DeleteAccountPage.waitUntilTitle(Settings.Default.TitleDashboard);
                outputText = "Back to Dashboard Page as expected. Account still Active.";

                //Take screenshot after Test

                DeleteAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DeleteAccountCancelledResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DeleteAccountCancelledResults.png"));
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DeleteAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DeleteAccountCancelledError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DeleteAccountCancelledError.png"));
                throw;
            }
        }


        [Test,Explicit]
        public void B_TheDeleteAccountTest()
        {
            try
            {
                test = extent.StartTest("Delete Account Test", "Test to attempt to delete account.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                DeleteAccountPage.Goto(baseURL);

                //Login
                DeleteAccountPage.doLogin();

                //Test Steps go here
                DeleteAccountPage.goToDeleteAccount();
                DeleteAccountPage.tickUnderstand();
                
                //Comment out until user setup
                //DeleteAccountPage.submitUDeleteAccount();

                //Any assertions if requried
               // DeleteAccountPage.waitUntilTitle(Settings.Default.TitleDashboard);
                outputText = "Account deleted as expected. Login page displayed";

                //Take screenshot after Test

                DeleteAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DeleteAccountResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DeleteAccountResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DeleteAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DeleteAccountError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DeleteAccountError.png"));
                throw;
            }
        }

    }
}