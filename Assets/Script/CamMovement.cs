using UnityEngine;
using UnityEngine.InputSystem;

public class CamMovement : MonoBehaviour
{
    public Transform cam;
    public float speed = 5f;
   

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.rightArrowKey.isPressed)
        {
            cam.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }

         if(Keyboard.current.leftArrowKey.isPressed)
        {
            cam.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        }
    }
}
