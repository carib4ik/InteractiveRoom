using UnityEngine;

public class InteractiveController : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;

    private InteractableItem _item;
    private Transform _inventoryHolder;
    private InteractableItem _heldItem;

    private void Awake()
    {
        _inventoryHolder = transform.GetChild(0);
    }

    private void Update()
    {
        var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, 2f, _layer))
        {
            if (hitInfo.collider.GetComponent<InteractableItem>())
            {
                _item = hitInfo.collider.GetComponent<InteractableItem>();
                _item.SetFocus();
                
                // подбираем предмет
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!_heldItem)
                    {
                        TakeItem();
                    }
                    else
                    {
                        DropItem();
                        TakeItem();
                    }
                }
            }

            // Дверь
            if (hitInfo.collider.GetComponent<Door>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitInfo.collider.GetComponent<Door>().SwitchDoorState();
                }
            }
        }
        else
        {
            _item?.RemoveFocus();
            _item = null;
        }
        
        // бросок
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_heldItem)
            {
                ThrowItem();
            }
        }
    }

    private void TakeItem()
    {
        _item.transform.GetComponent<Rigidbody>().useGravity = false;
        _item.transform.GetComponent<Rigidbody>().isKinematic = true;
        _item.transform.SetParent(_inventoryHolder);
        _item.transform.position = _inventoryHolder.position;
        _heldItem = _item;
    }

    private void DropItem()
    {
        _heldItem.transform.GetComponent<Rigidbody>().useGravity = true;
        _heldItem.transform.GetComponent<Rigidbody>().isKinematic = false;
        _heldItem.transform.parent = null;
        _heldItem = null;
    }

    private void ThrowItem()
    {
        _heldItem.transform.GetComponent<Rigidbody>().isKinematic = false;
        _heldItem.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
        
        DropItem();
    }
}
