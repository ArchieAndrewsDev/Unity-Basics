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
        private float heightSmoother = .1f;

        [SerializeField]
        private float heightFromFloor = 5;

        Vector3 targetHeight, velocity;

        private void Awake()
        {
            targetHeight = transform.position;
        }

        private void Update()
        {
            UpdateHeight();
        }

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

        private void UpdateHeight()
        {
            targetHeight.x = transform.position.x;
            targetHeight.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, targetHeight, ref velocity, heightSmoother);
        }

        private void CheckHeight()
        {
            RaycastHit hit;
            if (Physics.Raycast(targetHeight, Vector3.down, out hit))
            {
                targetHeight -= Vector3.up * (hit.distance - heightFromFloor);
            }
        }
    }
}
