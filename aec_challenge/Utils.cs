using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace aec_challenge
{
    internal class Utils
    {
        public static dynamic GetElements(IWebDriver driver, string xpath, int timeWait)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeWait));

                Func<IWebDriver, IList<IWebElement>> locateElements = d =>
                    d.FindElements(By.XPath(xpath));

                var elements = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath(xpath)));

                return elements;
            }
            catch
            {
                return "Elemento não encontrado";
            }

        }

        public static bool IsWebElement(dynamic element)
        {
            if (element is IWebElement || element is IList<IWebElement>)
            {
                return true;
            }

            return false;
        }

        public static string FindFirstElement(IWebDriver driver, string[] xpaths, string notFoundMessage, int timeWait)
        {
            foreach (string xpath in xpaths)
            {
                var elements = GetElements(driver, xpath, timeWait);
                if (IsWebElement(elements))
                {
                    return elements[0].Text;
                }
            }
            return notFoundMessage;
        }
    }
}
