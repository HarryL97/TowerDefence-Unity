using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum proType
{
    rock, arrow, fireball
};

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int attackStr;
    [SerializeField]
    private proType projectileType;

    public int AttackStr
    {
        get
        {
            return attackStr;
        }
    }

        public proType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }


}
