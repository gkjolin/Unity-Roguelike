using UnityEngine;

namespace AKSaigyouji.Roguelike
{
    public sealed class CameraController : MonoBehaviour
    {
        public bool IsMapToggled { get { return isMapToggled; } }

        [SerializeField] Transform followTarget;
        [SerializeField] float mapScrollSpeed = 10;

        Camera mainCamera;

        float xDelta;
        float yDelta;

        bool isMapToggled = false;

        const int MAP_ZOOM_FACTOR = 6;

        const float UPPER_BOUND = 0.8f;
        const float LOWER_BOUND = 0.2f;
        const float POST_ADJUSTMENT = 0.3f;

        void Start()
        {
            mainCamera = Camera.main;
            ComputeDeltas();
        }

        void LateUpdate()
        {
            if (!isMapToggled) // Follow the player, but only when the player isn't using the map
            {
                TrackTarget();
            }
        }

        void TrackTarget()
        {
            Vector3 targetViewportPosition = mainCamera.WorldToViewportPoint(followTarget.position);
            float y = targetViewportPosition.y;
            float x = targetViewportPosition.x;
            while ((UPPER_BOUND < y || y < LOWER_BOUND) || (UPPER_BOUND < x || x < LOWER_BOUND))
            {
                Vector3 adjustment = Vector3.zero;
                if (y > UPPER_BOUND)
                {
                    adjustment.y += yDelta;
                }
                else if (y < LOWER_BOUND)
                {
                    adjustment.y -= yDelta;
                }
                if (x > UPPER_BOUND)
                {
                    adjustment.x += xDelta;
                }
                else if (x < LOWER_BOUND)
                {
                    adjustment.x -= xDelta;
                }
                transform.position += adjustment;
                targetViewportPosition = mainCamera.WorldToViewportPoint(followTarget.position);
                y = targetViewportPosition.y;
                x = targetViewportPosition.x;
            }
        }

        public void TranslateMap(Vector3 translation)
        {
            mainCamera.transform.position += mapScrollSpeed * translation;
        }

        public void ActivateMap()
        {
            isMapToggled = true;
            mainCamera.orthographicSize *= MAP_ZOOM_FACTOR;
        }

        public void DeactivateMap()
        {
            isMapToggled = false;
            mainCamera.orthographicSize /= MAP_ZOOM_FACTOR;
        }

        void ComputeDeltas()
        {
            // When we get close to the edge of the screen, we shift the camera by these amounts to keep the camera
            // over the player without constantly tracking directly on top of it.
            Vector2 adjusted = POST_ADJUSTMENT * Vector2.one;
            Vector2 top = UPPER_BOUND * Vector2.up;
            Vector2 right = UPPER_BOUND * Vector2.right;

            Vector3 adjustedWorld = mainCamera.ViewportToWorldPoint(adjusted);
            Vector3 topWorld = mainCamera.ViewportToWorldPoint(top);
            Vector3 rightWorld = mainCamera.ViewportToWorldPoint(right);

            xDelta = rightWorld.x - adjustedWorld.x;
            yDelta = topWorld.y - adjustedWorld.y;
        }

    } 
}