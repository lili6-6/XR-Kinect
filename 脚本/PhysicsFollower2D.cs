using UnityEngine;

public class PhysicsFollower2D : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform followTarget;
    public Vector2 offset = Vector2.zero;
    public float followStrength = 40f;
    public float rotationStrength = 10f;

    private void Awake()
    {
        transform.position = followTarget.position;
        transform.rotation = followTarget.rotation;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void FixedUpdate()
    {
        Vector2 targetPos = (Vector2)followTarget.position + offset;
        Vector2 force = (targetPos - rb.position) * followStrength;
        rb.velocity = force;

        float targetAngle = followTarget.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(rb.rotation, targetAngle);
        rb.angularVelocity = deltaAngle * rotationStrength;
    }
}
