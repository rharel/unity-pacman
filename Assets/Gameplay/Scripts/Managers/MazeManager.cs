using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Handles the instantiation of the maze itself. This involves the maze paths, coins 
/// and powerups.
/// </summary>
public class MazeManager : Singleton<MazeManager>
{
    public GameObject pathPrefab;
    public Color pathColor = Color.white;
    public GameObject coinPrefab;
    public float coinSpacing = 1;
    public GameObject powerupPrefab;
    public CircleCollider2D pacmanCollider;

    public static RaycastHit2D RayCast(
        Vector2 currentPosition, Waypoint target, 
        Vector2 direction, float angleLimit, 
        LayerMask layerMask)
    {
        if (target == null)
            throw new ArgumentNullException("target");

        RaycastHit2D hit = new RaycastHit2D();
        while (target != null && hit.collider == null)
        {
            Vector2 targetPosition = target.transform.position;
            Vector2 targetDirection = targetPosition - currentPosition;
            float castDistance = targetDirection.magnitude;
            hit = Physics2D.Raycast(
                currentPosition, targetDirection, castDistance, layerMask);

            currentPosition = targetPosition;
            target = target.NeighbourAt(direction, angleLimit);
        }

        return hit;
    }

    private GameObject mazeHost;
    private GameObject pathHost;
    private GameObject coinHost;
    private GameObject powerupHost;
    
    void Awake()
    {
        // These game objects will host the dynamically generated maze.
        mazeHost = new GameObject("Maze (Dynamic)");
        pathHost = new GameObject("Paths (Dynamic)");
        coinHost = new GameObject("Coins (Dynamic)");
        powerupHost = new GameObject("Powerups (Dynamic)");

        pathHost.transform.parent = mazeHost.transform;
        coinHost.transform.parent = mazeHost.transform;
        powerupHost.transform.parent = mazeHost.transform;
    }

    public void InstantiatePath(Vector3 p, Quaternion rotation)
    {
        float size = 2 * pacmanCollider.radius;

        GameObject path = Instantiate(pathPrefab);
        path.transform.parent = pathHost.transform;
        path.transform.rotation = rotation;
        path.transform.localScale = new Vector3(size, size, 1);
        path.transform.position = p;
        path.GetComponent<SpriteRenderer>().color = pathColor;
    }

    public void InstantiatePath(Vector3 a, Vector3 b, bool includeEndpoints = false)
    {
        Vector3 ab = b - a;
        float pathLength = ab.magnitude;

        if (includeEndpoints)
            pathLength += 2 * pacmanCollider.radius;
        else
            pathLength -= 2 * pacmanCollider.radius;
        
        GameObject path = Instantiate(pathPrefab);
        path.transform.parent = pathHost.transform;
        path.transform.rotation = Quaternion.FromToRotation(Vector3.right, ab.normalized);
        path.transform.localScale = new Vector3(pathLength, 2 * pacmanCollider.radius, 1);
        path.transform.position = a + ab / 2;
        path.GetComponent<SpriteRenderer>().color = pathColor;
    }

    public void InstantiateCoinTrail(Vector3 a, Vector3 b)
    {
        if (coinSpacing <= 0)
            return;

        Vector3 ab = b - a;
        int nCoins = Mathf.FloorToInt(ab.magnitude / coinSpacing);
        Vector3 space = ab.normalized * ab.magnitude / nCoins;
        Vector3 position = a + space;

        for (int i = 0; i < nCoins - 1; ++i)
        {
            InstantiateCoin(position);
            position += space;
        }
    }

    public void InstantiateCoin(Vector3 position)
    {
        var coin =
            Instantiate(coinPrefab, position, Quaternion.identity) as GameObject;
        coin.transform.parent = coinHost.transform;
    }

    public void InstantiatePowerup(Vector3 position)
    {
        var powerup =
            Instantiate(powerupPrefab, position, Quaternion.identity) as GameObject;
        powerup.transform.parent = powerupHost.transform;
    }
}
