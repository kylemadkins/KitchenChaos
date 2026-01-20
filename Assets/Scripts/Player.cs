using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask countersLayer;
    [SerializeField] private Transform holdTransform;

    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private float moveDist;
    private bool isWalking = false;
    private KitchenObject kitchenObject;

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        kitchenObject = Instantiate(kitchenObjectSO.prefab, holdTransform).GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(this);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit hit, interactionDistance, countersLayer))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact(this);
            }
        }
    }

    private void Update()
    {
        HandlePlayerMovement();
        HandlePlayerInteraction();
    }

    private void HandlePlayerMovement()
    {
        Vector3 inputVector = gameInput.GetMovementVectorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
            moveDist = moveSpeed * Time.deltaTime;

            Vector3 spherePt1 = transform.position + Vector3.up * playerRadius;
            Vector3 spherePt2 = transform.position + Vector3.up * (playerHeight - playerRadius);

            if (Physics.CapsuleCast(spherePt1, spherePt2, playerRadius, moveDir, out RaycastHit hit, moveDist))
            {
                Debug.DrawRay(hit.point, hit.normal * 2, Color.red);
                // Slide along the hit surface instead of stopping
                Vector3 slideDir = Vector3.ProjectOnPlane(moveDir, hit.normal).normalized;
                lastMoveDir = slideDir;
                Debug.DrawRay(transform.position, slideDir * 2, Color.green);
                transform.position += slideDir * moveDist;
            }
            else
            {
                // No hit -> move normally
                transform.position += moveDir * moveDist;
            }

            // Rotate towards movement direction
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
    
    private void HandlePlayerInteraction()
    {
        Debug.DrawRay(transform.position, lastMoveDir * interactionDistance, Color.magenta);
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
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

    public Transform GetPlacementTransform()
    {
        return holdTransform;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 spherePt1 = transform.position + Vector3.up * playerRadius;
        Vector3 spherePt2 = transform.position + Vector3.up * (playerHeight - playerRadius);

        float radius = playerRadius;

        // Draw start and end spheres
        Gizmos.DrawWireSphere(spherePt1 + moveDir * moveDist, radius);
        Gizmos.DrawWireSphere(spherePt2 + moveDir * moveDist, radius);

        // Draw connecting lines between the spheres
        Gizmos.DrawLine(spherePt1 + Vector3.left * radius, spherePt2 + Vector3.left * radius);
        Gizmos.DrawLine(spherePt1 + Vector3.right * radius, spherePt2 + Vector3.right * radius);
        Gizmos.DrawLine(spherePt1 + Vector3.forward * radius, spherePt2 + Vector3.forward * radius);
        Gizmos.DrawLine(spherePt1 + Vector3.back * radius, spherePt2 + Vector3.back * radius);
    }
}
