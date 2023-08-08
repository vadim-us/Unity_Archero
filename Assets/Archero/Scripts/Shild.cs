using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour
{
    private static List<Shild> shilds = new List<Shild>();

    public static List<Shild> GetShilds { get { return shilds; } }

    private void Awake()
    {
        shilds.Add(this);
        Game.ClearLevel += Remove;

        transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
    }

    protected virtual void Remove()
    {
        shilds.Remove(this);
        Destroy(gameObject);
    }
}
