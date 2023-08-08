using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public event Action Enter;

    private void Awake()
    {
        Game.OpenGate += OpenOrClose;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.GetComponent<Player>())
        {
            OpenOrClose();
            Enter?.Invoke();
        }
    }

    private void OpenOrClose()
    {
        GetComponent<Animator>().SetTrigger("open");
    }
}
