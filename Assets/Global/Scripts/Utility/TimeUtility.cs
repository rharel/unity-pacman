using UnityEngine;
using System;
using System.Collections.Generic;

public static class TimeUtility
{
    public static bool IsTimeout(float startTime, float currentTime, float limit)
    {
        float duration = currentTime - startTime;
        return duration > limit;
    }
}
