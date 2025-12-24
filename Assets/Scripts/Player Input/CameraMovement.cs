using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        Camera.main.transform.Translate(new(x, 0, z), Space.World);
    }
}
