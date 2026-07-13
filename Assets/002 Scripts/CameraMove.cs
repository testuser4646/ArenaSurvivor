using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform target;

    Vector3 velocity;
    Vector3 targetPos;

    void Start()
    {
        velocity = Vector3.zero;
    }

    void Update()
    {
        targetPos = new Vector3(target.position.x, target.position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.2f);
    }
}
