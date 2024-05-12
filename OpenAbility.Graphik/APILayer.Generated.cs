// Auto-generated IGraphikAPI bindings!
// These should not be modified
using System;
using OpenAbility.Graphik;
using System.Numerics;
namespace OpenAbility.Graphik;
        
public partial class APILayer : IGraphikAPI
{
        public virtual void InitializeSystems() => Intercept("InitializeSystems", Underlying.InitializeSystems);
        public virtual IGraphikWindow InitializeWindow(String title, int width, int height) => (IGraphikWindow)Intercept("InitializeWindow", Underlying.InitializeWindow, title, width, height);
        public void SetWindowCurrent(IGraphikWindow window) => Intercept("SetWindowCurrent", Underlying.SetWindowCurrent, window);
        public virtual void SetErrorCallback(ErrorCallback errorCallback) => Intercept("SetErrorCallback", Underlying.SetErrorCallback, errorCallback);
        public virtual void SetDebugCallback(DebugCallback debugCallback) => Intercept("SetDebugCallback", Underlying.SetDebugCallback, debugCallback);
        public virtual void SetResizeCallback(ResizeCallback resizeCallback) => Intercept("SetResizeCallback", Underlying.SetResizeCallback, resizeCallback);
        public virtual void SetKeyCallback(KeyCallback keyCallback) => Intercept("SetKeyCallback", Underlying.SetKeyCallback, keyCallback);
        public virtual void SetMouseCallback(MouseCallback mouseCallback) => Intercept("SetMouseCallback", Underlying.SetMouseCallback, mouseCallback);
        public virtual void SetCursorCallback(CursorCallback cursorCallback) => Intercept("SetCursorCallback", Underlying.SetCursorCallback, cursorCallback);
        public virtual void SetTypeCallback(TypeCallback typeCallback) => Intercept("SetTypeCallback", Underlying.SetTypeCallback, typeCallback);
        public virtual void SetScrollCallback(ScrollCallback scrollCallback) => Intercept("SetScrollCallback", Underlying.SetScrollCallback, scrollCallback);
        public virtual bool WindowShouldClose() => (bool)Intercept("WindowShouldClose", Underlying.WindowShouldClose);
        public virtual void InitializeFrame() => Intercept("InitializeFrame", Underlying.InitializeFrame);
        public virtual void FinishFrame() => Intercept("FinishFrame", Underlying.FinishFrame);
        public virtual void Clear(ClearFlags clearFlags) => Intercept("Clear", Underlying.Clear, clearFlags);
        public virtual ITexture2D CreateTexture() => (OpenAbility.Graphik.ITexture2D)Intercept("CreateTexture", Underlying.CreateTexture);
        public virtual IMesh CreateMesh() => (OpenAbility.Graphik.IMesh)Intercept("CreateMesh", Underlying.CreateMesh);
        public virtual IShader CreateShader() => (OpenAbility.Graphik.IShader)Intercept("CreateShader", Underlying.CreateShader);
        public virtual IRenderTexture CreateRenderTexture() => (OpenAbility.Graphik.IRenderTexture)Intercept("CreateRenderTexture", Underlying.CreateRenderTexture);
        public virtual void ResetTarget() => Intercept("ResetTarget", Underlying.ResetTarget);
        public virtual void SetMouseState(MouseState state) => Intercept("SetMouseState", Underlying.SetMouseState, state);
        public virtual IShaderObject CreateShaderObject(ShaderType type) => (OpenAbility.Graphik.IShaderObject)Intercept("CreateShaderObject", Underlying.CreateShaderObject, type);
        public virtual void SetFeature(Feature feature, bool enabled) => Intercept("SetFeature", Underlying.SetFeature, feature, enabled);
        public virtual void SetCullMode(CullFace cullFace) => Intercept("SetCullMode", Underlying.SetCullMode, cullFace);
        public virtual void SetTexturePixelAlignment(int alignment) => Intercept("SetTexturePixelAlignment", Underlying.SetTexturePixelAlignment, alignment);
        public virtual void SetWindowTitle(System.String title) => Intercept("SetWindowTitle", Underlying.SetWindowTitle, title);
        public virtual void SetScissorArea(int x, int y, int width, int height) => Intercept("SetScissorArea", Underlying.SetScissorArea, x, y, width, height);
        public virtual OpenAbility.Graphik.ITexture2D FromNative(uint handle) => (OpenAbility.Graphik.ITexture2D)Intercept("FromNative", Underlying.FromNative, handle);
        public virtual void SetBlending(BlendMode blendMode) => Intercept("SetBlending", Underlying.SetBlending, blendMode);
        public virtual void SetBlendFunction(BlendFactor a, OpenAbility.Graphik.BlendFactor b) => Intercept("SetBlendFunction", Underlying.SetBlendFunction, a, b);
        public virtual void SetDepthFunction(DepthFunction depthFunction) => Intercept("SetDepthFunction", Underlying.SetDepthFunction, depthFunction);
        public virtual IController GetController(int controllerID) => (OpenAbility.Graphik.IController)Intercept("GetController", Underlying.GetController, controllerID);
        public virtual void UnbindTextures() => Intercept("UnbindTextures", Underlying.UnbindTextures);
        public virtual Vector2 ContentScale() => (System.Numerics.Vector2)Intercept("ContentScale", Underlying.ContentScale);
        public virtual void LineWidth(float width) => Intercept("LineWidth", Underlying.LineWidth, width);
        public virtual void PointSize(float size) => Intercept("PointSize", Underlying.PointSize, size);
        public virtual IShaderBuffer CreateShaderBuffer() => (OpenAbility.Graphik.IShaderBuffer)Intercept("CreateShaderBuffer", Underlying.CreateShaderBuffer);
        public virtual ICubemapTexture CreateCubemap() => (OpenAbility.Graphik.ICubemapTexture)Intercept("CreateCubemap", Underlying.CreateCubemap);
        public virtual OpenAbility.Graphik.IRenderTexture GetBoundTarget() => (OpenAbility.Graphik.IRenderTexture)Intercept("GetBoundTarget", Underlying.GetBoundTarget);
        public virtual IShaderCompiler GetCompiler() => (OpenAbility.Graphik.IShaderCompiler)Intercept("GetCompiler", Underlying.GetCompiler);
        public virtual System.String[] GetSupportedLanguages() => (System.String[])Intercept("GetSupportedLanguages", Underlying.GetSupportedLanguages);
        public virtual bool IsExtensionSupported(System.String extension) => (bool)Intercept("IsExtensionSupported", Underlying.IsExtensionSupported, extension);
        public virtual void SetWindowIcons(WindowIcon[] icons) => Intercept("SetWindowIcons", Underlying.SetWindowIcons, icons);
        public virtual void SetIncludeCallback(IncludeCallback includeCallback) => Intercept("SetIncludeCallback", Underlying.SetIncludeCallback, includeCallback);
        public virtual Object InvokeLibraryFunction(System.String function, System.Object[] parameters) => (System.Object)Intercept("InvokeLibraryFunction", Underlying.InvokeLibraryFunction, function, parameters);
        public virtual System.Object GetLibraryValue(System.String value) => (System.Object)Intercept("GetLibraryValue", Underlying.GetLibraryValue, value);
        public virtual System.String GetLibraryIdentifier() => (System.String)Intercept("GetLibraryIdentifier", Underlying.GetLibraryIdentifier);
        public virtual void LogMarker(System.String marker) => Intercept("LogMarker", Underlying.LogMarker, marker);
        public void ClearColour(float r, float g, float b, float a) => Intercept("ClearColour", Underlying.ClearColour, r, g, b, a);
}
