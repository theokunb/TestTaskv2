using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestTaskv2.Entity;

namespace TestTaskv2.Services
{
    public class XmlParser
    {
        public PurchaseData Parse(XmlDocument xmlDocument)
        {
            var root = xmlDocument.DocumentElement;
            var fcsNotificationEF = root.GetElementsByTagName("ns2:fcsNotificationEF")[0];
            var data = new PurchaseData();

            data.Parse(fcsNotificationEF);

            return data;
        }
    }

    public static partial class DataParseExtension
    {
        public static void Parse(this PurchaseData data, XmlNode xmlNode)
        {
            foreach (XmlNode child in xmlNode.ChildNodes)
            {
                switch (child.Name)
                {
                    case "purchaseNumber":
                        data.PurchaseNumber = child.InnerText;
                        break;
                    case "docPublishDate":
                        data.DocPublishDate = DateTime.Parse(child.InnerText);
                        break;
                    case "purchaseObjectInfo":
                        data.PurchaseObjectInfo = child.InnerText;
                        break;
                    case "customerRequirements":
                        data.Customers = data.ParseCustomers(child).ToList();
                        break;
                    case "lot":
                        data.Parse(child);
                        break;
                    default:
                        break;
                }
            }
        }

        public static IEnumerable<Customer> ParseCustomers(this PurchaseData data, XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "customerRequirement")
                {
                    var customer = new Customer();
                    customer.Parse(child);
                    yield return customer;
                }
            }
        }

        public static Customer Parse(this Customer customer, XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "customer")
                {
                    customer.Parse(child);
                }
                if (child.Name == "purchaseCode")
                {
                    customer.Inn = new string(child.InnerText.Skip(3).Take(10).ToArray());
                }
                if (child.Name == "fullName")
                {
                    customer.FullName = child.InnerText;
                }
            }

            return customer;
        }
    }

}
