using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class FooterLinks : DashboardPOM
    {
        public DashboardPOM foot;

        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestFooterLinksReport.html", true, DisplayOrder.NewestFirst));
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

            foot = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            foot.MaximizeWindow();

            //Set implicit wait
            foot.setImplicitWait(30);

            extent.Config()
       .DocumentTitle("Footer Links Report")
       .ReportName("Footer Links")
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
        [Ignore("The link is no longer used and does not need to be tested.")]
        public void A_TheDataInsideFooterTest()
        {
            try
            {
                test = extent.StartTest("Data Inside Test", "Test the Data Inside Footer Link is working.")
.AssignCategory(Settings.Default.Driver);
                //Got to base URL
                foot.Goto(baseURL);

                //Login
                foot.doLogin(foot.currentAccount);

                //Test Steps go here
                foot.goToAboutDataInside();

                foot.switchToLastWindow();
                //Any assertions if requried

                outputText = "Test Script incomplete. Improved Assertions to be added.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DataInsideResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DataInsideResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DataInsideError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DataInsideError.png"));
                throw;
            }
        }

        [Test]
        public void B_TheTandCFooterTest()
        {
            try
            {
                test = extent.StartTest("Terms and Conditions Test", "Test the Terms and Conditions Footer Link is working.")
.AssignCategory(Settings.Default.Driver);
                //Got to base URL
                foot.Goto(baseURL);

                //Login
                foot.doLogin(foot.currentAccount);

                //Test Steps go here
                foot.goToTandC();

                foot.switchToLastWindow();

                //Any assertions if requried
                outputText = "Test Script incomplete. Improved Assertions to be added.";
                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "TandCResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/TandCResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "TandCError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/TandCError.png"));
                throw;
            }
        }

        [Test]
        public void C_ThePrivacyPolicyTest()
        {
            try
            {
                test = extent.StartTest("Privacy Policy Test", "Test the Privacy Policy Footer Link is working.")
.AssignCategory(Settings.Default.Driver);
                //Got to base URL
                foot.Goto(baseURL);

                //Login
                foot.doLogin(foot.currentAccount);

                //Test Steps go here
                foot.goToPrivacyPolicy();

                foot.switchToLastWindow();

                //Any assertions if requried
                outputText = "Test Script incomplete. Improved Assertions to be added.";
                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PrivacyPolicyResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PrivacyPolicyResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PrivacyPolicyError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PrivacyPolicyError.png"));
                throw;
            }
        }


        [Test]
        public void D_TheContactUsFooterTest()
        {
            try
            {
                test = extent.StartTest("Contact Us Link Test", "Test the Contact Us Footer Link is working.")
.AssignCategory(Settings.Default.Driver);
                //Got to base URL
                foot.Goto(baseURL);

                //Login
                foot.doLogin(foot.currentAccount);

                //Test Steps go here
                foot.goToFooterContact();

                //Any assertions if requried
                outputText = "Test Script incomplete. Improved Assertions to be added.";
                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                foot.TakeScreenshot(@""+ Settings.Default.ScreenshotPath +"ContactUsFooterResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ContactUsFooterResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ContactUsFooterError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ContactUsFooterError.png"));
                throw;
            }
        }

        [Test]
        public void E_ThePoweredByAudiFooterTest()
        {
            try
            {
                test = extent.StartTest("Powered By Audi Link Test", "Test the Powered By Audi Footer Link is working.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                foot.Goto(baseURL);

                //Login
                foot.doLogin(foot.currentAccount);

                //Test Steps go here
                foot.waitForPlansToLoadDashboard();
                foot.goToPoweredBy();

                //Any assertions if requried
                outputText = "Test Script incomplete. Improved Assertions to be added.";
                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PoweredByAudiFooterResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PoweredByAudiFooterResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "PoweredByAudiFooterError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/PoweredByAudiFooterError.png"));
                throw;
            }
        }


        [Test]
        public void F_TheCopyrightTextFooterTest()
        {
            try
            {
                test = extent.StartTest("Copyright Footer Text Test", "Test the Copyright text is displayed correctly in footer.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                foot.Goto(baseURL);

                //Login
                foot.doLogin(foot.currentAccount);

                //Test Steps go here
                foot.waitForPlansToLoadDashboard();               

                //Any assertions if requried
                foot.assertCopyrightText();
                outputText = "Copyright text displayed as expected.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CopyrightTextResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CopyrightTextResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                foot.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CopyrightTextError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CopyrightTextError.png"));
                throw;
            }
        }

    }
}