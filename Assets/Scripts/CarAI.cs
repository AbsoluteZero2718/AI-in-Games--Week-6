using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using Unity.VisualScripting;


public class CarAI : MonoBehaviour
{
    private NavMeshAgent CarAgent;

    [Header("Route")]
    public Transform[] startRoute;
    public int currentWaypointIndex = 0;

    public Transform[] currentRoute;
    public Transform[] route_1;
    public Transform[] route_2;
    public Transform[] route_3;

    bool routeChosen = false;


    [Header("Traffic Light")]
    //Traffic Rules
    public StoplightScript trafficLight;
    public Transform stopPoint;
    public float stopDistance = 2.0f;

    [Header("Speed")]
    //SPEED
    public float normalSpeed;
    public float slowSpeed;
    public float dist;

    [Header("Raycast Car Detection0")]
    public Transform rayOrigin;
    public float rayDistance = 4f;
    public LayerMask carLayer;
    public bool carInFront = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        CarAgent = GetComponent<NavMeshAgent>();
        CarAgent.speed = normalSpeed;

        currentRoute = startRoute;
        currentWaypointIndex = 0;
        MoveToCurrentWaypoint(); // once the game starts, the vehicle starts to move
    }

    // Update is called once per frame
    void Update()
    {
        CheckCarInFront();

        if(carInFront)
        {
            CarAgent.isStopped = true;
            return;
        }
        FollowTrafficLight();

        if (CarAgent.pathPending)
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
        if(trafficLight == null || stopPoint == null)
        {
            return;
        }

        dist = Vector3.Distance(transform.position, stopPoint.position);
        
        if(!trafficLight.isGreen && dist <= stopDistance) // Red light
        {
            CarAgent.isStopped = true;
        }
        else
        {
            CarAgent.isStopped = false;
            CarAgent.speed = normalSpeed;
        }

    }

    private void MoveToCurrentWaypoint()
    {
        if(currentRoute.Length == 0 || currentRoute == null)
        {
            CarAgent.SetDestination(currentRoute[currentWaypointIndex].position);
        }
        //CarAgent.SetDestination(destinations[currentWaypointIndex].position);
    }

    private void GoToNextWaypoint()
    {
        currentWaypointIndex++; // tells the vehicle to go to next waypoint
        if(currentWaypointIndex >= currentRoute.Length)
        {
            if(routeChosen)
            {
                ChooseRandomRoute();
            }
            else
            {
                currentWaypointIndex = 0;
                MoveToCurrentWaypoint();
            }
        }
        MoveToCurrentWaypoint();
       
    }

    public void ChooseRandomRoute()
    {
        routeChosen = true;
        int randomRoute = Random.Range(0, 2);
        if(randomRoute == 0)
        {
            currentRoute = route_1;
        }
        else if(randomRoute == 1)
        {
            currentRoute = route_2;
        }
        else
        {
            currentRoute = route_3;
        }
        currentWaypointIndex = 0;
        MoveToCurrentWaypoint();
    }

    private void CheckCarInFront()
    {
        carInFront = false;
        if(rayOrigin == null)
        {
            return;
        }

        //Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 origin = rayOrigin.position;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * rayDistance, Color.green);

        RaycastHit hit;

        if(Physics.Raycast(origin, direction, out hit, rayDistance, carLayer))
        {
            carInFront = true;
        }
    }
}
