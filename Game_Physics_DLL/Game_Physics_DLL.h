#ifndef GAME_PHYSICS_DLL_H
#define GAME_PHYSICS_DLL_H

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else	// !__cplusplus

#endif	// __cplusplus

GAME_PHYSICS_DLL_SYMBOL int InitFoo(int newFoo);
GAME_PHYSICS_DLL_SYMBOL int DoFoo(int bar);
GAME_PHYSICS_DLL_SYMBOL int TermFoo();


#ifdef __cplusplus
}
#else	// !__cplusplus

#endif	// __cplusplus


#endif	// !GAME_PHYSICS_DLL_H
