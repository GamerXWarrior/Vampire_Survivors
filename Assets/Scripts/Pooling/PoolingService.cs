using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolingService<T> : MonoSingletonGeneric<PoolingService<T>>
{
    // Creating the list of items to be pooled
     private List<PooledItem<T>> pooledItems = new List<PooledItem<T>>();

    // returning the pooled item/object to the required class
    public virtual T GetItem(Enum _itemType)
    {
        var itemType = _itemType;
        PooledItem<T> item = null;

        if (pooledItems.Count > 0)
        {
            foreach (PooledItem<T> i in pooledItems)
            {
                if (!i.IsUsed)
                {
                    if (i.Itemtype.ToString() == itemType.ToString())
                    {
                        item = i;
                    }
                }
            }
            if (item != null)
            {
                item.IsUsed = true;
                return item.Item;
            }
        }
        return CreateNewPooledItem(itemType);
    }
    
    private T CreateNewPooledItem(Enum itemType)
    {
        PooledItem<T> pooledItem = new PooledItem<T>();
        pooledItem.Item = CreateItem();
        pooledItem.IsUsed = true;
        pooledItem.Itemtype = itemType;
        pooledItems.Add(pooledItem);
        return pooledItem.Item;
    }

    // returning item to the pooled list
    public virtual void ReturnItem(T item)
    {
        PooledItem<T> pooledItem = pooledItems.Find(i => i.Item.Equals(item));
        pooledItem.IsUsed = false;
    }

    protected virtual T CreateItem()
    {
        return default(T);
    }

    private class PooledItem<T>
    {
        public T Item;
        public bool IsUsed;
        public Enum Itemtype;
    }
}
