#ifndef LIB_H
#define LIB_H

#ifdef GAME_PHYSICS_DLL_EXPORT

#define GAME_PHYSICS_DLL_SYMBOL __declspec(dllexport)

#else	// !GAME_PHYSICS_DLL_EXPORT

#ifdef GAME_PHYSICS_DLL_IMPORT

#define GAME_PHYSICS_DLL_SYMBOL __declspec(dllimport)

#else	//! GAME_PHYSICS_DLL_IMPORT

#define GAME_PHYSICS_DLL_SYMBOL

#endif	// GAME_PHYSICS_DLL_IMPORT

#endif	// GAME_PHYSICS_DLL_EXPORT


#endif	// !LIB_H