using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Xml;
using TestTaskv2.Entity;

namespace TestTaskv2.Services
{
    public interface IDataSourceVisitor
    {
        PurchaseData Visit(XmlSource fileSource);
        PurchaseData Visit(HrefSource hrefSource);
    }

    public class DataSourceVisitor : IDataSourceVisitor
    {
        private readonly XmlParser _xmlParser;
        private readonly HtmlParser _htmlParser;

        public DataSourceVisitor(XmlParser parser, HtmlParser htmlParser)
        {
            _xmlParser = parser;
            _htmlParser = htmlParser;
        }

        public PurchaseData Visit(XmlSource fileSource)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileSource.FilePath);

            return _xmlParser.Parse(xmlDoc);
        }

        public PurchaseData Visit(HrefSource hrefSource)
        {
            PurchaseData purchaseData = null;

            using (var driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(hrefSource.Link);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement webElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span.main-lot-title")));

                string pageSource = driver.PageSource;
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(pageSource);

                purchaseData = _htmlParser.Parse(htmlDoc);

                driver.Quit();
            }

            return purchaseData;
        }
    }
}
