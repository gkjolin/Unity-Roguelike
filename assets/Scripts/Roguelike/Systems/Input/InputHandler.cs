using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    public sealed class InputHandler : MonoBehaviour
    {
        [SerializeField] CameraController cameraController;
        [SerializeField] PlayerController playerController;
        [SerializeField] InventoryUI inventory;

        const string INVENTORY_AXIS = "Inventory";
        const string HORIZONTAL_AXIS = "Horizontal";
        const string VERTICAL_AXIS = "Vertical";
        const string PICKUP_AXIS = "Get";
        const string EXIT_AXIS = "Exit";
        const string MAP_AXIS = "Map";

        Vector3 accumulatedTranslation = Vector3.zero;

        void Start()
        {
            Assert.IsNotNull(playerController);
            Assert.IsNotNull(cameraController);
        }

        void Update()
        {
            if (Input.GetButtonDown(MAP_AXIS))
            {
                ToggleMap();
            }
            if (Input.GetButtonDown(INVENTORY_AXIS))
            {
                inventory.ToggleInventory();
            }

            // The following has the effect of diverting inputs towards the correct system, so that e.g.
            // the player doesn't move while we look around the map. 
            if (cameraController.IsMapToggled)
            {
                UpdateMap();
            }
            else
            {
                UpdateCharacterController();
            }
        }

        void ToggleMap()
        {
            if (cameraController.IsMapToggled)
            {
                // Subtract off all the translations thus far, so that when we toggle off the map, we're focused back
                // on where we were before we toggled the map on.
                cameraController.TranslateMap(-accumulatedTranslation);
                cameraController.DeactivateMap();
                accumulatedTranslation = Vector3.zero;
            }
            else
            {
                cameraController.ActivateMap();
            }
        }

        void UpdateCharacterController()
        {
            if (Input.GetButtonDown(HORIZONTAL_AXIS) || Input.GetButtonDown(VERTICAL_AXIS))
            {
                float x = Input.GetAxisRaw(HORIZONTAL_AXIS);
                float y = Input.GetAxisRaw(VERTICAL_AXIS);

                playerController.MovePlayer((int)x, (int)y);
            }

            if (Input.GetButtonDown(PICKUP_AXIS))
            {
                playerController.TryPickupItem();
            }

            if (Input.GetButtonDown(EXIT_AXIS))
            {
                playerController.TryMoveDownStairs();
            }
        }

        void UpdateMap()
        {
            if (Input.GetButton(HORIZONTAL_AXIS) || Input.GetButton(VERTICAL_AXIS))
            {
                float x = Input.GetAxisRaw(HORIZONTAL_AXIS);
                float y = Input.GetAxisRaw(VERTICAL_AXIS);

                Vector3 translation = ((new Vector3(x, y)).normalized) * Time.deltaTime;

                accumulatedTranslation += translation;
                cameraController.TranslateMap(translation);
            }
        }
    } 
}