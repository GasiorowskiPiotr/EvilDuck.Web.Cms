using System;
using System.Data;
using System.Text;
using EvilDuck.Framework.Core;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class TableComponent
    {
        private readonly Table _table;
        private readonly Logger _logger;

        public TableComponent(Table table)
        {
            _table = table;
            _logger = LogManager.GetLogger(typeof(TableComponent).FullName);
        }

        public Result CreateDbTable()
        {
            try
            {
                var dbConnectionRequest = new DbConnectionRequested();
                OnDbConnectionRequested(dbConnectionRequest);

                if (dbConnectionRequest.Connection != null)
                {
                    if (dbConnectionRequest.Connection.State != ConnectionState.Open)
                    {
                        dbConnectionRequest.Connection.Open();
                    }

                    var cmd = dbConnectionRequest.Connection.CreateCommand();

                    cmd.CommandText = GetQuery();
                    

                    cmd.ExecuteNonQuery();

                    var result = Result.Success(String.Format("Tworzenie tabeli: {0} zakończone sukcesem.", _table.Name));
                    var dbTableCreatedEventArgs = new DbTableCreatedEventArgs(_table.Name, result);
                    OnTableCreated(dbTableCreatedEventArgs);

                    if (dbTableCreatedEventArgs.ExternalOperationResults.IsSuccess)
                    {
                        return dbTableCreatedEventArgs.ExternalOperationResults.MergeFailures();
                    }

                    return result;
                }

                return Result.Failure("Nie udało się pobrać połączenia do bazy danych");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while creating table '{0}'", _table.Name);
                return Result.Failure(String.Format("Błąd w czasie tworzenia tabeli: {0}.", _table.Name), ex);
            }
        }

        private string GetQuery()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("CREATE TABLE {0} ({1}", _table.Name, Environment.NewLine);
            foreach (var column in _table.Columns)
            {
                sb.AppendFormat("{0} ", column.Name);
                switch (column.Type)
                {
                    case DbDataType.DateTime:
                        sb.Append("datetime ");
                        break;
                    case DbDataType.Float:
                        sb.Append("float ");
                        break;
                    case DbDataType.Int:
                        sb.Append("int ");
                        break;
                    case DbDataType.String:
                        sb.AppendFormat("{0}{1} ", "varchar",
                            column.Length > 0 ? String.Format("({0})", column.Length) : String.Empty);
                        break;
                    case DbDataType.Text:
                        sb.Append("text ");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("column.Type");
                }

                if (column.AutoincrementKey && column.IsKey)
                {
                    sb.Append("PRIMARY KEY IDENTITY(1,1) ");
                }
                else if (column.IsKey)
                {
                    sb.Append("PRIMARY KEY ");
                }

                if (String.IsNullOrEmpty(column.DefaultValue))
                {
                    sb.AppendFormat("DEFAULT {0} ", column.DefaultValue);
                }

                if (column.IsRelation)
                {
                    sb.AppendFormat("references {0}({1}) ", column.RelationTable, "ID");
                }

                if (!column.IsRelation && !column.IsKey && column.IsRequired)
                {
                    sb.Append("NOT NULL");
                }

                sb.AppendLine(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public event EventHandler<DbTableCreatedEventArgs> TableCreated;

        private void OnTableCreated(DbTableCreatedEventArgs dbTableCreated)
        {
            var tableCreated = TableCreated;
            if (tableCreated != null)
            {
                tableCreated(this, dbTableCreated);
            }
        }

        public event EventHandler<DbConnectionRequested> DbConnectionRequested;

        private void OnDbConnectionRequested(DbConnectionRequested dbConnectionRequested)
        {
            var dbConnReq = DbConnectionRequested;
            if (dbConnReq != null)
            {
                dbConnReq(this, dbConnectionRequested);
            }
        }
    }
}
