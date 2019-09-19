using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inertia
{
    public static Vector2 Cuboid(float _particleMass, float _gravitationalConstant, Vector2 _worldUp)
    {
        // f = mg
        return (_particleMass * _gravitationalConstant * _worldUp);
    }
}
