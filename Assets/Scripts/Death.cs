using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] Vector3 spawnLocation;
    [SerializeField] GameObject player;
    [SerializeField] AudioSource deathSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            deathSound.Play();
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);

        Instantiate(player, spawnLocation, Quaternion.identity);

        yield return null;
    }


}
