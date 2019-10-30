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

        [SerializeField]
        private Bounds cameraBounds;

        Vector3 targetHeight, velocity;

        private void Awake()
        {
            targetHeight = transform.position;
        }

        private void Update()
        {
            UpdateHeight();
        }

        private void LateUpdate()
        {
            CheckBounds();
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
                targetHeight -= Vector3.up * (hit.distance - heightFromFloor);
        }

        private void CheckBounds()
        {
            if (!cameraBounds.Contains(transform.position))
            {
                Vector3 clampPos = transform.position;

                clampPos.x = Mathf.Clamp(clampPos.x, cameraBounds.min.x, cameraBounds.max.x);
                clampPos.y = Mathf.Clamp(clampPos.y, cameraBounds.min.y, cameraBounds.max.y);
                clampPos.z = Mathf.Clamp(clampPos.z, cameraBounds.min.z, cameraBounds.max.z);

                transform.position = clampPos;
            }
        }
    }
}
