using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private Transform firePoit;

    private Character owner;
    public Character SetOwner { set { owner = value; } }

    private void Start()
    {
        owner.Shot += Shot;
    }

    private void Shot()
    {
        Instantiate(arrowPrefab, firePoit).GetComponent<Arrow>().Damage = owner.GetDamage;
    }
}
