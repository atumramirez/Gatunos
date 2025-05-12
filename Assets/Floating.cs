using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float floatStrength = 10f;  
    public float floatSpeed = 1f;      

    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.localPosition = originalPos + new Vector3(0, offset, 0);
    }
}