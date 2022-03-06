using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip hitClip;

    public void PlayHitClip()
    {
        AudioSource.PlayClipAtPoint(hitClip, Camera.main.transform.position);
    }
}
