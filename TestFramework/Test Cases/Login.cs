using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    
    [TestFixture]
    public class Login : DashboardPOM
    {
        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent = new ExtentReports(@"..\..\TestLoginReport.html", true, DisplayOrder.NewestFirst));
                }
            }
        }
        public ExtentReports extent = ExtentManager.Instance;
        public ExtentTest test;

        public DashboardPOM LoginPage;

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

            LoginPage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            LoginPage.MaximizeWindow();

            //Set implicit wait
            LoginPage.setImplicitWait(30);

            extent.Config()
                .DocumentTitle("Login Report")
                .ReportName("Login")
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
        public void B_TheReloadTest()
        {
            try
            {
                test = extent.StartTest("Reload Test", "Test to ensure reload plans button works.")
                                .AssignCategory(Settings.Default.Driver); 
               // Test.Log(LogStatus.Info, "Tested in: "+Settings.Default.Driver);

                //Got to base URL
                LoginPage.Goto(baseURL);
                //Test.Log(LogStatus.Info, "Go to Base URL: "+Settings.Default.BaseURL);

                //Login
                LoginPage.doLogin(Settings.Default.User, Settings.Default.Password);
                //Test.Log(LogStatus.Info, "Login to Account");


                LoginPage.ReloadPlan();
                //Test.Log(LogStatus.Info, "Reload Plan");

                //Take screenshot after Test
                System.Threading.Thread.Sleep(3000);
                LoginPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ReloadResults.png");
                test.Log(LogStatus.Pass, "Screenshot below: " + test.AddScreenCapture("Screenshots/ReloadResults.png"));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                LoginPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ReloadError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ReloadError.png"));               
                throw;
            }
        }

        [Test]
        public void A_TheLoginTest()
        {
            try
            {
                test = extent.StartTest("Login Test", "Test to ensure Login functionality works.");
                test.Log(LogStatus.Info, "Tested in: " + Settings.Default.Driver);
                //Got to base URL
                LoginPage.Goto(baseURL);
                //Test.Log(LogStatus.Info, "Go to Base URL: " + Settings.Default.BaseURL);

                //Login
                LoginPage.doLogin(Settings.Default.User, Settings.Default.Password);
                //Test.Log(LogStatus.Info, "Login to Account");

                //Wait for page title
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(ExpectedConditions.TitleContains(Settings.Default.TitleDashboard));
                
                //Assert Page
                string currentPageTitle = driver.Title;
                Console.WriteLine("Current Page title is: "+currentPageTitle);
                assertPageTitle(currentPageTitle, Settings.Default.TitleDashboard);
                //Test.Log(LogStatus.Info, "Page title displayed is as Expected: " + currentPageTitle);

                //Take screenshot after Test
                LoginPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "LoginResults.png");
                test.Log(LogStatus.Pass, "Pass<br />Screenshot below: " + test.AddScreenCapture("Screenshots/LoginResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                LoginPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "LoginError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/LoginError.png"));
                throw;
            }
        }

    }
}
