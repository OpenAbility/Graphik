# Graphik
*Grafik + Graphite = Graphik*  

Graphik is a simple cross-platform rendering API designed for work in an internal
game engine, alongside future porting of said engine.

The API is designed to simply abstract away stuff, and be similar to OpenGL, without
being bound to OpenGL-only code.

This means implementing an API for e.g DirectX can be a little odd at times.

## Shaders
Shaders are written in whatever language is native. Recommended method is to have a system
to quickly substitute shaders whenever needed
