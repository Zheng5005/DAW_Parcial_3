using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;


namespace DAW_Parcial_3.Tests
{
    public class SeleniumTests
    {
        public List<string> RunTest()
        {
            List<string> searchResultsList = new List<string>();

           
            IWebDriver driver = new ChromeDriver(@"C:\Users\Sony\Downloads\chromedriver-win64\chromedriver-win64\chromedriver.exe");

           
            driver.Navigate().GoToUrl("https://localhost:7109/");

            IWebElement email = driver.FindElement(By.Name("correo"));
            IWebElement password = driver.FindElement(By.Name("contrasena"));
            //testeo para user
            //email.SendKeys("HarryMC@gmail.com");
            //password.SendKeys("852456");

            //testeo para Empleado

            //email.SendKeys("esmeralda.garcia1@catolica.edu.sv");
            //password.SendKeys("esmeralda");


            //testeo para admin
            email.SendKeys("jorgefranciscocz@gmail.com");
            password.SendKeys("852456");


            IWebElement loginButton = driver.FindElement(By.CssSelector("button.btn-submit"));
            loginButton.Click();

           
            Thread.Sleep(5000);



            IList<IWebElement> searchResults = driver.FindElements(By.CssSelector("div.g"));
            foreach (var result in searchResults)
            {
                searchResultsList.Add(result.Text);
            }

            // Cerrar el navegador
            driver.Quit();

            // Devolver la lista de resultados
            return searchResultsList;
        }

    }
}
