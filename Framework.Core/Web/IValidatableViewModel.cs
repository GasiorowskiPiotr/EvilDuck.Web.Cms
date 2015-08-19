using System.Web.Mvc;

namespace EvilDuck.Framework.Core.Web
{
    public interface IValidatableViewModel
    {
        void Validate(ModelStateDictionary modelState);
    }
}