using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
    public AudioSource DoorOpenSound;
    private float _countdown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if(player != null && player.CanLeaveLevel)
        {
            StartCoroutine(StartExitCounter());
            DoorOpenSound.Play();
        }
    }
    private IEnumerator StartExitCounter()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
