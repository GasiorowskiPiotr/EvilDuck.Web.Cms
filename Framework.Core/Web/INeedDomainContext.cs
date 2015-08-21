namespace EvilDuck.Framework.Core.Web
{
    public interface INeedDomainContext<in TDomainContext>
    {
        void UseContext(TDomainContext context);
    }
}