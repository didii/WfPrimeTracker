using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data {
    public class InMemoryPrimeContext : PrimeContext {
        /// <inheritdoc />
        public InMemoryPrimeContext(DbContextOptions<PrimeContext> options) : base(options) {
            
        }

        ///// <inheritdoc />
        //public override int SaveChanges() {
        //    var type = ChangeTracker.Entries()
        //                            .Select(entry => entry.Entity.GetType())
        //                            .Distinct()
        //                            .FirstOrDefault(t => typeof(IPersistentItem).IsAssignableFrom(t));

        //    Database.OpenConnection();
        //    try {
        //        Database.ExecuteSqlCommand(GenerateIdentityInsertSql(type, true));
        //        var result = base.SaveChanges();
        //        return result;
        //    }
        //    finally {
        //        Database.ExecuteSqlCommand(GenerateIdentityInsertSql(type, false));
        //        Database.CloseConnection();
        //    }
        //}

        private string GenerateIdentityInsertSql(Type type, bool on) {
            var mapping = Model.FindEntityType(type).Relational();
            var schema = mapping.Schema ?? "dbo";
            var tableName = mapping.TableName;
            var onOff = on ? "ON" : "OFF";
            return $"SET IDENTITY_INSERT {schema}.{tableName} {onOff}";
        }

        public void Clear() {
            foreach (var entityType in Model.GetEntityTypes()) {
                var mapping = Model.FindEntityType(entityType.ClrType).Relational();
                var schema = mapping.Schema ?? "dbo";
                var tableName = mapping.TableName;
                var sql = $"DELETE FROM {schema}.{tableName}";
                Database.ExecuteSqlCommand(sql);
            }
        }
    }
}