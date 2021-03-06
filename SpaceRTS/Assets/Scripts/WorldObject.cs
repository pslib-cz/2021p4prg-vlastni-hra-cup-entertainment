using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public string objectName;
    public Texture2D buildImage;
    public int cost, sellValue, hitPoints, maxHitPoints;

    protected Player player;
    protected string[] actions = { };
    protected bool currentlySelected = false;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.GetComponentInChildren<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelection(bool selected)
    {
        currentlySelected = selected;
    }
    public string[] GetActions()
    {
        return actions;
    }

    public virtual void PerformAction(string actionToPerform)
    {
        //it is up to children with specific actions to determine what to do with each of those actions
    }
    public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        //only handle input if currently selected
        if (currentlySelected && hitObject && hitObject.name != "Ground")
        {
            WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
            //clicked on another selectable object
            if (worldObject) ChangeSelection(worldObject, controller);
        }
    }
    private void ChangeSelection(WorldObject worldObject, Player controller)
    {
        //this should be called by the following line, but there is an outside chance it will not
        SetSelection(false);
        if (controller.SelectedObject) controller.SelectedObject.SetSelection(false);
        controller.SelectedObject = worldObject;
        worldObject.SetSelection(true);
    }
}
