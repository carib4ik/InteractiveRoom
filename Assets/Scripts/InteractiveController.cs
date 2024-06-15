using UnityEngine;

public class InteractiveController : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    
    private void Update()
    {
        var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, 1f, _layer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var door = hitInfo.collider.GetComponent<Door>();
                door.SwitchDoorState();
            }
        }
    }
}
