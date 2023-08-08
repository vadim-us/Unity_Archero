using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    private static List<Coin> coins = new List<Coin>();

    public event Action<Coin> ColectCoin;

    [SerializeField]
    private List<TextMeshPro> coinSide;

    private int value;
    public int Value
    {
        get { return value; }
        set
        {
            this.value = value;
            coinSide.ForEach((side) => { side.text = value.ToString(); });
        }
    }
    
    private void Awake()
    {
        coins.Add(this);

        Game.ClearLevel += Remove;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            ColectCoin?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void Remove()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Game.ClearLevel -= Remove;
        coins.Remove(this);
    }
}
