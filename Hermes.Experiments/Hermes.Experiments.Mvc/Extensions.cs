using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hermes.Experiments.Mvc
{
    public static class Extensions
    {
        public static IHtmlString Test(this HtmlHelper helper)
        {
            return new HtmlString("<div class='btn btn-info'></div>");
        }

        public static AngularBuilder Angular(this HtmlHelper helper)
        {
            return new AngularBuilder();
        }
    }

    public class AngularBuilder : IHtmlString
    {
        private string _controller;
        private string _content;

        public AngularBuilder Cntl(string controllerName)
        {
            _controller = controllerName;
            return this;
        }

        public AngularBuilder Content(string content)
        {
            _content = content;
            return this;
        }

        public string ToHtmlString()
        {
            return new HtmlString("<div class='btn btn-info' ng-controller='" + _controller + "' >" + _content + "</div>").ToHtmlString();
        }
    }

}