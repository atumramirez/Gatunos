using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchToLoadScene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    void Update()
    {
        // For touch devices
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            LoadScene();
        }

        // For mouse input (useful for testing in the editor)
        if (Input.GetMouseButtonDown(0))
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene name specified in the inspector.");
        }
    }
}