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

    void Start()
    {
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
        r = transform.position;

        //Calculate acceleration
        a = (1 / m) * Force;

        //Calculate velocity
        v += (a * Time.deltaTime);

        //Calculate position
        r += (v * Time.deltaTime);

        transform.position = r;

        //Reset Force
        Force = Vector3.zero;
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
}
