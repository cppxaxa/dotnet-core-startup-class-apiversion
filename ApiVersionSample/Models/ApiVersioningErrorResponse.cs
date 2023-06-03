namespace ApiVersionSample.Models
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using System;
    using System.Linq;
    using System.Text.Encodings.Web;

    internal class ApiVersioningErrorResponse : DefaultErrorResponseProvider
    {
        private readonly HtmlEncoder htmlEncoder = HtmlEncoder.Default;

        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var sanitizedContext = new ErrorResponseContext(
                context.Request,
                context.StatusCode,
                context.ErrorCode,
                htmlEncoder.Encode(context.Message) + " " + GetApiVersionsMessage(),
                context.MessageDetail);
            return base.CreateResponse(sanitizedContext);
        }

        private string GetApiVersionsMessage()
        {
            string apiVersionCatalog = string.Join(
                ",",
                typeof(Constants.ApiVersions).GetFields().Select(e => $"'{e.GetValue(null)}'"));
            string message = $"The supported api versions are {apiVersionCatalog}.";
            return message;
        }
    }
}
