﻿// FileName:  Window.cs
// Author:  guodp <guodp9u0@gmail.com>
// Create Date:  20180205 13:58
// Description:   

#region

using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

#endregion

namespace mhxy.Client.OpenTk {

    /// <summary>
    ///     游戏窗口
    /// </summary>
    public class GlWindow : GameWindow {

        private int _textureLowest;
        private int _textureLower;
        private int _textureNormal;
        private int _textureHigher;
        private int _textureHighest;
        private GlProgram _mProgram;
        private int _mLocation1 = -1;
        private int _mLocation2 = -1;

        // private readonly ILogger _logger = ServiceLocator.GlobalLogger;

        #region

        private int _fpsCount;

        #endregion

        private int _mVao;
        private int _mVbo;
        private int _mEbo;

        /// <summary>
        /// </summary>
        public new void Run() {
            Run(Global.FramePerSecond);
        }


        protected override void OnLoad(EventArgs e) {
            _mProgram = new GlProgram(@"Resources/texture.vert", @"Resources/texture.frag");
            _mProgram.Use();
            _mLocation1 = _mProgram.GetUniformLocation("ourTexture1");
            _mLocation2 = _mProgram.GetUniformLocation("ourTexture2");
            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e) {
            //_logger.Debug($"OnUpdateFrame {e.Time}");
            ServiceLocator.DrawingService.UpdateFrame();
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            //_logger.Debug($"OnRenderFrame {e.Time}");
#if DEBUG
            _fpsCount++;
            if (_fpsCount == 60) {
                Title = $"(Vsync: {VSync}),FPS: {1f / e.Time:0}";
                _fpsCount = 0;
            }
#endif
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var canvas = ServiceLocator.DrawingService.Draw();
            GL.DeleteTexture(_textureLowest);
            GL.DeleteTexture(_textureLower);
            GL.DeleteTexture(_textureNormal);
            GL.DeleteTexture(_textureHigher);
            GL.DeleteTexture(_textureHighest);

            _textureLowest = LoadTextureFromBitmap(canvas.Lowest);
            _textureLower = LoadTextureFromBitmap(canvas.Lower);
            _textureNormal = LoadTextureFromBitmap(canvas.Normal);
            _textureHigher = LoadTextureFromBitmap(canvas.Higher);
            _textureHighest = LoadTextureFromBitmap(canvas.Highest);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _textureLowest);
            GL.Uniform1(_mLocation1, 0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, _textureLower);
            GL.Uniform1(_mLocation2, 1);

            Draw();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        private int LoadTextureFromBitmap(Bitmap bitmap) {
            var textureId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureId);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bitmap.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            return textureId;
        }

        private void Build() {
            float[] vertices = {
                // Positions          // Colors           // Texture Coords
                0.5f * 2, 0.5f * 2, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, // Top Right
                0.5f * 2, -0.5f * 2, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, // Bottom Right
                -0.5f * 2, -0.5f * 2, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Bottom Left
                -0.5f * 2, 0.5f * 2, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f // Top Left 
            };
            int[] indices = {
                // Note that we start from 0!
                0, 1, 3, // First Triangle
                1, 2, 3 // Second Triangle
            };
            _mVao = GL.GenVertexArray();
            _mVbo = GL.GenBuffer();
            _mEbo = GL.GenBuffer();
            GL.BindVertexArray(_mVao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _mVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices,
                BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _mEbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indices.Length, indices,
                BufferUsageHint.StaticDraw);
            // Position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // Color attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            // TexCoord attribute
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        private void Draw() {
            if (_mVao <= 0) {
                Build();
            }

            if (_mVao > 0) {
                GL.BindVertexArray(_mVao);
                GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
                GL.BindVertexArray(0);
            }
        }

        #region Ctor

        public GlWindow() {
        }

        public GlWindow(int width, int height)
            : base(width, height) {
        }

        public GlWindow(int width, int height, GraphicsMode mode)
            : base(width, height, mode) {
        }

        public GlWindow(int width, int height, GraphicsMode mode, string title)
            : base(width, height, mode, title) {
        }

        public GlWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options)
            : base(width, height, mode, title, options) {
        }

        public GlWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options
            , DisplayDevice device)
            : base(width, height, mode, title, options, device) {
        }

        public GlWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options
            , DisplayDevice device, int major, int minor, GraphicsContextFlags flags)
            : base(width, height, mode, title, options, device, major, minor, flags) {
        }

        public GlWindow(int width, int height, GraphicsMode mode, string title, GameWindowFlags options
            , DisplayDevice device, int major, int minor, GraphicsContextFlags flags, IGraphicsContext sharedContext)
            : base(width, height, mode, title, options, device, major, minor, flags, sharedContext) {
        }

        #endregion

    }

}