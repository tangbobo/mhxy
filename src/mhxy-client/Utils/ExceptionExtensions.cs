﻿// FileName:  ExceptionExtensions.cs
// Author:  guodp <guodp9u0@gmail.com>
// Create Date:  20180202 08:53
// Description:   

#region

using System;
using System.Text;

#endregion

namespace mhxy.Utils {

    /// <summary>
    ///     异常操作扩展
    /// </summary>
    public static class ExceptionExtensions {

        /// <summary>
        ///     格式化异常消息
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="isHideStackTrace">是否隐藏异常规模信息</param>
        /// <returns>格式化后的异常信息字符串</returns>
        public static string FormatMessage(this Exception e, bool isHideStackTrace = false) {
            var sb = new StringBuilder();
            var count = 0;
            string appString = string.Empty;
            while (e != null) {
                if (count > 0)
                    appString += "  ";
                sb.AppendLine($"{appString}异常消息：{e.Message}");
                sb.AppendLine($"{appString}异常类型：{e.GetType().FullName}");
                sb.AppendLine($"{appString}异常方法：{(e.TargetSite == null ? null : e.TargetSite.Name)}");
                sb.AppendLine($"{appString}异常源：{e.Source}");
                if (!isHideStackTrace && e.StackTrace != null)
                    sb.AppendLine($"{appString}异常堆栈：{e.StackTrace}");
                if (e.InnerException != null) {
                    sb.AppendLine($"{appString}内部异常：");
                    count++;
                }

                e = e.InnerException;
            }

            return sb.ToString();
        }

    }

}