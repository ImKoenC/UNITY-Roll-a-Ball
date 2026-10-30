using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.InputSystem; // 1. Add this namespace at the top

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position; 
    }

    void Update()
    {
        // 2. Use the new Input System syntax to check for the R key
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f; 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;  
    }
}
