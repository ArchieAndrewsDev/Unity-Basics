using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    private CharacterController cc;

    public Transform headTransform;
    public float moveSpeed = 5;
    public float maxPitch = 85, minPitch = -85;
    public float jumpSpeed = 50;

    private Vector3 yaw;
    private Vector3 pitch;
    private float yVelocity = 0;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        pitch = headTransform.eulerAngles;
        yaw = transform.eulerAngles;
    }

    public void MoveCharacter(Vector3 dir, bool jump)
    {
        dir = transform.TransformDirection(dir);
        dir *= moveSpeed;

        if (!cc.isGrounded)
            yVelocity = Mathf.Clamp(yVelocity + Physics.gravity.y * Time.deltaTime, Physics.gravity.y, Mathf.Infinity);

        if (jump && cc.isGrounded)
            yVelocity = jumpSpeed;

        if ((cc.collisionFlags & CollisionFlags.Above) != 0)
            yVelocity = Mathf.Clamp(yVelocity, Physics.gravity.y, 0);

        dir.y = yVelocity;

        cc.Move(dir *= Time.deltaTime);
    }

    public void RotateCharacter(Vector3 dir)
    {
        yaw.x = transform.eulerAngles.x;
        yaw.y = transform.eulerAngles.y + dir.y;
        yaw.z = transform.eulerAngles.z;

        transform.rotation = Quaternion.Euler(yaw);

        pitch.x = Mathf.Clamp(pitch.x + dir.x, minPitch, maxPitch);
        pitch.y = headTransform.eulerAngles.y;
        pitch.z = headTransform.eulerAngles.z;

        headTransform.rotation = Quaternion.Euler(pitch);
    }
}
