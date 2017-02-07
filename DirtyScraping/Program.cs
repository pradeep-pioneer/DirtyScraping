using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.IO;

namespace DirtyScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestPhantom();
            TestChrome();
            //TestRemote();
        }

        static void TestChrome()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var driver = new ChromeDriver(chromeDriverService);
            List<string> urls = new List<string>();
            StreamReader reader = File.OpenText("urls.txt");
            while (!reader.EndOfStream)
            {
                urls.Add(reader.ReadLine());
            }
            var fs = new StreamWriter(File.OpenWrite("results.txt"));
            foreach (var item in urls)
            {
                driver.Url = item;
                driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                driver.Navigate();

                var source = driver.PageSource;
                var label1 = driver.FindElementByXPath("//label[text()='Как новый']");
                var label2 = driver.FindElementByXPath("//label[text()='Устройство полностью исправно']");
                var label3 = driver.FindElementByXPath("//label[text()='Полная комплектация']");
                var label4 = driver.FindElementByXPath("//label[text()='Блокировки отсутствуют']");
                label1.Click();
                label2.Click();
                label3.Click();
                label4.Click();
                driver.GetScreenshot().SaveAsFile("test.png", System.Drawing.Imaging.ImageFormat.Png);
                WaitForAjax(driver);
                var price = driver.FindElementByClassName("price");
                fs.WriteLine(string.Format("{0}|{1}", item, price.Text));
                fs.Flush();
                Console.WriteLine(price.Text);
            }
            
            fs.Close();
            driver.Close();
            driver.Quit();
        }

        static void TestPhantom()
        {
            var driverService = PhantomJSDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;

            var driver = new PhantomJSDriver(driverService);

            driver.Url = "https://www.kak-noviy.ru/sell/phones/apple/iphone-5/64gb-white";
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            driver.Navigate();

            var source = driver.PageSource;
            var label1 = driver.FindElementByXPath("//label[text()='Как новый']");
            var label2 = driver.FindElementByXPath("//label[text()='Устройство полностью исправно']");
            var label3 = driver.FindElementByXPath("//label[text()='Полная комплектация']");
            var label4 = driver.FindElementByXPath("//label[text()='Блокировки отсутствуют']");
            label1.Click();
            label2.Click();
            label3.Click();
            label4.Click();
            WaitForAjax(driver);
            driver.GetScreenshot().SaveAsFile("test.png", System.Drawing.Imaging.ImageFormat.Png);
            var price = driver.FindElementByClassName("price");
            Console.WriteLine(price.Text);
            driver.Close();
            driver.Quit();
            
        }

        static void TestRemote()
        {
            RemoteWebDriver driver;
            DesiredCapabilities capability = DesiredCapabilities.Chrome();
            capability.SetCapability("platform", "VISTA");
            capability.SetCapability("version", "46");
            capability.SetCapability("gridlasticUser", "ojRjIL3RnfkrT9KE0lCr4KF6rfzSdNVr");
            capability.SetCapability("gridlasticKey", "WDBueucrzmJjmkNdvfvpQ0m75MQ9pCrc");
            capability.SetCapability("video", "True");
            driver = new RemoteWebDriver(
              new Uri("http://3u9xxoul.gridlastic.com:80/wd/hub/"), capability, TimeSpan.FromSeconds(600));// NOTE: connection timeout of 600 seconds or more required for time to launch grid nodes if non are available.
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.kak-noviy.ru/sell/phones/apple/iphone-5/64gb-white";
            
            driver.Navigate();

            var source = driver.PageSource;
            var label1 = driver.FindElementByXPath("//label[text()='Как новый']");
            var label2 = driver.FindElementByXPath("//label[text()='Устройство полностью исправно']");
            var label3 = driver.FindElementByXPath("//label[text()='Полная комплектация']");
            var label4 = driver.FindElementByXPath("//label[text()='Блокировки отсутствуют']");
            label1.Click();
            label2.Click();
            label3.Click();
            label4.Click();
            WaitForAjax(driver);
            driver.GetScreenshot().SaveAsFile("test.png", System.Drawing.Imaging.ImageFormat.Png);
            var price = driver.FindElementByClassName("price");
            Console.WriteLine(price.Text);
            driver.Close();
            driver.Quit();
        }

        public static void WaitForAjax(IWebDriver driver, int timeoutSecs = 10, bool throwException = false)
        {
            for (var i = 0; i < timeoutSecs; i++)
            {
                var ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                if (ajaxIsComplete) return;
                Thread.Sleep(1000);
            }
            if (throwException)
            {

                throw new Exception("WebDriver timed out waiting for AJAX call to complete");
            }
        }
    }
    
}
