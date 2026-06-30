using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using Unity.VisualScripting;

[RequireComponent(typeof(NavMeshAgent))]
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

    //[Header("Lifecycle")]
    //public bool startOnAwake = false;

    [Header("Traffic Light")]
    //Traffic Rules
    public StoplightScript trafficLight;
    public Transform stopPoint;
    public float stopDistance = 2f;

    [Header("Speed")]
    public float normalSpeed = 5f;
    public float slowSpeed = 2f;
    public float dist;

    [Header("Raycast Car Detection")]
    public Transform rayOrigin;
    public float rayDistance = 4f;
    public LayerMask carLayer;
    public bool carInFront = false;

    void Start()
    {
    
        CarAgent = GetComponent<NavMeshAgent>();
        CarAgent.speed = normalSpeed;

        //if (startOnAwake)
        //    BeginDriving();
    }

    //public void BeginDriving()
    //{
    //    currentRoute = startRoute;
    //    currentWaypointIndex = 0;
    //    routeChosen = true;
    //    MoveToCurrentWaypoint();
    //}

    // Update is called once per frame
    void Update()
    {
        CheckCarInFront();

        if (carInFront)
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
        if (trafficLight == null || stopPoint == null)
        { 
            return;
        }

        dist = Vector3.Distance(transform.position, stopPoint.position);

        if (!trafficLight.isGreen && dist <= stopDistance)
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
        if (currentRoute == null || currentRoute.Length == 0)
            return;

        CarAgent.SetDestination(currentRoute[currentWaypointIndex].position);
    }
        //CarAgent.SetDestination(destinations[currentWaypointIndex].position);

    private void GoToNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= currentRoute.Length)
        {
            if (!routeChosen)
            {
                ChooseRandomRoute();
                return;
            }

            Destroy(gameObject);
            return;
        }

        MoveToCurrentWaypoint();
    }

    public void ChooseRandomRoute()
    {
        routeChosen = true;
        int randomRoute = Random.Range(0, 3);
        if (randomRoute == 0)
            currentRoute = route_1;
        else if (randomRoute == 1)
            currentRoute = route_2;
        else
            currentRoute = route_3;

        currentWaypointIndex = 0;
        MoveToCurrentWaypoint();
    }

    private void CheckCarInFront()
    {
        carInFront = false;
        if (rayOrigin == null)
            return;

        Vector3 origin = rayOrigin.position;
        Vector3 direction = transform.forward;

        Debug.DrawRay(origin, direction * rayDistance, Color.green);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayDistance, carLayer))
            carInFront = true;
    }
}
