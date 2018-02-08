﻿// FileName:  IDrawable.cs
// Author:  guodp <guodp9u0@gmail.com>
// Create Date:  20180205 13:58
// Description:   

namespace mhxy.Client {

    /// <summary>
    ///     可绘制对象接口
    /// </summary>
    public interface IDrawable {

        DrawPriority Priority { get; }

        /// <summary>
        ///     刷新到下一帧
        /// </summary>
        void NextFrame();

        /// <summary>
        /// </summary>
        /// <param name="cancas"></param>
        void Draw(DrawArgs cancas);

    }

}