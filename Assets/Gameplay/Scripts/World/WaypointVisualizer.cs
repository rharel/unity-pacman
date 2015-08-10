using UnityEngine;
using System.Collections;


public class WaypointVisualizer : MonoBehaviour 
{
    public Color waypointColor = Color.cyan;
    public Color pathColor = Color.green;

    private static readonly float SPHERE_GIZMO_RADIUS = 0.2f;

    private static readonly float ARROW_GIZMO_SPAN = 45;
    private static readonly float ARROW_GIZMO_LENGTH = SPHERE_GIZMO_RADIUS + 0.1f;

    private static readonly Quaternion ARROW_GIZMO_CW_ROTATION =
        Quaternion.AngleAxis(ARROW_GIZMO_SPAN, Vector3.back);
    private static readonly Quaternion ARROW_GIZMO_CCW_ROTATION =
        Quaternion.AngleAxis(ARROW_GIZMO_SPAN, Vector3.forward);

    void OnDrawGizmos()
    {
        DrawSelfGizmo();
        DrawNeighbourPointerGizmos();
    }   

    void DrawSelfGizmo()
    {
        Gizmos.color = waypointColor;
        Gizmos.DrawWireSphere(transform.position, SPHERE_GIZMO_RADIUS);
    }

    void DrawNeighbourPointerGizmos()
    {
        Gizmos.color = pathColor;

        var waypoint = GetComponent<Waypoint>();
        foreach (Waypoint neighbour in waypoint.ValidNeighbours)
            DrawArrowGizmo(this.transform.position, neighbour.transform.position);
    }

    void DrawArrowGizmo(Vector3 source, Vector3 target)
    {
        Vector3 wingtip = (source - target).normalized * ARROW_GIZMO_LENGTH;
        Vector3 wing1 = target + ARROW_GIZMO_CW_ROTATION * wingtip;
        Vector3 wing2 = target + ARROW_GIZMO_CCW_ROTATION * wingtip;

        Gizmos.DrawLine(wing1, target);
        Gizmos.DrawLine(wing2, target);
        Gizmos.DrawLine(source, target);
    }
}
