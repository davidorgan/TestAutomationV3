using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class CreditCardManagement : CreditCardManagementPOM
    {
        public CreditCardManagementPOM CreditCardManagementPage;

        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestCreditCardManagementReport.html", true,
                                   DisplayOrder.NewestFirst));
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

            CreditCardManagementPage = new CreditCardManagementPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            CreditCardManagementPage.MaximizeWindow();

            //Set implicit wait
            CreditCardManagementPage.setImplicitWait(30);

            extent.Config()
                    .DocumentTitle("Credit Card Report")
                    .ReportName("Credit Card")
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
        public void A_TheCheckCreditCardTest()
        {
            try
            {
                test = extent.StartTest("Check Credit Card Test", "Test to check the credit card displays as expected.")
                                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                CreditCardManagementPage.Goto(baseURL);

                //Login
                CreditCardManagementPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                CreditCardManagementPage.waitForSpinnerDashboard();
                CreditCardManagementPage.waitForPlansToLoadDashboard();
                CreditCardManagementPage.goToCreditCardManagement();
                bool ccPresent = CreditCardManagementPage.assertCreditCardIsPresent(Settings.Default.CurrentCreditCardString);

                //Any assertions if requried
                outputText = ccPresent ? "The expected credit card is displayed" : "There is no credit card associated to this account";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                CreditCardManagementPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckCreditCardResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CheckCreditCardResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CreditCardManagementPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CheckCreditCardError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CheckCreditCardError.png"));
                throw;
            }
        }

        [Test, Explicit]
        public void C_TheCheckNoCreditCardTest()
        {
            try
            {
                test = extent.StartTest("Check No Credit Card Test", "Test to check no credit card displays as expected.")
                .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                CreditCardManagementPage.Goto(baseURL);

                //Login
                CreditCardManagementPage.doLogin(Settings.Default.User, Settings.Default.Password);


                //Test Steps go here
                CreditCardManagementPage.waitForSpinnerDashboard();
                CreditCardManagementPage.waitForPlansToLoadDashboard();
                CreditCardManagementPage.goToCreditCardManagement();

                //Any assertions if requried
                CreditCardManagementPage.assertCreditCardIsNotPresent(Settings.Default.NoCardString);
                outputText = "No card displayed as Expected.";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                CreditCardManagementPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "NoCreditCardResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/NoCreditCardResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CreditCardManagementPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "NoCreditCardError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/NoCreditCardError.png"));
                throw;
            }
        }

        [Test, Explicit]
        public void B_TheDeleteCreditCardTest()
        {
            try
            {
                test = extent.StartTest("Remvoe Credit Card Test", "Test to remove the credit card on the account.")
.AssignCategory(Settings.Default.Driver);
                //Got to base URL
                CreditCardManagementPage.Goto(baseURL);

                //Login
                CreditCardManagementPage.doLogin(Settings.Default.User, Settings.Default.Password);

                //Test Steps go here
                CreditCardManagementPage.waitForSpinnerDashboard();
                CreditCardManagementPage.waitForPlansToLoadDashboard();
                CreditCardManagementPage.goToCreditCardManagement();   

                CreditCardManagementPage.assertCreditCardIsPresent(Settings.Default.CurrentCreditCardString);

                //Any assertions if requried
                Console.WriteLine("The expected credit card is displayed");
                CreditCardManagementPage.deleteCard();
                System.Threading.Thread.Sleep(500);

                if (isSponsorPopUpDisplayed())
                {
                    throw new Exception("Card cannot be deleted as this is a Sponsor User with In Car Top Up.");
                }

                CreditCardManagementPage.assertCreditCardIsNotPresent(Settings.Default.NoCardString);
                outputText = "Card should now be deleted";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                CreditCardManagementPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DeleteCreditCardResults.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DeleteCreditCardResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CreditCardManagementPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "DeleteCreditCardError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/DeleteCreditCardError.png"));
                throw;
            }
        }

    }
}