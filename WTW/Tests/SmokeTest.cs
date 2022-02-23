using NUnit.Framework;
using SeleniumProject;
using OpenQA.Selenium;
using System.Collections.Generic;
using Setup;

namespace SmokeTests
{
    [TestFixture]
    public class WTWRecruitmentTest
    {
        Setup.Functions Func = new Setup.Functions();
        [SetUp]
        public void StartUpTest()
        {
            Func.Init();
        }
        [TearDown]
        public void EndTest()
        {
            //Uncomment below to close browser aftre test is completed.
           // Func.Close();
        }

        [Test]
        public void BasicTest()
        {
            //Open page
            Func.Goto("https://www.wtwco.com/en-us/solutions/insurance-consulting-and-technology");

            //Select region
            Func.ClickElement(By.XPath("//button[@data-menu='country-selector--is-visible']"));
            Func.ClickElement(By.XPath("//span[contains(text(), 'Americas')]"));
            Func.ClickElement(By.XPath("//a[@href='/en-US']"));

            //click search box and input search
            Func.ClickElement(By.XPath("//button/descendant::span[contains(text(), 'Search')]"));
            Func.SendKeysElement(By.XPath("//input[@title='Insert a query. Press enter to send']"), "IFRS 17");
            Func.SendKeysElement(By.XPath("//input[@title='Insert a query. Press enter to send']"), Keys.Return);

            //Landed on results.
            Func.FindTextExists("Results 1-10");

            //Check sorter
            Func.SelectSorter("Sort by Date in descending order", "Sort by Date in ascending order");

            //Filter - click checkbox by Article
            Func.ClickElement(By.XPath("//span[@data-original-value='Article']"));

            //Checking link prefixes
            Func.CheckLinkPrefix("//div[contains(@class, 'CoveoFieldValue')]/descendant::span", "https://www.wtwco.com/en-US/");
        }
    }
    
}