/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2025 Ingo Herbote
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

using Autofac;
using Autofac.Extensions.DependencyInjection;

using System.Net;

using Microsoft.AspNetCore.Rewrite;

using YAF.Core.Context;
using YAF.Core.Extensions;
using YAF.Core.Hubs;
using YAF.Core.Middleware;
using YAF.RazorPages;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureYafLogging();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterYafModules();
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddAreaPageRoute("Forums", "/SiteMap", "/Sitemap.xml");
}).AddYafRazorPages(builder.Environment);

builder.Services.AddYafCore(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Forums/Error");

    app.UseHsts();
}

app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString();

    if (path.Contains("/RegisterV", StringComparison.InvariantCultureIgnoreCase))
    {
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        return;
    }

    await next();
});

app.RegisterAutofac();

app.UseAntiXssMiddleware();

using (var iisUrlRewriteStreamReader = File.OpenText(Path.Combine(app.Environment.ContentRootPath, "IISUrlRewrite.xml")))
{
    var options = new RewriteOptions().AddIISUrlRewrite(iisUrlRewriteStreamReader);
    app.UseRewriter(options);
}

app.UseStaticFiles();

app.UseSession();

app.UseYafCore(BoardContext.Current.ServiceLocator);

app.UseRobotsTxt(app.Environment);

app.MapRazorPages();

app.MapAreaControllerRoute(
    name: "default",
    areaName: "Forums",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.MapHub<NotificationHub>("/NotificationHub");
app.MapHub<ChatHub>("/ChatHub");

await app.RunAsync();