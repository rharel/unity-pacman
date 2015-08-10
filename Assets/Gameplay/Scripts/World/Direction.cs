using UnityEngine;
using System.Collections;

public enum Direction : int
{
    None = 0,
    Left = 1, Right = -1,
    Up = 2, Down = -2
}

public static class DirectionExtensions
{
    public static Direction GetOpposite(this Direction dir)
    {
        return (Direction)(-((int)dir));
    }
}

