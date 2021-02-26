using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutputController : MonoBehaviour
{
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioSource captureSound;
    [SerializeField] private AudioSource selectorShine;

    public void PlayMoveSound()
    {
        moveSound.Play();
    }
    public void PlayCaptureSound()
    {
        captureSound.Play();
    }
    public void PlaySelectorShine()
    {
        selectorShine.Play();
    }
}
