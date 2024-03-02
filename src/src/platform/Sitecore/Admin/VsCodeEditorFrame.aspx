<%@ Page language="c#" AutoEventWireup="false" Inherits="VsCodeEditor.Platform.Sitecore.Admin.VsCodeEditorFrame" %>
  <!DOCTYPE html>
  <html style="height: 100%; width: 100%; margin: 0;">

  <head>
    <title>Vs Code Editor</title>
    <link rel="shortcut icon" href="/sitecore/images/favicon.ico" />
    <meta content=C# name=CODE_LANGUAGE>
  </head>

  <body style="height: 100%; width: 100%; margin: 0;">
    <form id=Form1 method=post runat="server" style="height: 100%; width: 100%; margin: 0;">
      <iframe id="vsCodeFrame" src="<%= FrameSrc %>" style="height: 100%; width: 100%; margin: 0;"></iframe>
    </form>
  </body>

  </html>