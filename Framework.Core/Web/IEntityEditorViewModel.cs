namespace EvilDuck.Framework.Core.Web
{
    public interface IEntityEditorViewModel<in TEntity>
    {
        void FillEntity(TEntity entity);
    }
}
