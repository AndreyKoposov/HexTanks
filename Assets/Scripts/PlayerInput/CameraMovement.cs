using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * Time.deltaTime;

        Camera.main.transform.Translate(new(x, 0, z));
    }
}
