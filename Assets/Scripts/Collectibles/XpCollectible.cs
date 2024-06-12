using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpCollectible : MonoBehaviour
{
    [SerializeField]
    private ObjectType objectType;

    private PlayerMovement player;
    private float xpPoints;


    public void DestroyCollectible()
    {
        CollectiblesManager.Instance.DestroyXpGem(this);
        gameObject.SetActive(false);
    }

    public void SetXpPointsValue(float xp)
    {
        xpPoints = xp;
    }

    public ObjectType GetObjectType()
    {
        return objectType;
    }

    public float GetXPValue()
    {
        return xpPoints;
    }
}