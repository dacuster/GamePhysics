using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

using System.Runtime.InteropServices;

// ForceGeneratorDLL
public class Game_Physics_DLL
{
    [DllImport("Game_Physics_DLL")]
    public static extern int InitFoo(int newFoo = 0);

    [DllImport("Game_Physics_DLL")]
    public static extern int DoFoo(int bar = 0);

    [DllImport("Game_Physics_DLL")]
    public static extern int TermFoo();
}
