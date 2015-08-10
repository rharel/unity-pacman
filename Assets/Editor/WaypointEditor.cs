using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The waypoint editor extension allows you to design your own pacman levels with ease.
/// When selecting a waypoint, use either the hotkeys or the GUI shown in the editor to easily
/// extend, connect, disconnect or mirror waypoints.
/// </summary>
[CustomEditor (typeof(Waypoint))]
[CanEditMultipleObjects]
public class WaypointEditor : Editor 
{
    private static readonly KeyCode KEY_EXTEND = KeyCode.V;
    private static readonly KeyCode KEY_CONNECT = KeyCode.C;
    private static readonly KeyCode KEY_DISCONNCT = KeyCode.X;
    private static readonly KeyCode KEY_SUBDIVIDE = KeyCode.Z;
    private static readonly KeyCode KEY_MIRROR = KeyCode.M;

    private static readonly KeyCode[] ACTION_KEYS =
        new KeyCode[] { KEY_CONNECT, KEY_DISCONNCT, KEY_EXTEND, KEY_MIRROR, KEY_SUBDIVIDE };

    private static readonly string EXTEND_BUTTON_LABEL = "Extend";
    private static readonly string CONNECT_BUTTON_LABEL = "Connect";
    private static readonly string DISCONNECT_BUTTON_LABEL = "Disconnect";
    private static readonly string SUBDIVIDE_BUTTON_LABEL = "Subdivide";
    private static readonly string MIRROR_BUTTON_LABEL = "MirrorY";

    /// <summary>
    /// Mirrors a vector 'v' through a line 'AB'.
    /// </summary>
    /// <param name="v">Vector to mirror.</param>
    /// <param name="p">Point A of the reflecting-line.</param>
    /// <param name="q">Point B of the reflecting-line.</param>
    /// <returns></returns>
    private static Vector2 Mirror(Vector2 v, Vector2 p, Vector2 q)
    {
        float A = q.y - p.y;
        float B = -(q.x - p.x);
        float C = -A * p.x - B * p.y;
        float M = Mathf.Sqrt(A * A + B * B);
        float An = A / M;
        float Bn = B / M;
        float Cn = C / M;
        float D = An * v.x + Bn * v.y + Cn;

        return new Vector2(v.x - 2 * An * D, v.y - 2 * Bn * D);
    }

    private Waypoint active;
    private GameObject prefab;

    void OnEnable()
    {
        active = (Waypoint)target;
        prefab = (GameObject)PrefabUtility.GetPrefabParent(active.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button(EXTEND_BUTTON_LABEL))
            Extend(active.transform.position + Vector3.right);

        if (GUILayout.Button(MIRROR_BUTTON_LABEL))
            MirrorY();

        EditorGUI.BeginDisabledGroup(targets.Length != 2);

        if (GUILayout.Button(CONNECT_BUTTON_LABEL))
            Connect();

        if (GUILayout.Button(DISCONNECT_BUTTON_LABEL))
            Disconnect();

        if (GUILayout.Button(SUBDIVIDE_BUTTON_LABEL))
            Subdivide();

        EditorGUI.EndDisabledGroup();
    }

    void OnSceneGUI()
    {
        Event e = Event.current;

        if (e.type == EventType.KeyUp)
        {
            if (e.keyCode == KEY_EXTEND)
            {
                Ray ray = Camera.current.ScreenPointToRay(
                    new Vector3(e.mousePosition.x,
                               -e.mousePosition.y + Camera.current.pixelHeight));
                Vector2 spawnPosition =
                    new Vector2(Mathf.Round(ray.origin.x),
                                Mathf.Round(ray.origin.y));
                Extend(spawnPosition);
            }
            else if (e.keyCode == KEY_CONNECT)
                Connect();
            else if (e.keyCode == KEY_DISCONNCT)
                Disconnect();
            else if (e.keyCode == KEY_SUBDIVIDE)
                Subdivide();
            else if (e.keyCode == KEY_MIRROR)
                MirrorY();
        }

        if (ACTION_KEYS.Contains(e.keyCode))
            e.Use();
    }

    /// <summary>
    /// Create a new waypoint at a given position and connect it to the waypoint that is 
    /// currently active.
    /// </summary>
    private void Extend(Vector2 spawnPosition)
    {
        DebugUtility.Assert(prefab != null);

        var newInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        newInstance.transform.parent = active.gameObject.transform.parent;
        newInstance.transform.position = spawnPosition;
        var newWaypoint = newInstance.GetComponent<Waypoint>();

        active.Connect(newWaypoint);

        EditorUtility.SetDirty(active);
        EditorUtility.SetDirty(newWaypoint);

        Selection.activeObject = newInstance;
        SceneView.RepaintAll();
    }

    /// <summary>
    /// Iff exactly two waypoints are selected, connects them.
    /// </summary>
    private void Connect()
    {
        if (Selection.gameObjects.Length != 2)
            return;
        
        var a = Selection.gameObjects[0].GetComponent<Waypoint>();
        var b = Selection.gameObjects[1].GetComponent<Waypoint>();

        if (a == null || b == null)
            return;

        a.Connect(b);

        EditorUtility.SetDirty(a);
        EditorUtility.SetDirty(b);

        SceneView.RepaintAll();
    }

    /// <summary>
    /// Iff exactly two waypoints are selected, disconnects them.
    /// </summary>
    private void Disconnect()
    {
        if (Selection.gameObjects.Length != 2)
            return;

        var a = Selection.gameObjects[0].GetComponent<Waypoint>();
        var b = Selection.gameObjects[1].GetComponent<Waypoint>();

        if (a == null || b == null)
            return;

        a.Disconnect(b);

        EditorUtility.SetDirty(a);
        EditorUtility.SetDirty(b);

        SceneView.RepaintAll();
    }

    /// <summary>
    /// Iff exactly two waypoints A and B are selected, this disconnects them, creates a 
    /// new waypoint C in the middle, and finally connects A->C and C->B.
    /// </summary>
    private void Subdivide()
    {
        DebugUtility.Assert(prefab != null);
        
        if (Selection.gameObjects.Length != 2)
            return;

        var a = Selection.gameObjects[0].GetComponent<Waypoint>();
        var b = Selection.gameObjects[1].GetComponent<Waypoint>();

        if (a == null || b == null)
            return;

        var newInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        newInstance.transform.parent = active.gameObject.transform.parent;
        newInstance.transform.position = a.Position + (b.Position - a.Position) / 2;
        var newWaypoint = newInstance.GetComponent<Waypoint>();

        a.Disconnect(b);
        a.Connect(newWaypoint);
        b.Connect(newWaypoint);

        EditorUtility.SetDirty(a);
        EditorUtility.SetDirty(b);
        EditorUtility.SetDirty(newWaypoint);

        SceneView.RepaintAll();
    }

    /// <summary>
    /// Mirrors selected group of waypoints along the Y-axis. This helps design symmetric levels
    /// by hand.
    /// </summary>
    private void MirrorY()
    {
        if (!prefab)
            return;

        List<GameObject> reflections = new List<GameObject>();
        var reflectionOf = new Dictionary<Waypoint, Waypoint>();

        foreach (GameObject selected in Selection.gameObjects)
        {
            if (!selected.GetComponent<Waypoint>())
                continue;

            var mirrored = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            mirrored.transform.parent = selected.transform.parent;
            mirrored.transform.position =
                new Vector2(-selected.transform.position.x,
                             selected.transform.position.y);

            reflections.Add(mirrored);
            reflectionOf[selected.GetComponent<Waypoint>()] = mirrored.GetComponent<Waypoint>();
        }

        foreach (Waypoint original in reflectionOf.Keys)
        {
            Waypoint mirrored = reflectionOf[original];

            foreach (var neighbour in original.ValidNeighbours)
            {
                if (reflectionOf.ContainsKey(neighbour))
                    mirrored.Connect(reflectionOf[neighbour]);
            }
        }

        foreach (var reflection in reflectionOf.Values)
            EditorUtility.SetDirty(reflection);
    }
}
