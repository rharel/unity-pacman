using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class RandomUtility
{
    /// <summary>
    /// Chooses a random element.
    /// </summary>
    public static T Choose<T>(IEnumerable<T> collection)
    {
        return collection.ElementAt(Random.Range(0, collection.Count()));
    }
}