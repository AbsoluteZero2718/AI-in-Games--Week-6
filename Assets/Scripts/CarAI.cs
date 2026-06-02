using UnityEngine;
using Unity.AI;
using UnityEngine.AI;


public class CarAI : MonoBehaviour
{
    public Transform[] destinations; 
    private NavMeshAgent CarAgent;

    public int currentWaypointIndex = 0;

    //Traffic Rules
    public StoplightScript trafficLight;
    public Transform stopPoint;
    public float stopDistance = 2.0f;

    //SPEED
    public float normalSpeed;
    public float slowSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        CarAgent = GetComponent<NavMeshAgent>();
        CarAgent.speed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTrafficLight();

        if(CarAgent.pathPending)
        {
            return;
        }

        if (CarAgent.remainingDistance <= CarAgent.stoppingDistance)
        {
            GoToNextWaypoint();
        }
    }

    private void FollowTrafficLight()
    {
        float distanceToStopPoint = Vector3.Distance(transform.position, stopPoint.position);

        if(!trafficLight.isGreen && distanceToStopPoint <= stopDistance * 2)
        {
            CarAgent.isStopped = true;
            CarAgent.speed = 0;
        }
        else if(!trafficLight.isGreen && distanceToStopPoint <= stopDistance * 4)
        {
            CarAgent.isStopped = false;
            CarAgent.speed = slowSpeed;
        }
        else
        {
            CarAgent.isStopped = false;
            CarAgent.speed = normalSpeed;
        }

    }

    private void MoveToCurrentWaypoint()
    {
        CarAgent.SetDestination(destinations[currentWaypointIndex].position);
    }

    private void GoToNextWaypoint()
    {
        currentWaypointIndex++; // tells the vehicle to go to next waypoint
        if(currentWaypointIndex >= destinations.Length)
        {
            currentWaypointIndex = 0;
            
        }
        MoveToCurrentWaypoint();
    }
}
