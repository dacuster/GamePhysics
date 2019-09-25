using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inertia2D
{
    // Different types of shapes in this class.
    public enum Type
    {
        Disk,
        Ring,
        Rectangle,
        Square,
        Rod
    }

    // Disk shape.
    public static float Disk(float _particleMass, float _particleRadius)
    {
        /*
         *  I = 0.5 * m * r^2
         *  1. Square the radius.
         *  2. Multiply by the mass.
         *  3. Multiply by 1/2.
         */
        return 0.5f * _particleMass * _particleRadius * _particleRadius;
    }

    // Ring shape.
    public static float Ring(float _particleMass, float _particleInnerRadius, float _particleOuterRadius)
    {
        /*
         *  I = 0.5 * m * (ro^2 + ri^2)
         *  1. Square the inner and outer radii.
         *  2. Multiply by mass.
         *  3. Multiply by 1/2.
         */
        return 0.5f * _particleMass * (_particleOuterRadius * _particleOuterRadius + _particleInnerRadius * _particleInnerRadius);
    }

    // Rectangular shape.
    public static float Rectangle(float _particleMass, float _particleLength, float _particleWidth)
    {
        /*
         *  I = 1 / 12 * m * (l^2 + w^2)
         *  1. Square the length and width.
         *  2. Multiply by mass.
         *  3. Multiply by 1/12.
         */
        return 1.0f / 12.0f * _particleMass * (_particleLength * _particleLength + _particleWidth * _particleWidth);
    }

    // Square shape.
    public static float Square(float _particleMass, float _particleLength)
    {
        /*
         *  I = 1 / 12 * m * (l^2 * 2)
         *  1. Square the length.
         *  2. Multiply squared length by 2. (Same as rectangle, but all sides are the same.)
         *  3. Multiply by mass.
         *  4. Multiply by 1/12.
         */
        return 1.0f / 12.0f * _particleMass * (_particleLength * _particleLength * 2);
    }

    // Thin rod shape.
    public static float Rod(float _particleMass, float _particleLength)
    {
        /*
         *  I = 1 / 12 * m * l^2
         *  1. Square the length.
         *  2. Multiply by mass.
         *  3. Multiply by 1/12.
         */
        return 1.0f / 12.0f * _particleMass * _particleLength * _particleLength;
    }
}
