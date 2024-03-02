using System;
using System.Net;
using System.Web;
using Sitecore;
using Sitecore.Diagnostics;

namespace VsCodeEditor.Platform
{ 
    public class CodeProxy
    {
        public readonly string CodePublicRelativePath = "/sitecore/shell/code";
        public readonly string CodeTargetUtl = "http://localhost:8080";

        public bool GetCodeResponse(ref HttpContext context)
        {

            var path = context.Request.Url.PathAndQuery;
            if (!path.StartsWith(CodePublicRelativePath, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (Context.User.IsInRole(@"sitecore\Developer") || Context.IsAdministrator)
            {
                if (path.Equals(CodePublicRelativePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Response.Redirect($"{CodePublicRelativePath}/");
                    context.Response.End();
                }

                //var remoteUrl = $"{GetSolrServer()}{path.Replace(SolrSitecorePath, SolrOriginPath)}";
                var remoteUrl = $"{CodeTargetUtl}{path.Replace(CodePublicRelativePath, string.Empty)}";
                var request = (HttpWebRequest)WebRequest.Create(remoteUrl);
                HttpWebResponse response;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException webException)
                {
                    Log.Error($"Unable to get response from VS Code server", webException, this);
                    //remote url not found, send 404 to client 
                    context.Response.StatusCode = 500;
                    context.Response.StatusDescription = "Unable to proxy request to VS Code Server";
                    context.Response.Write("Unable to proxy request to VS Code Server");
                    context.Response.End();

                    return true;
                }

                var receiveStream = response.GetResponseStream();


                var buff = new byte[1024];
                var bytes = 0;
                while ((bytes = receiveStream.Read(buff, 0, 1024)) > 0)
                {
                    //Write the stream directly to the client 
                    context.Response.OutputStream.Write(buff, 0, bytes);
                }

                //close streams
                response.Close();
                context.Response.ContentType = response.ContentType;
                context.Response.End();

                return true;
            }


            else
            {
                context.Response.Redirect("/sitecore/login");
                context.Response.End();
            }

            return false;
        }
    }
}