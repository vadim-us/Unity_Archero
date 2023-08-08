using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private static List<Arrow> arrows = new List<Arrow>();

    private const int limitArrows = 50;

    private int damage;

    public int Damage
    {
        set { damage = value; }
        get { return damage; }
    }

    private Rigidbody rigiBody;

    private void Awake()
    {
        arrows.Add(this);

        Limit();

        transform.parent = null;

        rigiBody = GetComponent<Rigidbody>();
        rigiBody.AddForce(transform.forward * 30, ForceMode.Impulse);

        Game.ClearLevel += Remove;
    }

    private void Limit()
    {
        if (arrows.Count == limitArrows)
        {
            Destroy(arrows[0].gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        rigiBody.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        transform.parent = collision.collider.transform;
    }

    private void Remove()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Game.ClearLevel -= Remove;
        arrows.Remove(this);
    }
}
