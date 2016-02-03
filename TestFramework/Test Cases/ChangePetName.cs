using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using RelevantCodes.ExtentReports;


namespace TestFramework
{
    [TestFixture]
    public class ChangePetName : UsageHistoryPOM
    {
        public DashboardPOM PetNamePage;

        public class ExtentManager
        {
            private static ExtentReports extent;

            public static ExtentReports Instance
            {
                get {
                    return extent ??
                           (extent =
                               new ExtentReports(@"..\..\TestChangePetnameReport.html", true, DisplayOrder.NewestFirst));
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

            PetNamePage = new DashboardPOM(driver);

            baseURL = Settings.Default.BaseURL;
            verificationErrors = new StringBuilder();

            //// maximize window
            PetNamePage.MaximizeWindow();

            //Set implicit wait
            PetNamePage.setImplicitWait(30);

            extent.Config()
                .DocumentTitle("Change Pet Name Report")
                .ReportName("Change Pet Name")
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
        public void B_TheChangePetNameTest()
        {
            try
            {
                test = extent.StartTest("Change Pet Name Test", "Test to change the pet name of car.")
                    .AssignCategory(Settings.Default.Driver);
                //Got to base URL
                PetNamePage.Goto(baseURL);

                //Login
                PetNamePage.doLogin(PetNamePage.currentAccount);

                //Test Steps go here
                string petnamechange = "NewAutoName8";
                PetNamePage.waitForSpinnerDashboard();
                PetNamePage.waitForPlansToLoadDashboard();
                PetNamePage.goToPetName();

                PetNamePage.changePetName(petnamechange);
                PetNamePage.submitPetName();

                PetNamePage.waitForPlansToLoadDashboard();
                //Any assertions if requried
                CurrentPetName = driver.FindElement(By.XPath("//*[@id='device-triggers-list']/div/div/div/div/div[1]/label"));
                Console.WriteLine(CurrentPetName.Text);
                
                bool isChanged = PetNamePage.checkPetNameChange(petnamechange);
                if (isChanged == false)
                {
                    Console.WriteLine("The page is not displaying new Pet Name");
                    test.Log(LogStatus.Warning, "The page is not displaying new Pet Name");
                    throw new Exception("The page is not displaying new Pet Name");                  
                }

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PetNamePage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ChangePetNameResults.png");
                test.Log(LogStatus.Pass, "Pass<br />Pet Name change to '"+petnamechange+"' as expected.<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ChangePetNameResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PetNamePage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "ChangePetNameError.png");
                test.Log(LogStatus.Fail, "Fail: " + e.Message + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/ChangePetNameError.png"));
                throw;
            }
        }

        [Test]
        public void A_TheCancelChangePetNameTest()
        {
            try
            {
                test = extent.StartTest("Cancel Change Pet Name Test", "Test to cancel changing the pet name of car.")
                                .AssignCategory(Settings.Default.Driver); 
                //Got to base URL
                PetNamePage.Goto(baseURL);

                //Login
                PetNamePage.doLogin(PetNamePage.currentAccount);

                PetNamePage.waitForPlansToLoadDashboard();

                //Test Steps go here
                CurrentPetName = driver.FindElement(By.XPath("//*[@id='device-triggers-list']/div/div/div/div/div[1]/label"));
                string petnameOrig = CurrentPetName.Text;
                Console.WriteLine(petnameOrig);

                string petnamechange = "AutoNameNoChange";
                PetNamePage.goToPetName();
                PetNamePage.changePetName(petnamechange);
                PetNamePage.backPetName();

                PetNamePage.waitForPlansToLoadDashboard();

                //Any assertions if requried
                Console.WriteLine(CurrentPetName.Text);
                bool isChanged = PetNamePage.checkPetNameChange(petnameOrig);
                if (isChanged == false)
                {
                    Console.WriteLine("The Petname has changed from original name; This is not expected.");
                    throw new Exception("The Petname has changed from original name; This is not expected.");
                }

                //Take screenshot after Test
                System.Threading.Thread.Sleep(1000);
                PetNamePage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelChangePetNameResults.png");
                test.Log(LogStatus.Pass, "Pass<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CancelChangePetNameResults.png"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                PetNamePage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "CancelChangePetNameError.png"); 
                test.Log(LogStatus.Fail, "Fail: "+e.Message+"<br />Screenshot below: " + test.AddScreenCapture("Screenshots/CancelChangePetNameError.png"));
                throw;
            }
        }

    }
}