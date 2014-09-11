using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Hermes.Experiments.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new DecoratorViewEngine(new RazorViewEngine()));

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public class DecoratorViewEngine : IViewEngine
    {
        private readonly IViewEngine _viewEngine;

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return _viewEngine.FindPartialView(controllerContext, partialViewName, useCache);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var result = _viewEngine.FindView(controllerContext, viewName, masterName, useCache);

            if (result.View == null ||
                result.ViewEngine == null)
            {
                return result;   
            }
            return new ViewEngineResult(new DecoratorView(result.View), result.ViewEngine); 
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            var v = new DecoratorView(view);

            _viewEngine.ReleaseView(controllerContext, v);
        }

        public DecoratorViewEngine(IViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }
    }

    public class DecoratorView : IView
    {
        private readonly IView _view;

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            _view.Render(viewContext, writer);

            var script = string.Format(@"<script src='/Scripts/Controllers/{0}.js'></script>",
                viewContext.TempData["controller"]);

            writer.WriteLine(script);
        }

        public DecoratorView(IView view)
        {
            _view = view;
        }
    }


}
