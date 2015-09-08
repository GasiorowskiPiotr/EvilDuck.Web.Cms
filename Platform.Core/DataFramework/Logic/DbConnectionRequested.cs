using System;
using System.Data.Common;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class DbConnectionRequested : EventArgs
    {
        public DbConnection Connection { get; set; }
    }
}