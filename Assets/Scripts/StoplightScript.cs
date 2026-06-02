using UnityEngine;

public class StoplightScript : MonoBehaviour
{
    public bool isGreen = true;
    public float greenTime = 5.0f;
    public float redTime = 5.0f;

    public float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(isGreen && timer >= greenTime)
        {
            isGreen = false;
            timer = 0;
            Debug.Log("Red Light: ON");
        }

        else if(!isGreen && timer >= redTime)
        {
            isGreen = true;
            timer = 0;
            Debug.Log("Green Light: ON");
        }
    }
}
