using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Shooting : NetworkBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public Camera cam;
    private float threshold = .8f;
    private Transform player;

    void Start()
    {
        if (!IsOwner) return;
        player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 weaponPos = (player.position + mousePos) / 2f;
        
        weaponPos.x = Mathf.Clamp(weaponPos.x, -threshold + player.position.x, threshold + player.position.x);
        weaponPos.y = Mathf.Clamp(weaponPos.y, -(threshold + .5f) + player.position.y, threshold + player.position.y);

        firePoint.position = weaponPos;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 fireDir = (mousePos - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(fireDir.x, fireDir.y).normalized * bulletForce;

        ShootServerRpc();
    }

    void PrintShoot(ulong clientId)
    {
        Debug.Log(clientId + " shot!");
    }

    [ServerRpc]
    private void ShootServerRpc(ServerRpcParams serverRpcParams = default)
    {
        PlayerManager.instance.players[serverRpcParams.Receive.SenderClientId].GetComponent<Shooting>().PrintShoot(serverRpcParams.Receive.SenderClientId);
    }
}
