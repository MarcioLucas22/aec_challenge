using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace aec_challenge
{
    internal class GetAllSearchElements
    {
        public static dynamic GetElement(IWebDriver driver, string xpath, int timeWait)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeWait));

                var element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));

                return element;
            }
            catch
            {
                return "Elemento não encontrado";
            }
        }

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

        public void GetInfos(IWebDriver driver, int timeWaitElements)
        {
            // Barra de pesquisa
            dynamic searchElement = GetElement(driver, "//input[@id='header-barraBusca-form-campoBusca']", timeWaitElements);
            searchElement.SendKeys("RPA");

            // Botão "Pesquisar" (lupa)
            dynamic buttonSearchElement = GetElement(driver, "//button[@class='header__nav--busca-submit']", timeWaitElements);
            buttonSearchElement.Click();

            // Loop para percorrer por cada item do resultado da pesquisa
            dynamic titleElements = GetElements(driver, "//h4[@class='busca-resultado-nome']", timeWaitElements);
            for (int i = 0; i < titleElements.Count; i++)
            {
                // Necessário pegar o elemento "title" novamente pois quando muda de página e volta, o contexto do driver muda
                titleElements = GetElements(driver, "//h4[@class='busca-resultado-nome']", timeWaitElements);
                dynamic descriptionElements = GetElements(driver, "//p[@class='busca-resultado-descricao']", timeWaitElements);

                string title = titleElements[i].Text.ToString();
                string description = descriptionElements[i].Text.ToString();

                titleElements[i].Click();

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
}
