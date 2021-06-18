using System;
using Gabinet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Gabinet.TagHelpers
{
    [HtmlTargetElement("paging")]
    public class PagingTagHelpers : TagHelper
    {
        public PagingTagHelpers(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }
        private LinkGenerator _linkGenerator;
        public PagingModel PagingModel { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder nav = new("nav");
            nav.MergeAttribute("aria-label", "Page navigation example");
            TagBuilder ul = new("ul");
            ul.AddCssClass("pagination justify-content-end");

            TagBuilder liPrev = new("li");
            liPrev.AddCssClass(PagingModel.CurrentPage == 1 ? "page-item disabled" : "page-item");
            TagBuilder aPrev = new("a");
            aPrev.AddCssClass("page-link");
            aPrev.MergeAttribute("href", _linkGenerator.GetPathByAction(ActionName, ControllerName, new { page = PagingModel.CurrentPage-1, itemOnPage = PagingModel.ItemOnPage }));
            aPrev.InnerHtml.AppendHtml("&laquo;");
            liPrev.InnerHtml.AppendHtml(aPrev);
            ul.InnerHtml.AppendHtml(liPrev);

            for (int i = 1; i <= PagingModel.Page; i++)
            {
                TagBuilder li = new("li");
                li.AddCssClass(i == PagingModel.CurrentPage ? "page-item disabled" : "page-item");
                   
                TagBuilder atag = new("a");
                atag.AddCssClass("page-link");
                atag.MergeAttribute("href", _linkGenerator.GetPathByAction(ActionName, ControllerName, new { page = i, itemOnPage = PagingModel.ItemOnPage }));
                atag.InnerHtml.Append(i.ToString());
                li.InnerHtml.AppendHtml(atag);
                ul.InnerHtml.AppendHtml(li);

            }

            TagBuilder liNext = new("li");
            liNext.AddCssClass(PagingModel.CurrentPage == PagingModel.Page ? "page-item disabled" : "page-item");
            TagBuilder aNext = new("a");
            aNext.AddCssClass("page-link");
            aNext.MergeAttribute("href", _linkGenerator.GetPathByAction(ActionName, ControllerName, new { page = PagingModel.CurrentPage + 1, itemOnPage = PagingModel.ItemOnPage }));
            aNext.InnerHtml.AppendHtml("&raquo;");
            liNext.InnerHtml.AppendHtml(aNext);
            ul.InnerHtml.AppendHtml(liNext);

            nav.InnerHtml.AppendHtml(ul);
            output.Content.SetHtmlContent(nav);
        }
    }
}
