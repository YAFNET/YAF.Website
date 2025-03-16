/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bj�rnar Henden
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

using YAF.Core.Context;
using YAF.WebSite.Utilities;

namespace YAF.Website.Pages;

/// <summary>
/// Class KeyModel.
/// Implements the <see cref="PageModel" />
/// Implements the <see cref="YAF.Types.Interfaces.IHaveServiceLocator" />
/// </summary>
/// <seealso cref="PageModel" />
/// <seealso cref="YAF.Types.Interfaces.IHaveServiceLocator" />
public class KeyModel : PageModel, IHaveServiceLocator
{
    /// <summary>
    /// Gets or sets the output key.
    /// </summary>
    /// <value>The output key.</value>
    [BindProperty]
    public string OutputKey { get; set; }

    /// <summary>
    /// Gets the service locator.
    /// </summary>
    /// <value>The service locator.</value>
    public IServiceLocator ServiceLocator => BoardContext.Current.ServiceLocator;

    /// <summary>
    /// Called when [post].
    /// </summary>
    public void OnPost()
    {
        this.OutputKey = MachineKeyCreator.CreateKeyString();
    }
}