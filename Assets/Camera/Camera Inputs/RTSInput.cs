using UnityEngine;

namespace Basics.Camera
{
    [RequireComponent(typeof(RTSCameraMotor))]
    public class RTSInput : MonoBehaviour
    {
        private RTSCameraMotor motor;

        public KeyCode speedBoost = KeyCode.LeftShift;

        private float hor, ver;
        private float height = 0;

        private Vector3 movementDir;

        private void Awake()
        {
            motor = GetComponent<RTSCameraMotor>();
        }

        private void Update()
        {
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");

            movementDir.x = hor;
            movementDir.z = ver;

            float height = Input.GetAxis("Mouse ScrollWheel");

            if(height != 0)
                motor.AlterHeight(height);

            if (Input.GetKey(speedBoost))
                movementDir *= motor.GetSpeedBoostMultiplier();

            motor.Move(movementDir);
        }
    }
}
