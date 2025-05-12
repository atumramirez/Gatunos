using UnityEngine;
using TMPro;

public class FlashingText : MonoBehaviour
{
    public float flashSpeed = 2f;

    private TextMeshProUGUI tmp;
    private Color originalColor;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        originalColor = tmp.color;
    }

    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f; 
        tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}