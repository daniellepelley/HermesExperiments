using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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

        public static AngularBuilder Ng(this HtmlHelper helper)
        {
            return new AngularBuilder(helper);
        }

        public static AngularScope AngularController(this HtmlHelper helper, string controller, string tagName = "div")
        {
            return new AngularScope(helper, controller, tagName);
        }
    }

    public class AngularBuilder : IHtmlString
    {
        private string _tagName = "div";

        private string _controller;
        private string _content;
        private string _onClick;
        private bool _withHover;
        private HtmlHelper _helper;

        public AngularBuilder(HtmlHelper helper)
        {
            _helper = helper;
        }

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

        public AngularBuilder OnClick(string onClick)
        {
            _onClick = onClick;
            return this;
        }

        public AngularBuilder WithHover()
        {
            _withHover = true;
            return this;
        }

        public AngularBuilder WithSubControl(Func<AngularBuilder, AngularBuilder> builder)
        {
            _content += builder(new AngularBuilder(_helper)).ToHtmlString();
            return this;
        }

        public string ToHtmlString()
        {
            var tagBuilder = new TagBuilder(_tagName);
            //tagBuilder.Attributes.Add("class", "btn btn-info");
            if (_withHover)
            {
                tagBuilder.Attributes.Add("hoverdirective", "");
            }

            if (!string.IsNullOrEmpty(_controller))
            {
                _helper.ViewContext.TempData.Add("controller", _controller);

                tagBuilder.Attributes.Add("ng-controller", _controller);
            }

            if (!string.IsNullOrEmpty(_onClick))
            {
                tagBuilder.Attributes.Add("ng-click", _onClick);
            }

            //tagBuilder.SetInnerText(_content);

            tagBuilder.InnerHtml = _content;

            return tagBuilder.ToString(TagRenderMode.Normal);
        }
    }

    public class AngularScope
        : IDisposable
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly string _tagName;

        public AngularScope(HtmlHelper htmlHelper, string controllerName, string tagName = "div")
        {
            _tagName = tagName;
            _htmlHelper = htmlHelper;
            var tagBuilder = new TagBuilder(_tagName);
            tagBuilder.Attributes.Add("ng-controller", controllerName);
            htmlHelper.ViewContext.Writer.WriteLine(tagBuilder.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            var tagBuilder = new TagBuilder(_tagName);
            _htmlHelper.ViewContext.Writer.WriteLine(tagBuilder.ToString(TagRenderMode.EndTag));
        }
    }
}