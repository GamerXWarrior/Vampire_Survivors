using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollectiblesManager : MonoSingletonGeneric<CollectiblesManager>
{
    [SerializeField]
    private GameObject xpGemPrefab;
    [SerializeField]
    private float xpValue;
    [SerializeField]
    private Transform xpGemContainer;

    private bool isMaxLevelReached;
    private CollectiblePoolService collectiblePoolService;
    private List<XpCollectible> xpGemsHolder = new List<XpCollectible>();

    protected override void Start()
    {
        collectiblePoolService = GetComponent<CollectiblePoolService>();
        EventService.Instance.MaxLevelReached += OnMaxLevelReached;
        EventService.Instance.LevelEnd += DisableAllXpGems;
        EventService.Instance.LevelUP += OnLevelUp;
    }

    public void SpawnXpGem(Vector2 position)
    {
        if (isMaxLevelReached)
            return;

        XpCollectible xpGem = collectiblePoolService.GetCollectible(ObjectType.Collectible, xpGemPrefab);
        xpGem.gameObject.SetActive(true);
        xpGem.transform.position = position;
        xpGem.transform.SetParent(xpGemContainer);
        xpGem.SetXpPointsValue(xpValue);
        xpGemsHolder.Add(xpGem);
    }

    // Disabling all gems from the scene
    private void DisableAllXpGems()
    {
        if (xpGemsHolder.Count != 0)
        {
            for (int i = 0; i < xpGemsHolder.Count; i++)
            {
                if (xpGemsHolder[i] != null)
                {
                    //Debug.Log("xpGemHolder: " + xpGemsHolder + " i: " + i + " : " + xpGemsHolder[i]);
                    xpGemsHolder[i].gameObject.SetActive(false);
                    collectiblePoolService.ReturnItem(xpGemsHolder[i]);
                    xpGemsHolder[i] = null;
                }
            }
        }
    }

    private void OnMaxLevelReached()
    {
        isMaxLevelReached = true;
        DisableAllXpGems();
    }

    private void OnLevelUp()
    {
        xpValue *= PlayerManager.Instance.GetCurrentLevel();
        //Debug.Log("xpValue:" + xpValue);
    }

    //Disabling the gems when the player collets them

    public void DestroyXpGem(XpCollectible _xpGem)
    {
        if (_xpGem.GetObjectType() == ObjectType.Collectible)
        {
            for (int i = 0; i < xpGemsHolder.Count; i++)
            {
                if (_xpGem == xpGemsHolder[i])
                {
                    _xpGem.gameObject.SetActive(false);
                    collectiblePoolService.ReturnItem(_xpGem);
                    xpGemsHolder[i] = null;
                }
            }
        }
    }

}
