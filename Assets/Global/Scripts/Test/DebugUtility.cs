using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public static class DebugUtility 
{
    public class AssertionFailureException : Exception
    {
        public AssertionFailureException() : base() { }
        public AssertionFailureException(string message) : base(message) { }
        public AssertionFailureException(string message, Exception inner)
            : base(message, inner) { }
    }

    public static void Assert(bool condition, string message = "")
    {
        if (!Debug.isDebugBuild)
            return;

        else if (!condition)
            throw new AssertionFailureException(message);
    }

    public static void Log<T>(IEnumerable<T> collection, string message = "")
    {
        Debug.Log(message + String.Join(", ", collection.Select(x => x.ToString()).ToArray()));
    }
}
