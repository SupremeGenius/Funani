﻿/*
 * Copyright (c) 2008-2013, Jaap de Haan <jaap.dehaan@color-of-code.de>
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 *   * Redistributions of source code must retain the above copyright notice,
 *     this list of conditions and the following disclaimer.
 *   * Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 *   * Neither the name of the "Color-Of-Code" nor the names of its
 *     contributors may be used to endorse or promote products derived from
 *     this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace Funani.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Funani.Api.Metadata;

    public interface IEngine
    {
        /// <summary>
        /// Allows to check if the provided path points to a valid funani db structure
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Boolean IsValidDatabase(String path);

        /// <summary>
        /// Opens a funani database initializing the path if it is initially empty
        /// </summary>
        /// <param name="path">Path to the directory containing the funani data</param>
        void OpenDatabase(String pathToMongod, String path, IConsoleRedirect listener);

        /// <summary>
        /// Closes the funani database and flushes any pending operation to disk
        /// </summary>
        void CloseDatabase();

        /// <summary>
        /// Add a file to the database (if not already present) and get its info back
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        FileInformation AddFile(FileInfo file);

        /// <summary>
        /// Remove a file path from the metadata but leaves the file inside the storage area.
        /// A file gets physically removed only by purging the database.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        void RemoveFile(FileInfo file);

        /// <summary>
        /// Get the info associated in mongodb for the path of the specified file.
        /// If no info could be found, null is returned. This method is intended for
        /// frequent use and does not make any other check than the path/size/timestamp
        /// check
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        FileInformation GetFileInformation(FileInfo file);

        /// <summary>
        /// Returns the file data of the file with the given hash
        /// </summary>
        /// <param name="hash">SHA1 hash of the file to retrieve</param>
        /// <returns>The file contents as a byte array</returns>
        byte[] GetFileData(String hash);

    }
}
