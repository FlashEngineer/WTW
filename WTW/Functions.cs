using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using System.Collections.Generic;
using SeleniumProject;

namespace Setup
{
    public class Functions
    {
        private IWebDriver webDriver;

        public void Init()
        {
            Browsers.Init();
        }

        public void Close()
        {
            Browsers.Close();
        }

        public void Goto(string url)
        {
            Browsers.Goto(url);
        }

        //Helper fucntion to find text is on page
        private bool FindElementTextContains(string filtertext)
        {
            webDriver = Browsers.getDriver;
            try
            {
                IWebElement Element = webDriver.FindElement(By.XPath(("//*[contains(text(), \"" + filtertext + "\")]")));
                if (Element != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine($"Unable to locate Text '{filtertext}'.{ e.Message}");
                return false;
            }

        }

        //Helper function to wait for element to be valid
        private Func<IWebDriver, IList<IWebElement>> ElementWait(string XPath)
        {
            webDriver = Browsers.getDriver;
            return ((X) =>
            {
                if (webDriver.FindElements(By.XPath(XPath)).Count > 0)
                {
                    return webDriver.FindElements(By.XPath(XPath));
                }
                else
                {
                    Console.WriteLine("Element not ready");
                    return null;
                }
            });
        }

        //Helper to check if element is enabled.
        private Func<IWebDriver, IWebElement> SingleElementWait(By filter)
        {
            webDriver = Browsers.getDriver;
            return ((X) =>
            {
                if (webDriver.FindElement(filter).Enabled && webDriver.FindElement(filter).Displayed)
                {
                    return webDriver.FindElement(filter);
                }
                else
                {
                    Console.WriteLine("Element not enabled");
                    return null;
                }
            });
        }

        //Main find element
        private IWebElement FindElement(By by)
        {
            webDriver = Browsers.getDriver;
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
            IWebElement ElementInQuestion = wait.Until(SingleElementWait(by));
            return webDriver.FindElement(by);
        }

        //Send value to element
        public void SendKeysElement(By by, string value)
        {
            FindElement(by).SendKeys(value);
            Console.WriteLine(string.Format("[{0}] - SendKeys value [{1}] ", DateTime.Now.ToString("HH:mm:ss.fff"), value));
        }

        //Click element
        public void ClickElement(By by)
        {
            FindElement(by).Click();
        }

        //Select sorter for 2 options - ascend/descend
        public void SelectSorter(string value1, string value2)
        {
            IWebElement element = FindElement(By.XPath("//span[@aria-label='" + value1 +"']"));
            IWebElement element2 =FindElement(By.XPath("//span[@aria-label='" + value2 + "']"));
            if (element.GetAttribute("aria-checked").Equals("false") && element2.GetAttribute("aria-checked").Equals("false"))
            {
                //Click Descending order since no preference.
                element.Click();
            }
        }

        //Select sorter for only single option
        public void SelectSorter(string value1)
        {
            IWebElement element = FindElement(By.XPath("//span[@aria-label='" + value1 + "']"));
            if (element.GetAttribute("aria-checked").Equals("false"))
            {
                element.Click();
            }
        }

        //Find prefixes of dispalyed links
        public void CheckLinkPrefix(string TextElement, string LinkPrefix)
        {
            Thread.Sleep(5000);
            webDriver = Browsers.getDriver;
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(60));
            IList<IWebElement> ListToBeIterated = wait.Until(ElementWait(TextElement));

            foreach (IWebElement elem in ListToBeIterated)
            {
                string LinkText = elem.Text;
                Console.WriteLine("Link shown is: {0}", LinkText);
                Assert.True(LinkText.StartsWith(LinkPrefix));
            }
        }

        //Find text of element
        public void FindTextExists(string searchForText = null)
        {
            webDriver = Browsers.getDriver;
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);

            //Checks if it lands on the page with specifc text
            Assert.IsTrue(FindElementTextContains(searchForText));

        }
    }
}
