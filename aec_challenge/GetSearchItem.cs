using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace aec_challenge
{
    internal class GetSearchItem
    {
        public static void GetItem(IWebDriver driver, dynamic titleElements, int timeWaitElements, int index)
        {
            List<SearchResult> searchResults = DataHandler.GetAllTitles();

            // Necessário pegar o elemento "title" novamente pois quando muda de página e volta, o contexto do driver muda
            titleElements = Utils.GetElements(driver, "//h4[@class='busca-resultado-nome']", timeWaitElements);
            dynamic descriptionElements = Utils.GetElements(driver, "//p[@class='busca-resultado-descricao']", timeWaitElements);

            string title = titleElements[index].Text.ToString();
            string description = descriptionElements[index].Text.ToString();

            bool titleExists = searchResults.Any(sr => sr.Title == title);
            if (!titleExists)
            {
                titleElements[index].Click();

                // Pode ter mais de um Xpath para obter o instrutor
                string[] instructorXPaths =
                {
                    "//div[@class='cosmos-author-name']",
                    "//h3[@class='instructor-title--name']",
                    "//h3[@class='formacao-instrutor-nome']"
                };

                string instructor = Utils.FindFirstElement(driver, instructorXPaths, "Intrutor não encontrado", timeWaitElements);

                // Pode ter mais de um Xpath para obter a carga horária
                string[] workloadXPaths =
                {
                    "//p[@class='courseInfo-card-wrapper-infos']",
                    "//div[@class='formacao__info-destaque']",
                    "//p[@class='episode-programming-time']"
                };

                string workload = Utils.FindFirstElement(driver, workloadXPaths, "Carga horária não encontrada", timeWaitElements);

                DataHandler.InsertData(title, description, instructor, workload);

                driver.Navigate().Back();
            }
        }
    }
}
