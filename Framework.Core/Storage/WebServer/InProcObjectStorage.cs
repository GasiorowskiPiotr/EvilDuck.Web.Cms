using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EvilDuck.Framework.Core.Components;
using NLog;

namespace EvilDuck.Framework.Core.Storage.WebServer
{
    public class InProcObjectStorage : BaseComponent, IObjectStorage
    {
        private readonly HttpContextBase _httpContext;
        private readonly UrlHelper _urlHelper;


        public InProcObjectStorage(HttpContextBase httpContext, UrlHelper urlHelper, Logger logger)
            : base(logger)
        {
            _httpContext = httpContext;
            _urlHelper = urlHelper;
        }

        public string Save(Stream stream, string fileName, string container)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Saving file: {0} to container: {1}", fileName, container);
            }
            var storagePath = _httpContext.Server.MapPath("~/Storage");
            if (!Directory.Exists(storagePath))
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Could not find path: {0}. Creating directory.", storagePath);
                }
                Directory.CreateDirectory(storagePath);
            }

            var containerPath = Path.Combine(storagePath, container);
            if (!Directory.Exists(containerPath))
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Could not find path: {0}. Creating directory.", containerPath);
                }
                Directory.CreateDirectory(containerPath);
            }

            var filePath = Path.Combine(containerPath, fileName);
            if (File.Exists(filePath))
            {
                throw new ArgumentException("File already exists!");
            }

            using (var fileStream = File.Create(filePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            return _urlHelper.Content(String.Format("~/Storage/{0}/{1}", container, fileName));
        }
    }
}