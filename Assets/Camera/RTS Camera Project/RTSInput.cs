using UnityEngine;

namespace Basics.Camera.RTS
{
    [RequireComponent(typeof(RTSCameraMotor))]
    public class RTSInput : MonoBehaviour
    {
        private RTSCameraMotor motor;

        public KeyCode speedBoost = KeyCode.LeftShift;

        private float hor, ver;
        private float height = 0;

        private Vector3 movementDir, screenDir;

        private void Awake()
        {
            motor = GetComponent<RTSCameraMotor>();
        }

        private void Update()
        {
            KeyBoardCtrl();
            MouseCtrl();
        }

        private void MouseCtrl()
        {
            if (Input.mousePosition.x >= Screen.width - 1)
                screenDir.x = 1;
            else if (Input.mousePosition.x <= 1)
                screenDir.x = -1;
            else
                screenDir.x = 0;

            if (Input.mousePosition.y >= Screen.height - 1)
                screenDir.z = 1;
            else if (Input.mousePosition.y <= 1)
                screenDir.z = -1;
            else
                screenDir.z = 0;

                if(screenDir != Vector3.zero)
            motor.Move(screenDir);
        }

        private void KeyBoardCtrl()
        {
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");

            movementDir.x = hor;
            movementDir.z = ver;

            float height = Input.GetAxis("Mouse ScrollWheel");

            if (height != 0)
                motor.AlterHeight(height);

            if (Input.GetKey(speedBoost))
                movementDir *= motor.GetSpeedBoostMultiplier();

            if(movementDir != Vector3.zero)
                motor.Move(movementDir);
        }
    }
}
