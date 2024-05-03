using System;
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

        public string StringTest { get; set; } = null!;

        public int IntTest { get; set; }
    }
}
