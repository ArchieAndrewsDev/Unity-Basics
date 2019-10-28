using UnityEngine;

namespace Basics.Camera
{
    public class RTSCameraMotor : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed = 5;

        [SerializeField]
        private float speedBoostMultiplier = 2;

        [SerializeField]
        private float heightFromFloor = 5;

        public void Move(Vector3 direction)
        {
            transform.position += (direction * movementSpeed) * Time.deltaTime;
            CheckHeight();
        }

        public float GetSpeedBoostMultiplier()
        {
            return speedBoostMultiplier;
        }

        public void AlterHeight(float heightIncrement)
        {
            heightFromFloor -= heightIncrement;
            CheckHeight();
        }

        private void CheckHeight()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                transform.position -= Vector3.up * (hit.distance - heightFromFloor);
            }
        }
    }
}
