using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Reflection.Emit;

namespace aec_challenge
{
    internal class WebDriverConfig
    {
        public IWebDriver Config()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--log-level=3");

            IWebDriver driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl("https://www.alura.com.br/");

            return driver;
        }
    }
}
