using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null) this.kitchenObjectParent.ClearKitchenObject();
        this.kitchenObjectParent = kitchenObjectParent;
        this.kitchenObjectParent.SetKitchenObject(this);
        transform.parent = this.kitchenObjectParent.GetPlacementTransform();
        transform.localPosition = Vector3.zero;
    }
}
