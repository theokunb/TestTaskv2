using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using TestTaskv2.Entity;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;

namespace TestTaskv2.Services
{
    public class HtmlParser
    {
        public PurchaseData Parse(HtmlDocument htmlDoc)
        {
            var purchaseData = new PurchaseData();

            purchaseData.PurchaseObjectInfo = GetValueFromElement<string>(htmlDoc.DocumentNode, "//span[@class='main-lot-title']");
            purchaseData.DocPublishDate = GetValueFromElement(htmlDoc.DocumentNode, "//div[@class='date']", (text) => DateTime.Parse(text));
            purchaseData.PurchaseNumber = GetValueFromElement(htmlDoc.DocumentNode, "//div[@class='lot-card__link']", (text) => new string(text.Skip(2).ToArray()));

            purchaseData.Price = GetValueFromElement(htmlDoc.DocumentNode, "//span[@class='lot-card__main-cost-value']", text =>
                {
                    var priceDecoded = HttpUtility.HtmlDecode(text);
                    return ConvertCurrency(priceDecoded);
                });

            var customersElement = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='customers__content']");
            purchaseData.Customers = new List<Customer>();
            foreach (var customerNode in customersElement.ChildNodes)
            {
                if(customerNode.Name == "#comment")
                {
                    continue;
                }

                var customer = new Customer();
                purchaseData.Customers.Add(customer);

                foreach (var node in customerNode.ChildNodes)
                {
                    customer.Parse(node);
                }
            }

            return purchaseData;
        }

        public float ConvertCurrency(string value)
        {
            char currentDecimalSeparator = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            value = value.Replace('.', currentDecimalSeparator);
            value = value.Replace(',', currentDecimalSeparator);

            StringBuilder builder = new StringBuilder(value.Length);
            foreach (var ch in value)
            {
                if (char.IsDigit(ch) || ch == currentDecimalSeparator)
                    builder.Append(ch);
            }

            string s = builder.ToString();
            return float.Parse(s);
        }

            private T GetValueFromElement<T>(HtmlNode node, string xPath, Func<string, T> convertAction = null)
        {
            var element = node.SelectSingleNode(xPath);

            if (convertAction != null)
                return convertAction(element.InnerText);

            return (T)Convert.ChangeType(element.InnerText, typeof(T));
        }
    }

    public static class HtmlNodeExtension
    {
        public static bool HasAttributeValue(this HtmlNode node, string value)
        {
            foreach(var attribute in node.Attributes)
            {
                if(attribute.Value == value)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static partial class CustomerExtension
    {
        public static void Parse(this Customer customer, HtmlNode node)
        {
            if (node.HasAttributeValue("more-info__line customers__search"))
            {
                customer.Parse(node.FirstChild);
                return;
            }
            if(node.HasAttributeValue("card-value customers__search-column"))
            {
                foreach(var childNode in node.ChildNodes)
                {
                    customer.Parse(childNode);
                }
                return;
            }

            if (node.Name == "a")
            {
                customer.FullName = node.InnerText;
            }

            if (node.Name == "div")
            {
                var regExp = new Regex("ИНН: [0-9]+");
                var match = regExp.Match(node.InnerText);

                if (match.Success)
                {
                    regExp = new Regex("[0-9]+");
                    match = regExp.Match(match.Value);

                    customer.Inn = match.Value;
                }
            }
        } 
    }
}
