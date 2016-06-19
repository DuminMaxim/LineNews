using System.Web.Mvc;

namespace WebUI.Infrastructure
{
    public static class CustomHelpers
    {
        public static MvcHtmlString ReplaceNewLineСharacters(this HtmlHelper helper, string text)
        {
            text = text.Replace("\n", " <br />");
            return new MvcHtmlString(text);
        }

        public static MvcHtmlString CutText(this HtmlHelper helper, string text)
        {
            if (text.Length > 250)
            {
                text = text.Substring(0, 250);
                text += "...";
            }

            return new MvcHtmlString(text);
        }
    }
}