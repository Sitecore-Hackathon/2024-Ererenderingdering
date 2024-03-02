# escape=`

ARG PARENT_IMAGE
ARG SOLUTION_IMAGE
ARG TOOLS_IMAGE
ARG MANAGEMENT_SERVICES_IMAGE
ARG HEADLESS_SERVICES_IMAGE

FROM ${SOLUTION_IMAGE} as solution
FROM ${TOOLS_IMAGE} as tools
FROM ${MANAGEMENT_SERVICES_IMAGE} AS management_services
FROM ${HEADLESS_SERVICES_IMAGE} AS headless_services
FROM ${PARENT_IMAGE}

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR C:\inetpub\wwwroot

# Install Chocolatey
RUN Set-ExecutionPolicy Bypass -Scope Process -Force; `
    [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; `
    iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'));

# Install Python
RUN choco install python --version 3.11.0 --yes

# Test Python installation
RUN python --version

# Install Node.js
RUN choco install nodejs --version 18.18.2 --yes

# Test Node.js installation
RUN node --version

# Install Git
RUN choco install git --version 2.39.2 --yes

# Append additional git directories to PATH
#ENV ADD_PATH=";C:\Program Files\Git\usr\bin;C:\Program Files\Git\bin"
RUN setx /M PATH \"$Env:PATH;C:\\Program Files\\Git\\usr\\bin;C:\\Program Files\\Git\\bin\"

# Install Visual Studio Build Tools
RUN choco install visualstudio2019buildtools visualstudio2019-workload-vctools windows-sdk-10-version-1809-all --yes

RUN npm install --global code-server --unsafe-perm

# Copy developer tools and entrypoint
COPY --from=tools C:\tools C:\tools

# Copy the Sitecore Management Services Module
COPY --from=management_services C:\module\cm\content C:\inetpub\wwwroot

# Copy and init the JSS / Headless Services Module
COPY --from=headless_services C:\module\cm\content C:\inetpub\wwwroot
COPY --from=headless_services C:\module\tools C:\module\tools
RUN C:\module\tools\Initialize-Content.ps1 -TargetPath C:\inetpub\wwwroot; `
    Remove-Item -Path C:\module -Recurse -Force;

# Copy solution website files
COPY --from=solution /artifacts/sitecore/ ./
COPY readme.md *.config*  c:\inetpub\wwwroot\App_Config\Include\

COPY .\tools C:\tools
# Copy role transforms
COPY .\transforms c:\inetpub\wwwroot

#Get content of web.config.xdt and output to console
RUN Write-Host "Web.config.xdt" ; Get-Content C:\inetpub\wwwroot\Web.config.xdt

# Perform role transforms
RUN C:\tools\scripts\Invoke-XdtTransform.ps1 -Path 'C:\inetpub\wwwroot\Web.config' -XdtPath 'C:\inetpub\wwwroot\Web.config.xdt'

#Get content of web.config and output to console
RUN Write-Host "Web.config before transform" ; Get-Content C:\inetpub\wwwroot\Web.config