using TestTaskv2.Entity;

namespace TestTaskv2.Services
{
    public abstract class DataSource
    {
        public abstract PurchaseData Accept(IDataSourceVisitor visitor);
    }

    public class XmlSource : DataSource
    {
        public string FilePath { get; set; }

        public override PurchaseData Accept(IDataSourceVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class HrefSource : DataSource
    {
        public string Link { get; set; }

        public override PurchaseData Accept(IDataSourceVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
