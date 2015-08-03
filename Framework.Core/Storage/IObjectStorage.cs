using System.IO;

namespace EvilDuck.Framework.Core.Storage
{
    public interface IObjectStorage
    {
        string Save(Stream stream, string fileName, string container);
    }
}