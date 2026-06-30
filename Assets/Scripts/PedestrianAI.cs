using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PedestrianAI : MonoBehaviour
{
    public StoplightScript trafficLight;
    public Transform[] crosswalkRoute;

    private NavMeshAgent agent;
    private int waypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToCurrentWaypoint();
    }

    void Update()
    {
        if (trafficLight == null)
            return;

        // Opposite of cars: pedestrians cross on red, wait on green.
        agent.isStopped = trafficLight.isGreen;

        if (agent.pathPending || agent.isStopped)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
            GoToNextWaypoint();
    }

    void MoveToCurrentWaypoint()
    {
        if (crosswalkRoute == null || crosswalkRoute.Length == 0)
            return;

        agent.SetDestination(crosswalkRoute[waypointIndex].position);
    }

    void GoToNextWaypoint()
    {
        waypointIndex = (waypointIndex + 1) % crosswalkRoute.Length;
        MoveToCurrentWaypoint();
    }
}
