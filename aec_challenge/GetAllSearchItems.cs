using OpenQA.Selenium;

namespace aec_challenge
{
    internal class GetAllSearchItems
    {
        public static void GetInfos(IWebDriver driver, int timeWaitElements)
        {
            // Barra de pesquisa
            dynamic searchElement = Utils.GetElements(driver, "//input[@id='header-barraBusca-form-campoBusca']", timeWaitElements);
            searchElement[0].SendKeys("RPA");

            // Botão "Pesquisar" (lupa)
            dynamic buttonSearchElement = Utils.GetElements(driver, "//button[@class='header__nav--busca-submit']", timeWaitElements);
            buttonSearchElement[0].Click();

            // Loop para percorrer por cada item do resultado da pesquisa
            dynamic titleElements = Utils.GetElements(driver, "//h4[@class='busca-resultado-nome']", timeWaitElements);
            for (int i = 0; i < titleElements.Count; i++)
            {
                GetSearchItem.GetItem(driver, titleElements, timeWaitElements, i);
            }
        }
    }
}
