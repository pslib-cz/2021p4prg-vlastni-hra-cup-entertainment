using UnityEngine;

public class UserInputs : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;
    public float minZoomDist = 10;
    public float maxZoomDist = 30;
    public float RotateAmount = 10;
    public float RotateSpeed = 100;


    private Camera cam;
    private Player player;

    void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        player = transform.root.GetComponentInChildren<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player && player.human)
        {
            Move();
            RotateCamera();
            Zoom();
            MouseActivity();
        }
    }


    private void RotateCamera()
    {
        Vector3 origin = cam.transform.eulerAngles;
        Vector3 destination = origin;

        //detect rotation amount if ALT is being held and the Right mouse button is down
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * RotateAmount;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * RotateSpeed);
        }
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * zInput + transform.right * xInput;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, cam.transform.position);
        if (dist < minZoomDist && scrollInput > 0f)
            return;
        if (dist > maxZoomDist && scrollInput < 0f)
            return;
        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }
    public void FocusOnPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    private void MouseActivity()
    {
        if (Input.GetMouseButtonDown(0)) LeftMouseClick();
        else if (Input.GetMouseButtonDown(1)) RightMouseClick();
    }
    private void LeftMouseClick()
    {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();
            if (hitObject && hitPoint != ResourceManager.InvalidPosition)
            {
                if (player.SelectedObject) player.SelectedObject.MouseClick(hitObject, hitPoint, player);
                else if (hitObject.name != "Ground")
                {
                    WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
                    if (worldObject)
                    {
                        //we already know the player has no selected object
                        player.SelectedObject = worldObject;
                        worldObject.SetSelection(true);
                    }
                }
            }
    }
    private void RightMouseClick()
    {
        if (!Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false);
            player.SelectedObject = null;
        }
    }
    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject;
        return null;
    }
    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return ResourceManager.InvalidPosition;
    }
}
