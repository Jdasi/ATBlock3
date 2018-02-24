using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AppPanel : MonoBehaviour
{
    private Vector3 original_position;

    private Vector3 old_pos;
    private Vector3 drag_origin;

    private bool dragging;


    public bool IsActive()
    {
        return this.gameObject.activeSelf;
    }


    public void Toggle()
    {
        if (!IsActive())
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    
    public void Activate()
    {
        if (original_position == Vector3.zero)
            original_position = this.transform.position;

        this.transform.position = original_position;
        dragging = false;

        this.gameObject.SetActive(true);
        OnActivate();
    }


    public void Deactivate()
    {
        this.gameObject.SetActive(false);
        OnDeactivate();
    }


    public void DragStart()
    {
        if (Input.GetMouseButton(0))
        {
            dragging = true;

            old_pos = this.transform.position;
            drag_origin = Input.mousePosition;
        }
    }


    void Update()
    {
        if (dragging)
        {
            Vector3 diff = Input.mousePosition - drag_origin;
            this.transform.position = old_pos + diff;

            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
        }

        OnUpdate();
    }


    public abstract void OnActivate();
    public abstract void OnDeactivate();

    public virtual void OnUpdate() {}

}
