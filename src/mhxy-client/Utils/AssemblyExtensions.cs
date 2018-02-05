﻿// FileName:  AssemblyExtensions.cs
// Author:  guodp <guodp9u0@gmail.com>
// Create Date:  20180202 10:51
// Description:   

#region

using System;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace mhxy.Utils {

    /// <summary>
    ///     程序集扩展操作类
    /// </summary>
    public static class AssemblyExtensions {

        /// <summary>
        ///     获取程序集的文件版本
        /// </summary>
        public static Version GetFileVersion(this Assembly assembly) {
            assembly.CheckNotNull("assembly");
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.FileVersion);
        }

        /// <summary>
        ///     获取程序集的产品版本
        /// </summary>
        public static Version GetProductVersion(this Assembly assembly) {
            assembly.CheckNotNull("assembly");
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.ProductVersion);
        }

    }

}