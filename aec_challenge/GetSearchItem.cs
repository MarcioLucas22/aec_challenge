using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aec_challenge
{
    internal class GetSearchItem
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

        static string FindFirstElement(IWebDriver driver, string[] xpaths, string notFoundMessage, int timeWait)
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

        public void getItem(IWebDriver driver, dynamic titleElements, int timeWaitElements, int index)
        {
            // Necessário pegar o elemento "title" novamente pois quando muda de página e volta, o contexto do driver muda
            titleElements = GetElements(driver, "//h4[@class='busca-resultado-nome']", timeWaitElements);
            dynamic descriptionElements = GetElements(driver, "//p[@class='busca-resultado-descricao']", timeWaitElements);

            string title = titleElements[index].Text.ToString();
            string description = descriptionElements[index].Text.ToString();

            titleElements[index].Click();

            // Pode ter mais de um Xpath para obter o instrutor
            string[] instructorXPaths =
            {
                    "//div[@class='cosmos-author-name']",
                    "//h3[@class='instructor-title--name']",
                    "//h3[@class='formacao-instrutor-nome']"
                };

            string instructor = FindFirstElement(driver, instructorXPaths, "Intrutor não encontrado", timeWaitElements);

            // Pode ter mais de um Xpath para obter a carga horária
            string[] workloadXPaths =
            {
                    "//p[@class='courseInfo-card-wrapper-infos']",
                    "//div[@class='formacao__info-destaque']",
                    "//p[@class='episode-programming-time']"
                };

            string workload = FindFirstElement(driver, workloadXPaths, "Carga horária não encontrada", timeWaitElements);

            DataHandler.InsertData(title, description, instructor, workload);

            driver.Navigate().Back();
        }
    }
}
