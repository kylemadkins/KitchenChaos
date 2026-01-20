using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform placementTransform;

    private KitchenObject kitchenObject;

    public void Start()
    {
        if (kitchenObjectSO)
        {
            kitchenObject = Instantiate(kitchenObjectSO.prefab, placementTransform).GetComponent<KitchenObject>();
            kitchenObject.SetKitchenObjectParent(this);
        }
    }

    public void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else if (!player.HasKitchenObject())
        {
            kitchenObject.SetKitchenObjectParent(player);
        }
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public Transform GetPlacementTransform()
    {
        return placementTransform;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        if (!HasKitchenObject()) this.kitchenObject = kitchenObject;
    }
}
