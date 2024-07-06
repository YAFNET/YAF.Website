/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2024 Ingo Herbote
 * https://www.yetanotherforum.net/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * https://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using Microsoft.AspNetCore.Mvc.RazorPages;

using YAF.Core.Context;
using YAF.Types.Extensions;

namespace YAF.Website.Pages;

/// <summary>
/// Class MigrationToolModel.
/// Implements the <see cref="PageModel" />
/// Implements the <see cref="YAF.Types.Interfaces.IHaveServiceLocator" />
/// </summary>
/// <seealso cref="PageModel" />
/// <seealso cref="YAF.Types.Interfaces.IHaveServiceLocator" />
public class MigrationToolModel : PageModel, IHaveServiceLocator
{
    /// <summary>
    /// Gets the service locator.
    /// </summary>
    /// <value>The service locator.</value>
    public IServiceLocator ServiceLocator => BoardContext.Current.ServiceLocator;

    /// <summary>
    /// Gets or sets the import.
    /// </summary>
    /// <value>The import.</value>
    [BindProperty]
    public string Import { get; set; }

    /// <summary>
    /// Gets or sets the merged output.
    /// </summary>
    /// <value>The merged output.</value>
    [BindProperty]
    public string MergedOutput { get; set; }

    /// <summary>
    /// Gets or sets the application settings.
    /// </summary>
    /// <value>The application settings.</value>
    [BindProperty]
    public string AppSettings { get; set; }

	/// <summary>
	/// Gets or sets the error message.
	/// </summary>
	/// <value>The error message.</value>
	[BindProperty]
    public string ErrorMessage { get; set; }

/// <summary>
/// Called when [post].
/// </summary>
public void OnPost()
    {
#if !DEBUG
        try
        {
#endif
        if (this.Import.IsNotSet())
        {
            return;
        }

        this.MigrateConfig();

#if !DEBUG
        }
        catch (Exception exception)
        {
            this.ErrorMessage = exception.Message;

            BoardContext.Current?.Get<ILogger<MigrationToolModel>>().Log(
                    null,
                    "web.config Migration",
                    $"{this.Import}+++{exception.Source}",
                    EventLogTypes.Information);
        }
#endif
    }

    /// <summary>
    /// Migrates the configuration.
    /// </summary>
    private void MigrateConfig()
    {
        var appSettings = new StringBuilder();

        var webConfig = XDocument.Parse(
            this.Import.Replace(
                """
                xmlns="http://schemas.microsoft.com/.NetConfiguration/v2.0"
                """,
                string.Empty));

        var reader = webConfig.CreateReader();

        // Select Membership Node
        var memberShipNode = webConfig.XPathSelectElement("/configuration/system.web/membership");

        if (memberShipNode != null)
        {
            var memberShipName = memberShipNode.Attribute("defaultProvider").Value;

            try
            {
                appSettings.AppendFormat(
                    "<add key=\"YAF.LegacyMembershipHashAlgorithmType\" value=\"{0}\" />",
                    memberShipNode.Attribute("hashAlgorithmType").Value);
            }
            catch (Exception)
            {
                // Ignore if not exist
            }

            var memberShipProviderNode = webConfig.XPathSelectElement(
                $"/configuration/system.web/membership/providers/add[@name='{memberShipName}']");

            try
            {
                appSettings.AppendFormat(
                    "<add key=\"YAF.LegacyMembershipPasswordFormat\" value=\"{0}\" />",
                    memberShipProviderNode.Attribute("passwordFormat").Value);
            }
            catch (Exception)
            {
                // Ignore if not exist
            }

            try
            {
                appSettings.AppendFormat(
                    "<add key=\"YAF.LegacyMembershipHashCase\" value=\"{0}\" />",
                    memberShipProviderNode.Attribute("hashCase").Value);
            }
            catch (Exception)
            {
                // Ignore if not exist
            }

            try
            {
                appSettings.AppendFormat(
                    "<add key=\"YAF.LegacyMembershipHashHex\" value=\"{0}\" />",
                    memberShipProviderNode.Attribute("hashHex").Value);
            }
            catch (Exception)
            {
                // Ignore if not exist
            }
        }

        // Remove Nodes
        var taskNodes = new List<string>
                            {
                                "/configuration/system.webServer/modules/remove[@name='YafTaskModule']",
                                "/configuration/system.webServer/modules/add[@name='YafTaskModule']",
                                "/configuration/system.web/httpModules/add[@name='YafTaskModule']",
                                "/configuration/system.webServer/modules/remove[@name='TaskModule']",
                                "/configuration/system.webServer/modules/add[@name='TaskModule']",
                                "/configuration/system.web/httpModules/add[@name='TaskModule']",
                                "/configuration/system.web/profile",
                                "/configuration/system.web/roleManager",
                                "/configuration/system.web/pages/namespaces/add[starts-with(@namespace,'YAF.Utils')]",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'YAF.Controls.Statistics')]",
                                "/configuration/system.web/authentication",
                                "/configuration/system.web/membership",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'DNA.UI')]",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'DNA.UI')]",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'YAF.Classes.UI')]",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'YAF.Classes.Utils')]",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'YAF.Classes.Core')]",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'YAF.Classes.Data')]",
                                "/configuration/system.web/pages/controls/add[starts-with(@namespace,'YAF.Classes')]",
                                "/configuration/system.web/pages/namespaces/add[@namespace='YAF']"
                            };

        taskNodes.ForEach(
            path =>
            {
                var node = webConfig.XPathSelectElement(path);

                node?.Remove();
            });

        // Rename Intelligencia.UrlRewriter to YAF.UrlRewriter
        var rewriterNode = webConfig.XPathSelectElement(
            "/configuration/configSections/section[starts-with(@type,'Intelligencia.UrlRewriter')]");

        if (rewriterNode != null)
        {
            rewriterNode.Attribute("type").Value = rewriterNode.Attribute("type").Value
                .Replace("Intelligencia.UrlRewriter", "YAF.UrlRewriter");
        }

        var rewriterNode2 = webConfig.XPathSelectElement(
            "/configuration/system.webServer/modules/add[starts-with(@type,'Intelligencia.UrlRewriter')]");

        if (rewriterNode2 != null)
        {
            rewriterNode2.Attribute("type").Value = rewriterNode2.Attribute("type").Value
                .Replace("Intelligencia.UrlRewriter", "YAF.UrlRewriter");
        }

        var rewriterNode3 = webConfig.XPathSelectElement(
            "/configuration/system.web/httpModules/add[starts-with(@type,'Intelligencia.UrlRewriter')]");

        if (rewriterNode3 != null)
        {
            rewriterNode3.Attribute("type").Value = rewriterNode3.Attribute("type").Value
                .Replace("Intelligencia.UrlRewriter", "YAF.UrlRewriter");
        }

        // Update Depended Assembly
        var assemblyBindingNode = webConfig.XPathSelectElement("/configuration/runtime");

        var runtimeString = XElement.Parse(
            """
            <runtime>
                <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
                  <probing privatePath="bin" />
                 <dependentAssembly>
            		<assemblyIdentity name="System.Diagnostics.DiagnosticSource" publicKeyToken="CC7B13FFCD2DDD51" culture="neutral"/>
            	    <bindingRedirect oldVersion="0.0.0.0-8.0.0.1" newVersion="8.0.0.1"/>
            	  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" />
                    <bindingRedirect oldVersion="0.0.0.0-13.0.0.0" newVersion="13.0.0.0" />
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="Microsoft.Bcl.AsyncInterfaces" publicKeyToken="cc7b13ffcd2ddd51"/>
                    <bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0"/>
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="Microsoft.Owin.Security" publicKeyToken="31bf3856ad364e35" />
                    <bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" />
                    <bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="Microsoft.Owin.Security.Cookies" publicKeyToken="31bf3856ad364e35" />
                    <bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="Microsoft.Extensions.Primitives" publicKeyToken="ADB9793829DDAE60" culture="neutral"/>
                    <bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0"/>
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="System.Runtime.CompilerServices.Unsafe" publicKeyToken="B03F5F7F11D50A3A" culture="neutral"/>
                    <bindingRedirect oldVersion="0.0.0.0-6.0.0.0" newVersion="6.0.0.0"/>
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="System.Threading.Tasks.Extensions" publicKeyToken="cc7b13ffcd2ddd51"/>
                    <bindingRedirect oldVersion="0.0.0.0-4.2.0.1" newVersion="4.2.0.1"/>
                  </dependentAssembly>
                  <dependentAssembly>
                    <assemblyIdentity name="System.Memory" culture="neutral" publicKeyToken="cc7b13ffcd2ddd51" />
                    <bindingRedirect oldVersion="0.0.0.0-4.0.1.2" newVersion="4.0.1.2" />
                  </dependentAssembly>
                </assemblyBinding>
              </runtime>
            """);

        if (assemblyBindingNode != null)
        {
            var nsmgr = new XmlNamespaceManager(reader.NameTable);
            nsmgr.AddNamespace("ab", "urn:schemas-microsoft-com:asm.v1");

            var assemblybindings = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding", nsmgr);

            // Update Newtonsoft.Json
            var newton = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='Newtonsoft.Json']", nsmgr);

            if (newton != null)
            {
                var node = newton.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-13.0.0.0" newVersion="13.0.0.0" />""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                                                        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" />
                                             <bindingRedirect oldVersion="0.0.0.0-13.0.0.0" newVersion="13.0.0.0" /></dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update Microsoft.Bcl.AsyncInterfaces
            var asyncInterfaces = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='Microsoft.Bcl.AsyncInterfaces']", nsmgr);

            if (asyncInterfaces != null)
            {
                var node = asyncInterfaces.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0"/>""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                               <assemblyIdentity name="Microsoft.Bcl.AsyncInterfaces" publicKeyToken="cc7b13ffcd2ddd51"/>
                                               <bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0"/>
                                             </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update Microsoft.Owin.Security
            var owinSecurity = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='Microsoft.Owin.Security']", nsmgr);

            if (owinSecurity != null)
            {
                var node = owinSecurity.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                                                <assemblyIdentity name="Microsoft.Owin.Security" publicKeyToken="31bf3856ad364e35" />
                                                                                                <bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />
                                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update Microsoft.Owin
            var owin = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='Microsoft.Owin']", nsmgr);

            if (owin != null)
            {
                var node = owin.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                               <assemblyIdentity name="Microsoft.Owin" publicKeyToken="31bf3856ad364e35" />
                                                                               <bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />
                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update Microsoft.Owin.Security.Cookies
            var owinCookies = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='Microsoft.Owin.Security.Cookies']", nsmgr);

            if (owinCookies != null)
            {
                var node = owinCookies.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                               <assemblyIdentity name="Microsoft.Owin.Security.Cookies" publicKeyToken="31bf3856ad364e35" />
                                                                               <bindingRedirect oldVersion="0.0.0.0-4.2.2.0" newVersion="4.2.2.0" />
                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update Microsoft.Extensions.Primitives
            var extensionPrimitives = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='Microsoft.Extensions.Primitives']", nsmgr);

            if (extensionPrimitives != null)
            {
                var node = extensionPrimitives.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0"/>""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                               <assemblyIdentity name="Microsoft.Extensions.Primitives" publicKeyToken="ADB9793829DDAE60" culture="neutral"/>
                                                                               <bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0"/>
                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update System.Runtime.CompilerServices.Unsafe
            var compilerUnsafe = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='System.Runtime.CompilerServices.Unsafe']", nsmgr);

            if (compilerUnsafe != null)
            {
                var node = compilerUnsafe.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-6.0.0.0" newVersion="6.0.0.0"/>""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                               <assemblyIdentity name="System.Runtime.CompilerServices.Unsafe" publicKeyToken="B03F5F7F11D50A3A" culture="neutral"/>
                                                                               <bindingRedirect oldVersion="0.0.0.0-6.0.0.0" newVersion="6.0.0.0"/>
                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update System.Threading.Tasks.Extensions
            var threadingTask = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='System.Threading.Tasks.Extensions']", nsmgr);

            if (threadingTask != null)
            {
                var node = threadingTask.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-4.2.0.1" newVersion="4.2.0.1"/>""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                               <assemblyIdentity name="System.Threading.Tasks.Extensions" publicKeyToken="cc7b13ffcd2ddd51"/>
                                                                               <bindingRedirect oldVersion="0.0.0.0-4.2.0.1" newVersion="4.2.0.1"/>
                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Update System.Memory
            var systemMemory = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='System.Memory']", nsmgr);

            if (systemMemory != null)
            {
                var node = systemMemory.NextNode;

                var replaceNode = XElement.Parse("""<bindingRedirect oldVersion="0.0.0.0-4.0.1.2" newVersion="4.0.1.2" />""");

                node.ReplaceWith(replaceNode);
            }
            else
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                               <assemblyIdentity name="System.Memory" culture="neutral" publicKeyToken="cc7b13ffcd2ddd51" />
                                                                                 <bindingRedirect oldVersion="0.0.0.0-4.0.1.2" newVersion="4.0.1.2" />
                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }

            // Add System.Diagnostics.DiagnosticSource
            var systemDiagnosticSource = webConfig.XPathSelectElement("/configuration/runtime/ab:assemblyBinding/ab:dependentAssembly/ab:assemblyIdentity[@name='System.Diagnostics.DiagnosticSource']", nsmgr);

            if (systemDiagnosticSource == null)
            {
                var addNode = XElement.Parse("""
                                             <dependentAssembly xmlns="urn:schemas-microsoft-com:asm.v1">
                                                                               <assemblyIdentity name="System.Diagnostics.DiagnosticSource" culture="neutral" publicKeyToken="CC7B13FFCD2DDD51" />
                                                                                 <bindingRedirect oldVersion="0.0.0.0-8.0.0.1" newVersion="8.0.0.1" />
                                                                                </dependentAssembly>
                                             """);

                assemblybindings.Add(addNode);
            }
        }
        else
        {
            var systemWebNode = webConfig.XPathSelectElement("/configuration/system.web");

            systemWebNode.AddAfterSelf(runtimeString);
        }

        // Change ResourceHandler
        var resourceHandlerNode = webConfig.XPathSelectElement(
            "/configuration/system.web/httpHandlers/add[starts-with(@type,'YAF.YafResourceHandler')]");

        if (resourceHandlerNode != null)
        {
            resourceHandlerNode.Attribute("type").Value = resourceHandlerNode.Attribute("type").Value.Replace(
                "YAF.YafResourceHandler, YAF",
                "YAF.Core.Handlers.ResourceHandler, YAF.Core");
        }

        var resourceHandlerNode2 = webConfig.XPathSelectElement(
            "/configuration/system.webServer/handlers/add[starts-with(@type,'YAF.YafResourceHandler')]");

        if (resourceHandlerNode2 != null)
        {
            resourceHandlerNode2.Attribute("type").Value = resourceHandlerNode2.Attribute("type").Value.Replace(
                "YAF.YafResourceHandler, YAF",
                "YAF.Core.Handlers.ResourceHandler, YAF.Core");
        }

        // Change all Name Spaces
        var nameSpaceNode1 = webConfig.XPathSelectElement(
            "/configuration/system.web/pages/namespaces/add[@namespace='YAF.Classes']");

        if (nameSpaceNode1 != null)
        {
            nameSpaceNode1.Attribute("namespace").Value = nameSpaceNode1.Attribute("namespace").Value.Replace(
                "YAF.Classes",
                "YAF.Configuration");
        }

        var nameSpaceNode2 = webConfig.XPathSelectElement(
            "/configuration/system.web/pages/namespaces/add[@namespace='YAF.Controls']");

        if (nameSpaceNode2 != null)
        {
            nameSpaceNode2.Attribute("namespace").Value = nameSpaceNode2.Attribute("namespace").Value.Replace(
                "YAF.Controls",
                "YAF.Web.Controls");
        }

        var nameSpaceNode3 = webConfig.XPathSelectElement(
            "/configuration/system.web/pages/namespaces/add[@namespace='YAF.Classes.UI']");

        if (nameSpaceNode3 != null)
        {
            nameSpaceNode3.Attribute("namespace").Value = "YAF.Types.Interfaces";
        }

        var nameSpaceNode4 = webConfig.XPathSelectElement(
            "/configuration/system.web/pages/namespaces/add[@namespace='YAF.Classes.Core']");

        if (nameSpaceNode4 != null)
        {
            nameSpaceNode4.Attribute("namespace").Value = "YAF.Core";
        }

        var nameSpaceNode5 = webConfig.XPathSelectElement(
            "/configuration/system.web/pages/namespaces/add[@namespace='YAF.Classes.Data']");

        if (nameSpaceNode5 != null)
        {
            nameSpaceNode5.Attribute("namespace").Value = "YAF.Web";
        }

        var nameSpaceNode6 = webConfig.XPathSelectElement(
            "/configuration/system.web/pages/namespaces/add[@namespace='YAF.Classes.Utils']");

        if (nameSpaceNode6 != null)
        {
            nameSpaceNode6.Attribute("namespace").Value = "YAF.Types";
        }

        // Change all Controls
        var controlNode1 = webConfig.XPathSelectElement(
            "/configuration/system.web/pages/controls/add[@namespace='YAF.Controls']");

        if (controlNode1 != null)
        {
            controlNode1.Attribute("namespace").Value = controlNode1.Attribute("namespace").Value.Replace(
                "YAF.Controls",
                "YAF.Web.Controls");

            controlNode1.Attribute("assembly").Value = "YAF.Web";
        }

        // remove AjaxPro
        var ajaxProNode = webConfig.XPathSelectElement(
            "/configuration/system.webServer/handlers/add[starts-with(@path,'ajaxpro')]");

        if (ajaxProNode != null)
        {
            var resourceHandler = XElement.Parse(
                """
                <add name="YafHandler" preCondition="integratedMode" verb="GET" path="Resource.ashx"
                                                                                    type="YAF.Core.Handlers.ResourceHandler, YAF.Core"/>
                """);

            ajaxProNode.ReplaceWith(resourceHandler);
        }

        var ajaxProNode2 = webConfig.XPathSelectElement(
            "/configuration/system.web/httpHandlers/add[starts-with(@path,'ajaxpro')]");

        if (ajaxProNode2 != null)
        {
            var resourceHandler = XElement.Parse(
                """<add verb="GET" path="Resource.ashx" type="YAF.Core.Handlers.ResourceHandler, YAF.Core"/>""");

            ajaxProNode2.ReplaceWith(resourceHandler);
        }

        var handlersNode = webConfig.XPathSelectElement("/configuration/system.webServer/handlers");

        if (handlersNode is null)
        {
            var webServerNode = webConfig.XPathSelectElement("/configuration/system.webServer");

            var handlersElement = XElement.Parse(
                """
                <handlers>
                                        <add name="YafHandler" preCondition="integratedMode" verb="GET"
                                           path="Resource.ashx" type="YAF.Core.Handlers.ResourceHandler, YAF.Core" />
                                        <add name="YafSitemapHandler" path="Sitemap.xml"
                                           verb="*" type="YAF.Core.Handlers.SiteMapHandler, YAF.Core" preCondition="integratedMode" />
                                        <add name="FileUploader" path="FileUploader.ashx"
                                          verb="*" type="YAF.Core.Handlers.FileUploader, YAF.Core" preCondition="integratedMode" />
                                      </handlers>
                """);

            webServerNode.Add(handlersElement);

            handlersNode = webConfig.XPathSelectElement("/configuration/system.webServer/handlers");
        }

        var resourceHandlerNodeNew =
            webConfig.XPathSelectElement("/configuration/system.webServer/handlers/add[@path='Resource.ashx']");

        if (resourceHandlerNodeNew is null)
        {
            var resourceHandlerElement = XElement.Parse(
                """
                <add name="YafHandler" preCondition="integratedMode" verb="GET"
                                           path="Resource.ashx" type="YAF.Core.Handlers.ResourceHandler, YAF.Core" />
                """);

            handlersNode.Add(resourceHandlerElement);
        }

        // Add SiteMap and FileUploader Handlers
        var handler1 = webConfig.XPathSelectElement(
            "/configuration/system.webServer/handlers/add[@name='YafSitemapHandler']");

        if (handler1 == null)
        {
            var siteMapHandler = XElement.Parse(
                """
                <add name="YafSitemapHandler" path="Sitemap.xml"
                                       verb="*" type="YAF.Core.Handlers.SiteMapHandler, YAF.Core" preCondition="integratedMode" />
                """);

            if (resourceHandlerNodeNew is null)
            {
                handlersNode.Add(siteMapHandler);
            }
            else
            {
                resourceHandlerNodeNew.AddAfterSelf(siteMapHandler);
            }
        }

        var handler2 =
            webConfig.XPathSelectElement("/configuration/system.webServer/handlers/add[@name='FileUploader']");

        if (handler2 == null)
        {
            var fileUploadHandler = XElement.Parse(
                """
                <add name="FileUploader" path="FileUploader.ashx"
                                       verb="*" type="YAF.Core.Handlers.FileUploader, YAF.Core" preCondition="integratedMode" />
                """);

            if (resourceHandlerNodeNew is null)
            {
                handlersNode.Add(fileUploadHandler);
            }
            else
            {
                resourceHandlerNodeNew.AddAfterSelf(fileUploadHandler);
            }
        }

        // Add HttpGet and HttpPost Protocols
        var webServicesNode =
            webConfig.XPathSelectElement("/configuration/system.web/webServices/protocols/add[@name='HttpGet']");

        if (webServicesNode == null)
        {
            var webServicesElement = XElement.Parse(
                """
                <webServices>
                                               <protocols>
                                                  <add name="HttpGet" />
                                                  <add name="HttpPost" />
                                               </protocols >
                                           </webServices>
                """);

            var systemWebNode = webConfig.XPathSelectElement("/configuration/system.web/pages");

            systemWebNode.AddAfterSelf(webServicesElement);
        }

        /// Add MimeMap
        var woffNode = webConfig.XPathSelectElement("/configuration/system.webServer/staticContent");

        if (woffNode == null)
        {
            var addElement = XElement.Parse(
                """
                <staticContent>
                      <remove fileExtension=".ico" />
                      <mimeMap fileExtension=".ico" mimeType="image/x-icon" />
                      <remove fileExtension=".ttf" />
                      <remove fileExtension=".svg" />
                      <remove fileExtension=".woff" />
                      <remove fileExtension=".woff2" />
                      <mimeMap fileExtension=".woff" mimeType="application/x-font-woff" />
                      <mimeMap fileExtension=".ttf" mimeType="application/font-sfnt" />
                      <mimeMap fileExtension=".svg" mimeType="image/svg+xml" />
                      <mimeMap fileExtension=".woff2" mimeType="font/woff2" />
                      <mimeMap fileExtension=".webmanifest" mimeType="application/manifest+json" />
                    </staticContent>
                """);

            handlersNode.AddAfterSelf(addElement);
        }

        // Change Target Framework
        var httpRuntimeNode = webConfig.XPathSelectElement("/configuration/system.web/httpRuntime");

        if (httpRuntimeNode != null)
        {
            try
            {
                if (httpRuntimeNode.Attribute("targetFramework").Value != "4.8")
                {
                    httpRuntimeNode.Attribute("targetFramework").Value = "4.8";
                }
            }
            catch (Exception)
            {
                var targetFramework = new XAttribute("targetFramework", "4.8");
                httpRuntimeNode.Add(targetFramework);
            }
        }

        var compilationNode = webConfig.XPathSelectElement("/configuration/system.web/compilation");

        try
        {
            if (compilationNode.Attribute("targetFramework").Value != "4.8")
            {
                compilationNode.Attribute("targetFramework").Value = "4.8";
            }
        }
        catch (Exception)
        {
            var targetFramework = new XAttribute("targetFramework", "4.8");

            compilationNode?.Add(targetFramework);
        }

        this.MergedOutput = webConfig.ToString();

        if (appSettings.ToString().IsSet())
        {
            this.AppSettings = appSettings.ToString();
        }
    }
}