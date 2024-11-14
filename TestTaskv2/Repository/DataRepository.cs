using Dapper;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;
using TestTaskv2.Entity;

namespace TestTaskv2.Repository
{
    public class DataRepository : IDataRepository
    {
        private string _connectionString = null;

        public DataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> CreateAsync(PurchaseData data)
        {
            using (IDbConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < data.Customers.Count; i++)
                        {
                            data.Customers[i].Id = await GetOrCreateCustomerAsync(connection, transaction, data.Customers[i]);
                        }
                        data.Id = await CreatePurchaseData(connection, transaction, data);

                        foreach(var customer in data.Customers)
                        {
                            var customerPurchase = new CustomerPurchase
                            {
                                CustomerId = customer.Id,
                                PurchaseId = data.Id
                            };

                            customerPurchase.Id = await CreateCustomerPurchase(connection, transaction, customerPurchase);
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }

            return data.Id;
        }

        private async Task<int> CreateCustomerPurchase(IDbConnection connection, IDbTransaction transaction, CustomerPurchase customerPurchase)
        {
            var query = "insert into customerPurchases (PurchaseId, CustomerId) values(@PurchaseId, @CustomerId) returning id";
            return await connection.QueryFirstAsync<int>(query, customerPurchase, transaction);
        }

        private async Task<int> CreatePurchaseData(IDbConnection connection, IDbTransaction transaction, PurchaseData data)
        {
            var query = "insert into PurchaseData (PurchaseNumber, PurchaseObjectInfo, DocPublishDate)" +
                            "values (@PurchaseNumber, @PurchaseObjectInfo, @DocPublishDate) returning id";
            return await connection.QueryFirstAsync<int>(query, data, transaction);
        }

        private async Task<int> GetOrCreateCustomerAsync(IDbConnection connection, IDbTransaction transaction, Customer customer)
        {
            string query = "select * from Customer where FullName = @FullName or Inn = @Inn";
            var existingCustomerId = await connection.QueryFirstOrDefaultAsync<int?>(query, customer, transaction);

            if (existingCustomerId.HasValue)
            {
                return existingCustomerId.Value;
            }

            query = "insert into Customer (FullName, Inn) values(@FullName, @Inn) returning Id";
            var createdCustomerId = await connection.QueryFirstAsync<int>(query, customer, transaction);

            return createdCustomerId;
        }
    }
}
