using System;
using EvilDuck.Framework.Core;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class DbTableCreatedEventArgs : EventArgs
    {
        public string TableName { get; private set; }
        public Result CreationResult { get; private set; }

        public ResultCollection ExternalOperationResults { get; private set; }

        public DbTableCreatedEventArgs(string tableName, Result creationResult)
        {
            TableName = tableName;
            CreationResult = creationResult;
            ExternalOperationResults = new ResultCollection();
        }
    }
}