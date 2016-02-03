using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Text;
using System.Collections.Generic;
using RelevantCodes.ExtentReports;

namespace TestFramework
{
    [TestFixture]
     public class FAQ:FAQPOM
     {
         public FAQPOM FAQPage;

         public class ExtentManager
         {
             private static ExtentReports extent;

             public static ExtentReports Instance
             {
                 get {
                     return extent ??
                            (extent = new ExtentReports(@"..\..\TestFAQReport.html", true, DisplayOrder.NewestFirst));
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

             FAQPage = new FAQPOM(driver);

             baseURL = Settings.Default.BaseURL;
             verificationErrors = new StringBuilder();

             //// maximize window
             FAQPage.MaximizeWindow();

             //Set implicit wait
             FAQPage.setImplicitWait(30);

             // Save all settings
             Settings.Default.Save();

             extent.Config()
                    .DocumentTitle("FAQ Report")
                    .ReportName("FAQ")
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
         public void A_TheFAQTest()
         {
             try
             {
                 test = extent.StartTest("FAQ Test", "Test the FAQ questions and answers are displaying correctly.")
                                .AssignCategory(Settings.Default.Driver);
                 //Got to base URL
                 FAQPage.Goto(baseURL);

                 //Login
                 FAQPage.doLogin(FAQPage.currentAccount);

                 //Test Steps go here
                 //FAQPage.waitUntilElementExists("//*[@id='nav-left']/ul/li[3]/ul/li[2]/a");
                 FAQPage.waitForSpinnerDashboard();
                 FAQPage.waitForPlansToLoadDashboard();
                 FAQPage.goToFAQ();


                 IList<IWebElement> allQuestions = FAQPage.getAllQuestions();
                 IList<IWebElement> allPageAnswers = FAQPage.getAllAnswers();
                 FAQPage.clickALLQuestions(allQuestions, allPageAnswers);
                
                //Any assertions if requried
                 FAQPage.assertAllAnswers();

                 outputText = "The FAQ questions and answers displayed as expected.";
                 Console.WriteLine(outputText);
                 //Take screenshot after Test
                 System.Threading.Thread.Sleep(1000);
                 FAQPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "FAQResults.png");
                 test.Log(LogStatus.Pass, outputText + "<br />Screenshot below: " + test.AddScreenCapture("Screenshots/FAQResults.png"));
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.Message);
                 FAQPage.TakeScreenshot(@"" + Settings.Default.ScreenshotPath + "FAQError.png");
                 test.Log(LogStatus.Fail, outputText +"<br />Fail: " + e.Message + outputText+"<br />Screenshot below: " + test.AddScreenCapture("Screenshots/FAQError.png"));
                 throw;
             }
         }

     }
}
