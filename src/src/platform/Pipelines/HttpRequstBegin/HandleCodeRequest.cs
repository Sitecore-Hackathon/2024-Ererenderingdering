using Sitecore.Pipelines.HttpRequest;

namespace VsCodeEditor.Platform.Pipelines.HttpRequestBegin
{
    public class HandleCodeRequest : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            var proxy = new CodeProxy();
            var argsContext = args.HttpContext.ApplicationInstance.Context;
            if (proxy.GetCodeResponse(ref argsContext))
            {
                args.AbortPipeline();
            }
        }
    }
}