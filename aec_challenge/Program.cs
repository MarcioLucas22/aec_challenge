using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace aec_challenge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataHandler.CreateTable();                

            for (int i = 1; i <= 3; i++)
            {
                WebDriverConfig config = new WebDriverConfig();
                IWebDriver driver = config.Config();

                // Se der algum erro, aumenta 1 segundo no tempo de espera do elemento
                int timeWaitElements = i;

                try
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    GetAllSearchElements getElements = new GetAllSearchElements();
                    getElements.GetInfos(driver, timeWaitElements);

                    driver.Quit();

                    stopwatch.Stop();                    

                    double elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                    Console.WriteLine($"Tempo de execução: {elapsedSeconds} segundos");
                    Console.ReadLine();
                    break;
                }
                catch (Exception ex)
                {
                    driver.Quit();
                    Console.WriteLine("Erro: " + ex.ToString());
                }
            }
        }
    }
}
