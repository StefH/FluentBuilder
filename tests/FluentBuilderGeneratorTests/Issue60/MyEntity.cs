using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Data.Tables;

namespace FluentBuilderGeneratorTests.Issue60
{
    public class MyEntity : ITableEntity
    {
        #region ITableEntity properties
        public string PartitionKey { get; set; } = null!;

        public string RowKey { get; set; } = null!;

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
        #endregion

        public string Test { get; set; } = null!;
    }
}