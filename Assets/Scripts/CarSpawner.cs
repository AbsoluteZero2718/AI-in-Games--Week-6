using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarSpawner : MonoBehaviour
{
    [Header("Car Spawning")]
    public GameObject carTemplate;
    public Transform spawnPoint;
    public Transform[] spawnRoute;
    public StoplightScript trafficLight;
    public Transform stopPoint;

    [Header("UI")]
    public Button spawnButton;


    void Start()
    {
        ResolveReferences();
        HideTemplateCars();
        
    }

    public void SpawnCar()
    {
        if (carTemplate == null || spawnPoint == null)
        {
            Debug.LogWarning("CarSpawner is missing a car template or spawn point.");
            return;
        }

        GameObject car = Instantiate(carTemplate, spawnPoint.position, spawnPoint.rotation);
        car.SetActive(true);

        CarAI carAI = car.GetComponent<CarAI>();
        if (carAI == null)
            return;

        if (trafficLight != null)
            carAI.trafficLight = trafficLight;
        if (stopPoint != null)
            carAI.stopPoint = stopPoint;
        if (spawnRoute != null && spawnRoute.Length > 0)
            carAI.startRoute = spawnRoute;

       // carAI.BeginDriving();
    }

    void ResolveReferences()
    {
        if (trafficLight == null)
            trafficLight = FindFirstObjectByType<StoplightScript>();

        if (stopPoint == null)
        {
            GameObject intersection = GameObject.Find("Intersection");
            if (intersection != null)
                stopPoint = intersection.transform;
        }

        if (carTemplate == null)
            carTemplate = GameObject.Find("Bus");

        if (spawnPoint == null)
        {
            GameObject spawnObject = GameObject.Find("SpawnPoint");
            if (spawnObject != null)
            {
                spawnPoint = spawnObject.transform;
            }
            else
            {
                spawnObject = new GameObject("SpawnPoint");
                spawnObject.transform.SetPositionAndRotation(
                    new Vector3(-1.78f, 1.53f, 13.08f),
                    Quaternion.Euler(0f, 178.534f, 0f));
                spawnPoint = spawnObject.transform;
            }
        }

        if (spawnRoute == null || spawnRoute.Length == 0)
            spawnRoute = GetRouteWaypoints("Route 1");
    }

    Transform[] GetRouteWaypoints(string routeName)
    {
        GameObject route = GameObject.Find(routeName);
        if (route == null)
            return new Transform[0];

        Transform[] waypoints = new Transform[route.transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
            waypoints[i] = route.transform.GetChild(i);

        return waypoints;
    }

    void HideTemplateCars()
    {
        DisableIfPresent("Bus");
        DisableIfPresent("Bus (1)");
        DisableIfPresent("Bus (2)");
    }

    void DisableIfPresent(string objectName)
    {
        GameObject target = GameObject.Find(objectName);
        if (target != null)
            target.SetActive(false);
    }

   

 

   
}
