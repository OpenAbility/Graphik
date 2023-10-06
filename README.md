# Graphik
*Grafik + Graphite = Graphik*  

Graphik is a simple cross-platform rendering API designed for work in an internal
game engine, alongside future porting of said engine.

The API is designed to simply abstract away stuff, and be similar to OpenGL, without
being bound to OpenGL-only code.

This means implementing an API for e.g DirectX can be a little odd at times.

# Graphik.Audio
Graphik.Audio is a system to play back sound through whatever backend the system may support.  

The library aims to provide an OOP approach to an OpenAL-like API.

## Heads-up
Currently the only implementation of Graphik.Audio is for OpenAL, and it uses `AL_EXT_float32`, which only appears to be available if you have OpenAL Soft installed.  
This means that by default, it doesn't work on windows.
