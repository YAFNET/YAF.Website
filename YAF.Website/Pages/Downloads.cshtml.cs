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

using Microsoft.AspNetCore.Mvc.RazorPages;

using Octokit;

using YAF.Core.Context;
using YAF.Types.Extensions;

namespace YAF.Website.Pages;

/// <summary>
/// Class DownloadsModel.
/// Implements the <see cref="PageModel" />
/// Implements the <see cref="YAF.Types.Interfaces.IHaveServiceLocator" />
/// </summary>
/// <seealso cref="PageModel" />
/// <seealso cref="YAF.Types.Interfaces.IHaveServiceLocator" />
public class DownloadsModel : PageModel, IHaveServiceLocator
{
    /// <summary>
    /// Gets the service locator.
    /// </summary>
    /// <value>The service locator.</value>
    public IServiceLocator ServiceLocator => BoardContext.Current.ServiceLocator;

    /// <summary>
    /// Gets or sets the release DNN install.
    /// </summary>
    /// <value>The release DNN install.</value>
    [BindProperty]
    public string ReleaseDnnInstall { get; set; }

    /// <summary>
    /// Gets or sets the release sample application.
    /// </summary>
    /// <value>The release sample application.</value>
    [BindProperty]
    public string ReleaseSampleApplication { get; set; }

    /// <summary>
    /// Gets or sets the release sample application.
    /// </summary>
    /// <value>The release sample application.</value>
    [BindProperty]
    public string ReleaseSampleApp { get; set; }

    /// <summary>
    /// Gets or sets the release DNN install downloads.
    /// </summary>
    /// <value>The release DNN install downloads.</value>
    [BindProperty]
    public string ReleaseDnnInstallDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release DNN source.
    /// </summary>
    /// <value>The release DNN source.</value>
    [BindProperty]
    public string ReleaseDnnSource { get; set; }

    /// <summary>
    /// Gets or sets the release DNN title.
    /// </summary>
    /// <value>The release DNN title.</value>
    [BindProperty]
    public string ReleaseDnnTitle { get; set; }

    /// <summary>
    /// Gets or sets the release sample application title.
    /// </summary>
    /// <value>The release sample application title.</value>
    [BindProperty]
    public string ReleaseSampleApplicationTitle { get; set; }


    /// <summary>
    /// Gets or sets the release sample application title.
    /// </summary>
    /// <value>The release sample application title.</value>
    [BindProperty]
    public string ReleaseSampleAppTitle { get; set; }

    /// <summary>
    /// Gets or sets the release source.
    /// </summary>
    /// <value>The release source.</value>
    [BindProperty]
    public string ReleaseSource { get; set; }

    /// <summary>
    /// Gets or sets the release title.
    /// </summary>
    /// <value>The release title.</value>
    [BindProperty]
    public string ReleaseTitle { get; set; }

    /// <summary>
    /// Gets or sets the release my SQL install.
    /// </summary>
    /// <value>The release my SQL install.</value>
    [BindProperty]
    public string ReleaseMySqlInstall { get; set; }

    /// <summary>
    /// Gets or sets the release my SQL install downloads.
    /// </summary>
    /// <value>The release my SQL install downloads.</value>
    [BindProperty]
    public string ReleaseMySqlInstallDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release my SQL upgrade.
    /// </summary>
    /// <value>The release my SQL upgrade.</value>
    [BindProperty]
    public string ReleaseMySqlUpgrade { get; set; }

    /// <summary>
    /// Gets or sets the release my SQL upgrade downloads.
    /// </summary>
    /// <value>The release my SQL upgrade downloads.</value>
    [BindProperty]
    public string ReleaseMySqlUpgradeDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release ms SQL install.
    /// </summary>
    /// <value>The release ms SQL install.</value>
    [BindProperty]
    public string ReleaseMsSqlInstall { get; set; }

    /// <summary>
    /// Gets or sets the release ms SQL install downloads.
    /// </summary>
    /// <value>The release ms SQL install downloads.</value>
    [BindProperty]
    public string ReleaseMsSqlInstallDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release ms SQL upgrade.
    /// </summary>
    /// <value>The release ms SQL upgrade.</value>
    [BindProperty]
    public string ReleaseMsSqlUpgrade { get; set; }

    /// <summary>
    /// Gets or sets the release ms SQL upgrade downloads.
    /// </summary>
    /// <value>The release ms SQL upgrade downloads.</value>
    [BindProperty]
    public string ReleaseMsSqlUpgradeDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release sqlite install.
    /// </summary>
    /// <value>The release sqlite install.</value>
    [BindProperty]
    public string ReleaseSqliteInstall { get; set; }

    /// <summary>
    /// Gets or sets the release sqlite install downloads.
    /// </summary>
    /// <value>The release sqlite install downloads.</value>
    [BindProperty]
    public string ReleaseSqliteInstallDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release sqlite upgrade.
    /// </summary>
    /// <value>The release sqlite upgrade.</value>
    [BindProperty]
    public string ReleaseSqliteUpgrade { get; set; }

    /// <summary>
    /// Gets or sets the release sqlite upgrade downloads.
    /// </summary>
    /// <value>The release sqlite upgrade downloads.</value>
    [BindProperty]
    public string ReleaseSqliteUpgradeDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release postgre SQL install.
    /// </summary>
    /// <value>The release postgre SQL install.</value>
    [BindProperty]
    public string ReleasePostgreSqlInstall { get; set; }

    /// <summary>
    /// Gets or sets the release postgre SQL install downloads.
    /// </summary>
    /// <value>The release postgre SQL install downloads.</value>
    [BindProperty]
    public string ReleasePostgreSqlInstallDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release postgre SQL upgrade.
    /// </summary>
    /// <value>The release postgre SQL upgrade.</value>
    [BindProperty]
    public string ReleasePostgreSqlUpgrade { get; set; }

    /// <summary>
    /// Gets or sets the release postgre SQL upgrade downloads.
    /// </summary>
    /// <value>The release postgre SQL upgrade downloads.</value>
    [BindProperty]
    public string ReleasePostgreSqlUpgradeDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release core source.
    /// </summary>
    /// <value>The release core source.</value>
    [BindProperty]
    public string ReleaseCoreSource { get; set; }

    /// <summary>
    /// Gets or sets the release core title.
    /// </summary>
    /// <value>The release core title.</value>
    [BindProperty]
    public string ReleaseCoreTitle { get; set; }

    /// <summary>
    /// Gets or sets the release my SQL core URL.
    /// </summary>
    /// <value>The release my SQL core URL.</value>
    [BindProperty]
    public string ReleaseMySqlCoreUrl { get; set; }

    /// <summary>
    /// Gets or sets the release my SQL core downloads.
    /// </summary>
    /// <value>The release my SQL core downloads.</value>
    [BindProperty]
    public string ReleaseMySqlCoreDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release ms SQL core URL.
    /// </summary>
    /// <value>The release ms SQL core URL.</value>
    [BindProperty]
    public string ReleaseMsSqlCoreUrl { get; set; }

    /// <summary>
    /// Gets or sets the release ms SQL core downloads.
    /// </summary>
    /// <value>The release ms SQL core downloads.</value>
    [BindProperty]
    public string ReleaseMsSqlCoreDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release postgre SQL core URL.
    /// </summary>
    /// <value>The release postgre SQL core URL.</value>
    [BindProperty]
    public string ReleasePostgreSqlCoreUrl { get; set; }

    /// <summary>
    /// Gets or sets the release postgre SQL core downloads.
    /// </summary>
    /// <value>The release postgre SQL core downloads.</value>
    [BindProperty]
    public string ReleasePostgreSqlCoreDownloads { get; set; }

    /// <summary>
    /// Gets or sets the release sqlite core URL.
    /// </summary>
    /// <value>The release sqlite core URL.</value>
    [BindProperty]
    public string ReleaseSqliteCoreUrl { get; set; }

    /// <summary>
    /// Gets or sets the release sqlite core downloads.
    /// </summary>
    /// <value>The release sqlite core downloads.</value>
    [BindProperty]
    public string ReleaseSqliteCoreDownloads { get; set; }

    /// <summary>
    /// Called when [get].
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task OnGetAsync()
    {
        var githubClient = new GitHubClient(new ProductHeaderValue("YAF.NET"));

        await this.GetDnnReleasesAsync(githubClient);

        var sampleAppReleases =  await githubClient.Repository.Release.GetAll("YAFNET", "YAF.SampleWebApplication");

        await this.GetSampleApplicationReleasesAsync(sampleAppReleases);

        await this.GetSampleAppReleasesAsync(sampleAppReleases);

        await this.GetMySqlReleasesAsync(githubClient);

        await this.GetMsSqlReleasesAsync(githubClient);

        await this.GetSqliteReleasesAsync(githubClient);

        await this.GetPostgreSqlReleasesAsync(githubClient);

        await this.GetNetCoreReleasesAsync(githubClient);
    }

    /// <summary>
    /// Gets the net core releases.
    /// </summary>
    /// <param name="githubClient"></param>
    private async Task GetNetCoreReleasesAsync(GitHubClient githubClient)
    {
        Release release;

        if (this.Get<IDataCache>().Get("ReleaseNetCore") == null)
        {
            var releases = await githubClient.Repository.Release.GetAll("YAFNET", "YAFNET");

            release = releases.First(r => r.TagName.StartsWith("v4") && !r.Prerelease);

            this.Get<IDataCache>().Set("ReleaseNetCore", release);
        }
        else
        {
            release = this.Get<IDataCache>().Get("ReleaseNetCore").ToType<Release>();
        }

        ReleaseCoreSource = release.ZipballUrl;
        ReleaseCoreTitle = release.Name;

        ReleaseMySqlCoreUrl = release.Assets[0].BrowserDownloadUrl;
        ReleaseMySqlCoreDownloads = $"{release.Assets[0].DownloadCount} downloads";

        ReleaseMsSqlCoreUrl = release.Assets[3].BrowserDownloadUrl;
        ReleaseMsSqlCoreDownloads = $"{release.Assets[3].DownloadCount} downloads";

        ReleasePostgreSqlCoreUrl = release.Assets[1].BrowserDownloadUrl;
        ReleasePostgreSqlCoreDownloads = $"{release.Assets[1].DownloadCount} downloads";

        ReleaseSqliteCoreUrl = release.Assets[2].BrowserDownloadUrl;
        ReleaseSqliteCoreDownloads = $"{release.Assets[2].DownloadCount} downloads";
    }

    /// <summary>
    /// Gets my SQL releases.
    /// </summary>
    /// <param name="githubClient"></param>
    private async Task GetMySqlReleasesAsync(GitHubClient githubClient)
    {
        Release release;

        if (this.Get<IDataCache>().Get("Release") == null)
        {
            release = await githubClient.Repository.Release.GetLatest("YAFNET", "YAFNET");
            this.Get<IDataCache>().Set("Release", release);
        }
        else
        {
            release = this.Get<IDataCache>().Get("Release").ToType<Release>();
        }

        ReleaseMySqlInstall = release.Assets[1].BrowserDownloadUrl;
        ReleaseMySqlInstallDownloads = $"{release.Assets[1].DownloadCount} downloads";
        ReleaseMySqlUpgrade = release.Assets[2].BrowserDownloadUrl;
        ReleaseMySqlUpgradeDownloads = $"{release.Assets[2].DownloadCount} downloads";

        ReleaseSource = release.ZipballUrl;
        ReleaseTitle = release.Name;
    }

    /// <summary>
    /// Gets the ms SQL releases.
    /// </summary>
    /// <param name="githubClient"></param>
    private async Task GetMsSqlReleasesAsync(GitHubClient githubClient)
    {
        Release release;

        if (this.Get<IDataCache>().Get("Release") == null)
        {
            release = await githubClient.Repository.Release.GetLatest("YAFNET", "YAFNET");
            this.Get<IDataCache>().Set("Release", release);
        }
        else
        {
            release = this.Get<IDataCache>().Get("Release").ToType<Release>();
        }

        ReleaseMsSqlInstall = release.Assets[6].BrowserDownloadUrl;
        ReleaseMsSqlInstallDownloads = $"{release.Assets[6].DownloadCount} downloads";
        ReleaseMsSqlUpgrade = release.Assets[7].BrowserDownloadUrl;
        ReleaseMsSqlUpgradeDownloads = $"{release.Assets[7].DownloadCount} downloads";
    }

    /// <summary>
    /// Gets the sqlite releases.
    /// </summary>
    /// <param name="githubClient"></param>
    private async Task GetSqliteReleasesAsync(GitHubClient githubClient)
    {
        Release release;

        if (this.Get<IDataCache>().Get("Release") == null)
        {
            release = await githubClient.Repository.Release.GetLatest("YAFNET", "YAFNET");
            this.Get<IDataCache>().Set("Release", release);
        }
        else
        {
            release = this.Get<IDataCache>().Get("Release").ToType<Release>();
        }

        ReleaseSqliteInstall = release.Assets[5].BrowserDownloadUrl;
        ReleaseSqliteInstallDownloads = $"{release.Assets[5].DownloadCount} downloads";
        ReleaseSqliteUpgrade = release.Assets[0].BrowserDownloadUrl;
        ReleaseSqliteUpgradeDownloads = $"{release.Assets[0].DownloadCount} downloads";
    }

    /// <summary>
    /// Gets the postgre SQL releases.
    /// </summary>
    /// <param name="githubClient"></param>
    private async Task GetPostgreSqlReleasesAsync(GitHubClient githubClient)
    {
        Release release;

        if (this.Get<IDataCache>().Get("Release") == null)
        {
            release = await githubClient.Repository.Release.GetLatest("YAFNET", "YAFNET");
            this.Get<IDataCache>().Set("Release", release);
        }
        else
        {
            release = this.Get<IDataCache>().Get("Release").ToType<Release>();
        }

        ReleasePostgreSqlInstall = release.Assets[3].BrowserDownloadUrl;
        ReleasePostgreSqlInstallDownloads = $"{release.Assets[3].DownloadCount} downloads";
        ReleasePostgreSqlUpgrade = release.Assets[4].BrowserDownloadUrl;
        ReleasePostgreSqlUpgradeDownloads = $"{release.Assets[4].DownloadCount} downloads";
    }

    /// <summary>
    /// Gets the DNN releases.
    /// </summary>
    /// <param name="githubClient"></param>
    private async Task GetDnnReleasesAsync(GitHubClient githubClient)
    {
        Release dnnRelease;

        if (this.Get<IDataCache>().Get("DnnRelease") == null)
        {
            dnnRelease = await githubClient.Repository.Release.GetLatest("YAFNET", "YAFNET-DNN");
            this.Get<IDataCache>().Set("DnnRelease", dnnRelease);
        }
        else
        {
            dnnRelease = this.Get<IDataCache>().Get("DnnRelease").ToType<Release>();
        }

        ReleaseDnnInstall = dnnRelease.Assets[1].BrowserDownloadUrl;
        ReleaseDnnInstallDownloads = $"{dnnRelease.Assets[1].DownloadCount} downloads";
        ReleaseDnnSource = dnnRelease.Assets[0].BrowserDownloadUrl;
        ReleaseDnnTitle = dnnRelease.Name;
    }

    /// <summary>
    /// Gets the Sample Application (v3) releases.
    /// </summary>
    /// 
    /// <param name="sampleAppReleases">list of Releases.</param>
    private Task GetSampleApplicationReleasesAsync(IReadOnlyList<Release> sampleAppReleases)
    {
        Release release;

        if (this.Get<IDataCache>().Get("SampleApplicationRelease") == null)
        {
            release = sampleAppReleases.First(r => r.TagName.StartsWith("v3") && !r.Prerelease);

            this.Get<IDataCache>().Set("SampleApplicationRelease", release);
        }
        else
        {
            release = this.Get<IDataCache>().Get("SampleApplicationRelease").ToType<Release>();
        }

        ReleaseSampleApplication = release.ZipballUrl;
        ReleaseSampleApplicationTitle = release.Name;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the Sample App (v4) releases.
    /// </summary>
    /// <param name="sampleAppReleases">list of Releases.</param>
    private Task GetSampleAppReleasesAsync(IReadOnlyList<Release> sampleAppReleases)
    {
        Release release;

        if (this.Get<IDataCache>().Get("SampleAppRelease") == null)
        {
            release = sampleAppReleases.First(r => r.TagName.StartsWith("v4") && !r.Prerelease);

            this.Get<IDataCache>().Set("SampleAppRelease", release);
        }
        else
        {
            release = this.Get<IDataCache>().Get("SampleAppRelease").ToType<Release>();
        }

        ReleaseSampleApp = release.ZipballUrl;
        ReleaseSampleAppTitle = release.Name;
        return Task.CompletedTask;
    }
}