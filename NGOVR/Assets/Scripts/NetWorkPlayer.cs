using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using System;

public class NetWorkPlayer : NetworkBehaviour
{
    [SerializeField] private Vector2 placementArea = new Vector2(-10.0f,10.0f);

    public override void OnNetworkSpawn()
    {
        DisableClientInput();
    }

    private void DisableClientInput()
    {
        if(IsClient && IsOwner)
        {
            var clientMoveProvider = GetComponent<NetWorkMoveProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            var clientTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();

            clientCamera.enabled = false;
            clientMoveProvider.enableInputActions = false;
            clientTurnProvider.enableTurnLeftRight = false;
            clientTurnProvider.enableTurnAround = false;
            clientHead.enabled = false;

            foreach(var controller in clientControllers)
            {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
            }

        }
    }

    private void Start()
    {
        if(IsClient && IsOwner)
        {
            transform.position = new Vector3(UnityEngine.Random.Range(placementArea.x,placementArea.y), transform.position.y, UnityEngine.Random.Range(placementArea.x,placementArea.y));
        }
    }
}
