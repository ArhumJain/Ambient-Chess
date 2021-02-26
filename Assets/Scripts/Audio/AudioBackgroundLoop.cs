using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBackgroundLoop : MonoBehaviour
{
    [SerializeField] AudioSource source;

    // Update is called once per frame
    void Update()
    {
        if(!source.isPlaying)
            source.Play();
    }
}
