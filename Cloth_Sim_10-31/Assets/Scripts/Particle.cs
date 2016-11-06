using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Particle : MonoBehaviour
{
    public float m = 1;
    public float s = 1;
    public bool anchor;
    public Vector3 Pos0;
    public Vector3 r;
    public Vector3 v;
    public Vector3 a;
    public Vector3 Force;
    public Vector3 g = new Vector3(0, -9.8f, 0);
    public Vector3 Fgravity;
    public Vector3 screenPoint;
    public Vector3 offset;
    public bool selected = false;
    Camera camera;
    void Start()
    {
        camera = FindObjectOfType<Camera>();
        Force = Vector3.zero;
        Pos0 = transform.position;
        r = transform.position;
        //Calculate acceleration
        a = (1 / m) * Force;

        //Calculate velocity
        v += (a * Time.deltaTime);

        //Calculate position
        r += (v * Time.deltaTime);

        transform.position = r;
    }

    void Update()
    {
        Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
        if ((Math.Abs(screenPos.x - Input.mousePosition.x) < 5f) && (Math.Abs(screenPos.y - Input.mousePosition.y) < 5f))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //Debug.Log(transform.position);
                Debug.Log("gotcha");
                selected = true;
            }
        }
        if(selected)
        {
            //Convert camera pos to screen space, find screen space difference
            //Vector3 MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            //Debug.Log("Screen Particle Pos: " + screenPos);
            //Debug.Log("Screen Mouse Pos: " + MousePos);
            ////Debug.Log("World Camera Pos: " + camera.ScreenToWorldPoint(new Vector3(100,100, camera.nearClipPlane)));
            //Debug.Log("PrevPos: " + transform.position);
            //Vector3 Difference = new Vector3(MousePos.x - screenPos.x, MousePos.y - screenPos.y, 0);
            //Debug.Log("Difference: " + Difference);
            //Vector3 NewPos = new Vector3(transform.position.x /*+ Difference.x*/, transform.position.y + Difference.y, transform.position.z + Difference.z);
            //transform.position = NewPos;

            //Vector3 screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
            //Vector3 offset = Input.mousePosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
            //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
            //Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            //if (offset.magnitude >= 5.0f)
            //{
            //    transform.position = curPosition;
            //}

        }
        r = transform.position;

        if (!anchor)
        {

            //Calculate acceleration
            a = (1 / m) * Force;

            //Calculate velocity
            v += (a * Time.deltaTime);

            //Calculate position
            r += (v * Time.deltaTime);

            //Reset Force
            Force = Vector3.zero;
        }

        transform.position = r;
    }

    public void UpdateParticle()
    {

    }

    public void ApplyGravity()
    {
        g = new Vector3(0, -9.8f, 0) * (m);
        Fgravity = g * m;
        AddForce(Fgravity);
    }

    public void AddForce(Vector3 force)
    {
        if(!anchor)
            Force += force;
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}
