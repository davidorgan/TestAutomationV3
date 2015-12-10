using System;
using System.IO;
using OpenQA.Selenium;
using System.Collections.Generic;


namespace TestFramework
{
    public class FAQPOM : BasePageObject
    {
        public FAQPOM() {  }
        public List<string> allAnswers = new List<string>();

        public FAQPOM(IWebDriver driver)
        {
            this.driver = driver;

            allAnswers.Add(Settings.Default.FAQ1);
            allAnswers.Add(Settings.Default.FAQ2);
            allAnswers.Add(Settings.Default.FAQ3);
            allAnswers.Add(Settings.Default.FAQ4);
            allAnswers.Add(Settings.Default.FAQ5);
            allAnswers.Add(Settings.Default.FAQ6);
            allAnswers.Add(Settings.Default.FAQ7);
            allAnswers.Add(Settings.Default.FAQ8);
            allAnswers.Add(Settings.Default.FAQ9);
            allAnswers.Add(Settings.Default.FAQ10);
            allAnswers.Add(Settings.Default.FAQ11);
            allAnswers.Add(Settings.Default.FAQ12);
            allAnswers.Add(Settings.Default.FAQ13);
            allAnswers.Add(Settings.Default.FAQ14);
            allAnswers.Add(Settings.Default.FAQ15);
            allAnswers.Add(Settings.Default.FAQ16);
            allAnswers.Add(Settings.Default.FAQ17);
            allAnswers.Add(Settings.Default.FAQ18);
            allAnswers.Add(Settings.Default.FAQ19);
            allAnswers.Add(Settings.Default.FAQ20);
            allAnswers.Add(Settings.Default.FAQ21);
            allAnswers.Add(Settings.Default.FAQ22);
           
        }

        List<bool> QandAMatch = new List<bool>();
        bool isAnswer;

        public IList<IWebElement> getAllQuestions()
        {
            IWebElement qElement;
            IList<IWebElement> allQuestionElements = null;
            string log="";
            try
            {
                //Create array of all questions on page
                qElement = driver.FindElement(By.Id("accordion"));
                allQuestionElements = qElement.FindElements(By.TagName("strong"));
                log = allQuestionElements.ToString();
                if (allQuestionElements.Equals(null))
                {
                    throw new Exception("List of elements is empty");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(log);
            return allQuestionElements;
        }

        public IList<IWebElement> getAllAnswers()
        {
            IWebElement qElement;
            IList<IWebElement> allanswerElements = null;
            try
            {
                //Create array of all questions on page
                qElement = driver.FindElement(By.Id("accordion"));
                allanswerElements = qElement.FindElements(By.TagName("p"));
                if (allanswerElements == null)
                {
                    throw new Exception("List of elements is empty");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return allanswerElements;
        }

        public void clickQuestion(IList<IWebElement> questions ,int id)
        {
            int checkIndex = id;
            try
            {
                int i;
                for(i=0;i<questions.Count;i++)
                {
                    if(i == checkIndex)
                    {
                        questions[i].Click();
                    }
                    else if (checkIndex > questions.Count)
                    {
                        throw new Exception("Question with Id = " + id + " was not found");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool checkAnswer(IList<IWebElement> answers, int id)
        {
            //int[] allText = new int[answers.Count];
            int checkIndex = id;
            try
            {
                int i;
                for (i = 0; i < answers.Count; i++)
                {
                    if (i == checkIndex)
                    {
                        string inner = answers[i].Text;
                        // Set a variable to the My Documents path.
                        string mydocpath =
                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        // Write the string array to a new file named "WriteLines.txt".
                        using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\WriteLines.txt"))
                        {
                                outputFile.WriteLine(inner+";");
                                outputFile.WriteLine(allAnswers[checkIndex]+";");
                        }
                        
                        if (inner.Contains(allAnswers[checkIndex]))
                        {
                            return true;
                        }

                        throw new Exception(inner + " does not equal " + allAnswers[checkIndex] + ". i");
                    }
                    else if (checkIndex > answers.Count)
                    {
                        throw new Exception("Question with Id = " + id + " was not found");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public void clickALLQuestions(IList<IWebElement> allQuestions, IList<IWebElement> allPageAnswers)
        {
            for (int i = 0; i < allQuestions.Count; i++)
            {
                System.Threading.Thread.Sleep(500);
                clickQuestion(allQuestions, i);
                System.Threading.Thread.Sleep(500);
                isAnswer = checkAnswer(allPageAnswers, i);
                QandAMatch.Add(isAnswer);

                if (isAnswer == false)
                {
                    int qNum = i + 1;
                    outputText = "<br />There was a mismatch for question " + qNum + ": " + allQuestions[i].Text;
                    outputText += "<br />Actual answer was : " + allPageAnswers[i].Text;
                    outputText += "<br />Expected answer should contain : " + allAnswers[i];
                    outputText += "<br />------------------------------------------------------------------";
                    Console.WriteLine(outputText);
                }
                clickQuestion(allQuestions, i);

            }
        }

        public bool assertAllAnswers()
        {
            for (int i = 0; i < QandAMatch.Count; i++)
            {
                if (QandAMatch[i] == false)
                {
                    Console.WriteLine("There was a mismatch");
                    throw new Exception("There was a mismatch");
                }
            }
            return true;
        }
        //Nav Menu "GoTo" links
        public void goToFAQ()
        {
            try
            {
                /*Actions action = new Actions(driver);
                IWebElement we = driver.FindElement(By.XPath("//*[@id='nav-left']/ul/li[2]"));
                
                action.MoveToElement(we).MoveToElement(driver.FindElement(By.XPath("//*[@id='nav-left']/ul/li[2]/ul/li[2]/a"))).Click().Build().Perform();*/

                Goto(Settings.Default.BaseURL+"Support/FAQ");
            }
            catch (ElementNotVisibleException e)
            {
                throw new Exception("Link to FAQ not found: " + e.Message);
            }
        }
        
    }
}
