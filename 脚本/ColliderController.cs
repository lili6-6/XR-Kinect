using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private float doorAudioCooldown = 1f;
    [SerializeField] private float armAudioCooldown = 1f;

    private float lastDoorAudioTime = -999f;
    private float lastArmAudioTime = -999f;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col == null || col.collider == null)
            return;

        AudioManager audioManager = GameSceneManager.instance ? GameSceneManager.instance.audioManager : null;
        if (audioManager == null)
            return;

        if (col.collider.CompareTag("Door"))
        {
            if (Time.time - lastDoorAudioTime < doorAudioCooldown)
                return;

            if (audioManager.doorAudio != null)
                audioManager.doorAudio.Play();

            lastDoorAudioTime = Time.time;
        }
        else if (col.collider.CompareTag("Arm"))
        {
            if (Time.time - lastArmAudioTime < armAudioCooldown)
                return;

            if (audioManager.armHitAudio != null)
                audioManager.armHitAudio.Play();

            lastArmAudioTime = Time.time;
        }
    }
}
