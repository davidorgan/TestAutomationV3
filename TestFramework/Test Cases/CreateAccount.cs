using NUnit.Framework;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
    public class CreateAccount : CreateAccountPOM
    {
        public CreateAccountPOM CreateAccountPage;

        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestCreateAccountReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }
        public ExtentReports extent = ExtentManager.Instance;
        public ExtentTest test;

        [SetUp]
        public void SetupTest()
        {

            mailNum = Settings.Default.NewMailCounter.ToString();


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

            CreateAccountPage = new CreateAccountPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            CreateAccountPage.MaximizeWindow();

            //Set implicit wait
            CreateAccountPage.setImplicitWait(30);

            extent.Config()
                    .DocumentTitle("Create Account Report")
                    .ReportName("Create Account")
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

        [Test,Explicit]
        public void A_TheCreateAccountTest()
        {
            try
            {
                test = extent.StartTest("Create Account Test", "Test to create account.")
                .AssignCategory(Settings.Default.Driver);

                //Got to base URL
                CreateAccountPage.Goto(baseURL);
                
                //Test Steps go here
                CreateAccountPage.goToCreateAccount();
                System.Threading.Thread.Sleep(1000);
                CreateAccountPage.enterNewAccountDetails();
                CreateAccountPage.tickAgreeTandC();
                CreateAccountPage.submitCreateAccount();

                //Any assertions if requried
                System.Threading.Thread.Sleep(1000);
                CreateAccountPage.assertAccountCreated();
                outputText = "Account created";

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                CreateAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CreateAccountResults1.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CreateAccountResults1.png"));

                CreateAccountPage.goBackToLogin();
                outputText = "Login page displayed";
                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                CreateAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CreateAccountResults2.png");
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CreateAccountResults2.png"));
                
                //Check to ensure the customer has been added to the Database table
                CreateAccountPage.assertCustomerAddedtoDB(newEmail);

                test.Log(LogStatus.Pass, outputText);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CreateAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CreateAccountError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CreateAccountError.png"));
                throw;
            }
        }

        [Test, Explicit]
        public void B_TheActivateAccountTest()
        {
            try
            {
                test = extent.StartTest("Activate Account Test", "Test to activate account.")
.AssignCategory(Settings.Default.Driver);

                //Test Steps go here
                //Go to Activation URL
                CreateAccountPage.goToActivatePage();

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                CreateAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ActivateAccountResults1.png");       
                CreateAccountPage.assertPageTitle(Settings.Default.TitleHome);

                outputText = "Activate link worked as intended.";
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ActivateAccountResults1.png"));

                CreateAccountPage.doLogin(newEmail, newPassword);

                System.Threading.Thread.Sleep(1000);
                CreateAccountPage.assertPageTitle(Settings.Default.TitleAddVin);

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                CreateAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ActivateAccountResults2.png");

                outputText = "Account active for User: "+newEmail+"";
                test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ActivateAccountResults2.png"));

                Settings.Default.NewMailCounter = Settings.Default.NewMailCounter + 1;
                Settings.Default.Save();
                Settings.Default.Reload();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CreateAccountPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ActivateAccountError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ActivateAccountError.png"));
                throw;
            }
        }

    }
}