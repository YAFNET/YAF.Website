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

using YAF.Types.Extensions;

namespace YAF.WebSite.Utilities;

using System.Collections.Generic;
using System.Security.Cryptography;

/// <summary>
/// Helper Class to generate a Machine Key String
/// </summary>
public static class MachineKeyCreator
{
    /// <summary>
    /// Creates the key string.
    /// </summary>
    /// <returns>Returns the Key String</returns>
    public static string CreateKeyString()
    {
        var decryptionKey = CreateKey(24);
        var validationKey = CreateKey(64);

        return
            $"<machineKey validationKey=\"{validationKey}\" \n decryptionKey=\"{decryptionKey}\" \n validation=\"HMACSHA256\" decryption=\"AES\" />";
    }

    /// <summary>
    /// Creates the key.
    /// </summary>
    /// <param name="numBytes">The number bytes.</param>
    /// <returns>Returns the Key</returns>
    private static string CreateKey(int numBytes)
    {
        var rng = RandomNumberGenerator.Create();
        var buff = new byte[numBytes];

        rng.GetBytes(buff);
        return BytesToHexString(buff);
    }

    /// <summary>
    /// Convert Bytes to a hexadecimal string.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <returns>Returns the Hex String</returns>
    private static string BytesToHexString(IEnumerable<byte> bytes)
    {
        var hexString = new StringBuilder(64);

        bytes.ForEach(t => hexString.Append($"{t:X2}"));

        return hexString.ToString();
    }
}