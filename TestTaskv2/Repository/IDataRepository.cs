using System.Threading.Tasks;
using TestTaskv2.Entity;

namespace TestTaskv2.Repository
{
    public interface IDataRepository
    {
        Task<int> CreateAsync(PurchaseData data);
    }
}
