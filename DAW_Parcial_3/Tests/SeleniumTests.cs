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

            // Configurar el WebDriver (asegúrate de que chromedriver esté en tu PATH)
            IWebDriver driver = new ChromeDriver(@"C:\Users\Sony\Downloads\chromedriver-win64\chromedriver-win64\chromedriver.exe");

            // Navegar a Google
            driver.Navigate().GoToUrl("https://localhost:7109/");

            // Encontrar los campos de entrada de correo electrónico y contraseña
            IWebElement emailInput = driver.FindElement(By.Name("correo"));
            IWebElement passwordInput = driver.FindElement(By.Name("contrasena"));

            // Ingresar las credenciales
            emailInput.SendKeys("esmeralda.garcia1@catolica.edu.sv");
            passwordInput.SendKeys("esmeralda");

            // Encontrar y hacer clic en el botón de inicio de sesión
            IWebElement loginButton = driver.FindElement(By.CssSelector("button.btn-submit"));
            loginButton.Click();

            // Esperar unos segundos para ver los resultados
            Thread.Sleep(5000);

            // Obtener los resultados de búsqueda
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
