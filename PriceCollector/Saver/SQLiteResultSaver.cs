using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using PriceCollector.Core.Collector;
using PriceCollector.Core.Data.Result;
using PriceCollector.Core.Extensions;

namespace PriceCollector.Saver {
    internal class SQLiteResultSaver: IResultSaver {
        private const string TABLE_NAME = "PriceResult";
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private Lazy<string> _connectionString = new Lazy<string>(() => {
            var connectionString = ConfigurationManager.ConnectionStrings["priceCollector"].ConnectionString;
            using (var conn = new SQLiteConnection(connectionString)) {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT name FROM sqlite_master WHERE name='{TABLE_NAME}'";
                var name = cmd.ExecuteScalar();
                if (name != null && name.ToString() == TABLE_NAME) {
                    return connectionString;
                }

                cmd.CommandText =
                    $"CREATE TABLE {TABLE_NAME} (ProductName VARCHAR(1000), SellerName VARCHAR(1000), Price FLOAT, Url VARCHAR(1000), Status BYTE, CollectDate DATETIME)";
                cmd.ExecuteNonQuery();
            }

            return connectionString;
        });



        public async Task Save(ICollection<ResultItem> items) {
            if (items == null || !items.Any()) {
                return;
            }

            using (var conn = new SQLiteConnection(_connectionString.Value)) {
                conn.Open();
                
                foreach (var resultItem in items) {
                    var insertSql =
                        new SQLiteCommand(
                            $"INSERT INTO {TABLE_NAME} (ProductName, SellerName, Price, Url, Status, CollectDate) " +
                            $"VALUES (@ProductName, @SellerName, @Price, @Url, @Status, @CollectDate)",
                            conn);
                    //insertSql.Parameters["Status"].DbType = DbType.Byte;
                    insertSql.Parameters.AddWithValue("ProductName", resultItem.ProductName);
                    insertSql.Parameters.AddWithValue("SellerName", resultItem.SellerName);
                    insertSql.Parameters.AddWithValue("Price", resultItem.Result.Price);
                    insertSql.Parameters.AddWithValue("Url", resultItem.Result.Url);
                    insertSql.Parameters.AddWithValue("Status", (byte)resultItem.Result.Status);
                    insertSql.Parameters.AddWithValue("CollectDate", DateTime.Now);
                    Debug.WriteLine(insertSql.CommandText);

                    try {
                        if (await insertSql.ExecuteNonQueryAsync() != 1) {
                            _logger.Error($"Saving are failed {resultItem.ToJson()}");
                        }
                    } catch (Exception ex) {
                        _logger.Error(ex, $"Exception from saving {resultItem.ToJson()}");
                    }
                }
            }
        }
    }
}
