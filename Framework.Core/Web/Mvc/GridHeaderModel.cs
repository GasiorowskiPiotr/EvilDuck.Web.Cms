namespace EvilDuck.Framework.Core.Web.Mvc
{
    public class GridHeaderModel
    {
        public GridHeaderModel()
        {
        }

        public GridHeaderModel(string columnName, string caption)
        {
            ColumnName = columnName;
            Caption = caption;
        }

        public string ColumnName { get; set; }
        public string Caption { get; set; }
    }
}
