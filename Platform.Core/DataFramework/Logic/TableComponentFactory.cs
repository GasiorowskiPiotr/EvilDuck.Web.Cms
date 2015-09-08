using System;
using EvilDuck.Framework.Core;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class TableComponentFactory
    {
        private readonly TablesRepository _repository;
        private readonly IUnitOfWork<PlatformDomainContext> _unitOfWork;
        private readonly PlatformDomainContext _domainContext;

        public TableComponentFactory(TablesRepository repository, IUnitOfWork<PlatformDomainContext> unitOfWork, PlatformDomainContext domainContext)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _domainContext = domainContext;
        }

        public TableComponent CreateTableComponent(int tableId)
        {

            var table = _repository.GetByKey(tableId);
            var component = new TableComponent(table);
            component.DbConnectionRequested += component_DbConnectionRequested;
            component.TableCreated += component_TableCreated;

            return component;    

            
        }

        void component_TableCreated(object sender, DbTableCreatedEventArgs e)
        {
            try
            {
                var table = _repository.GetByName(e.TableName);
                table.IsExported = true;

                _unitOfWork.SaveChanges();

                e.ExternalOperationResults.Add(Result.Success("Tabela oznaczona jako wyeksportowana."));
            }
            catch (Exception ex)
            {
                e.ExternalOperationResults.Add(Result.Failure("B³¹d w czasie zapisu tabeli", ex));
            }

        }

        void component_DbConnectionRequested(object sender, DbConnectionRequested e)
        {
            e.Connection = _domainContext.Database.Connection;
        }

        public TableComponent CreateTableComponent(string tableName)
        {
            var table = _repository.GetByName(tableName);
            var component = new TableComponent(table);
            component.DbConnectionRequested += component_DbConnectionRequested;
            component.TableCreated += component_TableCreated;
            return component;
        }
    }
}