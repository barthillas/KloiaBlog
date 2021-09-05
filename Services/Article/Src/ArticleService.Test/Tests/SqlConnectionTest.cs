using System;
using Microsoft.Data.SqlClient;
using ThrowawayDb;
using Xunit;
using Xunit.Abstractions;

namespace ArticleService.UnitTest.Controllers.Tests
{
    public class SqlConnectionTest : TestBase.TestBase
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SqlConnectionTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private static class Settings
        {
            public const string Username = "sa";
            public const string Password = "kloia12345!@#$%";
            public const string Host = "localhost,1433";
        }

        [Fact]
        public void Can_Select_1_From_Database()
        {
            using var database = ThrowawayDatabase.Create(
                Settings.Username,
                Settings.Password,
                Settings.Host
            );

            _testOutputHelper.WriteLine($"Created database {database.Name}");

            using var connection = new SqlConnection(database.ConnectionString);
            connection.Open();
            using var cmd = new SqlCommand("SELECT 1", connection);
            var result = Convert.ToInt32(cmd.ExecuteScalar());

            _testOutputHelper.WriteLine(result.ToString());

            Assert.Equal(1, result);
        }
    }
}