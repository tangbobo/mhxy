﻿// FileName:  ServiceLocator.cs
// Author:  guodp <guodp9u0@gmail.com>
// Create Date:  20180202 08:53
// Description:   

#region

using mhxy.Client;
using mhxy.Display;
using mhxy.Log4Net;
using mhxy.Logging;
using mhxy.Resource;
using mhxy.Resource.Configs;
using mhxy.Resource.Maps;
using mhxy.Resource.Profiles;
using mhxy.Resource.Wdfs;
using OpenTK;
using OpenTK.Graphics;

#endregion

namespace mhxy {

    /// <summary>
    ///     简单的服务定位器
    /// </summary>
    public static class ServiceLocator {

        /// <summary>
        /// </summary>
        static ServiceLocator() {
            LogManager.AddLoggerAdapter(new Log4NetLoggerAdapter());
            AppLogger = LogManager.GetLogger(typeof(ServiceLocator));
            ConfigManager = new ConfigManager(Environment.ConfigPath);
            ProfileService = new ProfileService(Environment.ProfilePath);
            MapManager = new MapManager(Environment.MapPath);
            WdfManager = new WdfManager(Environment.WdfPath);
            WasManager = new WasManager();
            DrawingService = new DrawingService();
            ClientEngine = new ClientEngine();
            GameWindow = new MhxyGameWindow(Environment.Width, Environment.Height, GraphicsMode.Default
                , Environment.Title, GameWindowFlags.FixedWindow);
        }

        /// <summary>
        ///     全局日志功能
        /// </summary>
        public static ILogger AppLogger { get; }

        /// <summary>
        ///     地图管理器
        /// </summary>
        public static IMapManager MapManager { get; }

        /// <summary>
        ///     WDF管理器
        /// </summary>
        public static IWdfManager WdfManager { get; }

        /// <summary>
        ///     注解配置管理器
        /// </summary>
        public static IConfigManager ConfigManager { get; }

        /// <summary>
        ///     Was管理器
        /// </summary>
        public static IWasManager WasManager { get; }

        /// <summary>
        ///     绘制服务
        /// </summary>
        public static IDrawingService DrawingService { get; }

        /// <summary>
        ///     存档服务
        /// </summary>
        public static IProfileService ProfileService { get; }

        /// <summary>
        ///     客户端引擎
        /// </summary>
        public static IClientEngine ClientEngine { get; }

        /// <summary>
        ///     游戏主窗口
        /// </summary>
        public static MhxyGameWindow GameWindow { get; }

    }

}