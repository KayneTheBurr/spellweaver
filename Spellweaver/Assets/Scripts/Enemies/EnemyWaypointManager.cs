using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyWaypointManager : MonoBehaviour
{
    public static EnemyWaypointManager instance;

    [Header("Waypoint List")]
    public List<Transform> waypoints = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        GetAllWaypoints();
    }
    private void GetAllWaypoints()
    {
        waypoints.Clear();
        Waypoint[] foundWaypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);

        foreach (Waypoint point in foundWaypoints)
        {
            waypoints.Add(point.transform);
        }
    }
    public Transform[] GetRandomWaypoints(int count)
    {
        if (waypoints.Count == 0) return null;

        List<Transform> selectedWaypoints = new List<Transform>(); //new list of waypoints that will be sent 

        while(selectedWaypoints.Count < count)
        {
            Transform randomWaypoint = waypoints[Random.Range(0, waypoints.Count)]; //pick a random point from waypoint list
            selectedWaypoints.Add(randomWaypoint);
            waypoints.Remove(randomWaypoint);
        }
        return selectedWaypoints.ToArray(); //convert to array 
    }
}
