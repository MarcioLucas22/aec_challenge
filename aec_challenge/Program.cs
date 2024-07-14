using OpenQA.Selenium;
using System;
using System.Diagnostics;

namespace aec_challenge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Cria a tabela no SQLite caso não exista
            DataHandler.CreateTable();                

            for (int i = 1; i <= 3; i++)
            {
                IWebDriver driver = WebDriverConfig.Config();

                // Se der algum erro, aumenta 1 segundo no tempo de espera do elemento
                int timeWaitElements = i;

                try
                {
                    // Inicia tempo de execução
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    GetAllSearchItems.GetInfos(driver, timeWaitElements);

                    driver.Quit();

                    // Finaliza tempo de execução
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
