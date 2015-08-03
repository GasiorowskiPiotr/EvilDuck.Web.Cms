using System;

namespace EvilDuck.Framework.Core.Web
{
    public class QueryModel
    {
        public string FilterField { get; set; }
        public string FilterValue { get; set; }
        public string FilterOper { get; set; }

        public string OrderBy { get; set; }
        public int OrderDir { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }

        public QueryModel()
        {
            FilterOper = "Equal";
            Take = 10;
            Skip = 0;
        }

        public QueryModel(string filterField, string filterValue, string filterOper, string orderBy, int orderDir, int take, int skip)
        {
            FilterField = filterField;
            FilterValue = filterValue;
            FilterOper = filterOper;
            OrderBy = orderBy;
            OrderDir = orderDir;
            Take = take;
            Skip = skip;
        }

        public QueryModel Alter(string filterField = null, string filterValue = null, string filterOper = null, string orderBy = null, int orderDir = 0, int take = 0, int skip = -1)
        {
            var newQueryModel = new QueryModel(FilterField, FilterValue, FilterOper, OrderBy, OrderDir, Take, Skip);
            if (!String.IsNullOrEmpty(filterField))
            {
                newQueryModel.FilterField = filterField;
            }
            if (!String.IsNullOrEmpty(filterValue))
            {
                newQueryModel.FilterValue = filterValue;
            }
            if (!String.IsNullOrEmpty(filterOper))
            {
                newQueryModel.FilterOper = filterOper;
            }
            if (!String.IsNullOrEmpty(orderBy))
            {
                newQueryModel.OrderBy = orderBy;
            }
            if (orderDir != 0)
            {
                newQueryModel.OrderDir = orderDir;
            }
            if (take != 0)
            {
                newQueryModel.Take = take;
            }
            if (skip > -1)
            {
                newQueryModel.Skip = skip;
            }

            return newQueryModel;
        }

        public override string ToString()
        {
            return
                String.Format(
                    "QueryModel: FilterField={0}, FilterValue={1}, FilterOper={2}, OrderBy={3}, OrderDir={4}, Take={5}, Skip={6}",
                    FilterField, FilterValue, FilterOper, OrderBy, OrderDir, Take, Skip);
        }
    }
}