using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ShootingGame.Player;

public class RedZone : MonoBehaviourPunCallbacks
{
    private float m_Timer;
    private float m_DamageTimer;
    private float m_RandomX;
    private float m_RandomZ;

    private void Start()
    {
        m_RandomX = UnityEngine.Random.Range(-40 , 40);
        m_RandomZ = UnityEngine.Random.Range(-40 , 40);
        
        transform.position = new Vector3(m_RandomX,transform.position.y,m_RandomZ);
    }

    private void Update()
    {
        m_DamageTimer += Time.deltaTime;
        if (!photonView.IsMine)
        {
            return;
        }
        m_Timer += Time.deltaTime;
        if (m_Timer < 20.0f)
        {
            return;
        }
        m_RandomX = UnityEngine.Random.Range(-40 , 40);
        m_RandomZ = UnityEngine.Random.Range(-40 , 40);
        transform.position = new Vector3(m_RandomX,transform.position.y,m_RandomZ);
        m_Timer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController =  other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.ApplyDamage();
        }
        m_DamageTimer = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_DamageTimer > 2.0f)
        {
            PlayerController playerController =  other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ApplyDamage();
            }
            m_DamageTimer = 0;
        }
        
    }
}
