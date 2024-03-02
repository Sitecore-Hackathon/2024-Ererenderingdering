<%@ Page language="c#" AutoEventWireup="false" Inherits="VsCodeEditor.Platform.Sitecore.Admin.VsCodeEditorFrame" %>
<!DOCTYPE html>
<html>
  <head>
    <title>Vs Code Editor</title>
    <link rel="shortcut icon" href="/sitecore/images/favicon.ico" />
    <meta content=C# name=CODE_LANGUAGE>
  </head>
  <body>
    <form id=Form1 method=post runat="server">
      <iframe id="vsCodeFrame" src="<%= FrameSrc %>"></iframe>
    </form>
  </body>
</html>
