using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using TestTaskv2.Entity;
using System.Linq;

namespace TestTaskv2.Services
{
    public class HtmlParser
    {
        public PurchaseData Parse(HtmlDocument htmlDoc)
        {
            var purchaseData = new PurchaseData();

            var purchaseObjectInfoElement = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='main-lot-title']");
            purchaseData.PurchaseObjectInfo = purchaseObjectInfoElement.InnerText;

            var publishDateElement = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='date']");
            purchaseData.DocPublishDate = DateTime.Parse(publishDateElement.InnerText);

            var purchaseNumberElement = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='lot-card__link']");
            purchaseData.PurchaseNumber = new string(purchaseNumberElement.FirstChild.InnerText.Skip(2).ToArray());

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
