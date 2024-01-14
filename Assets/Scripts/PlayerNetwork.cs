using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;
using Cinemachine;

public class PlayerNetwork : NetworkBehaviour
{
    // [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;

    private float previousPosX;


    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;
        } 
        else
        {
            vc.Priority = 0;
        }
    }

    private void Update()
    {
        // GLOBAL


        //PLAYER FLIPPING
        if ((transform.position.x - previousPosX) < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if ((transform.position.x - previousPosX) > 0)
        {
            spriteRenderer.flipX = false;
        }
        previousPosX = transform.position.x;


        // PLAYER


    }
}
