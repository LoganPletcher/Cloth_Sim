using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SpringDamper : MonoBehaviour
{
    public float Fspring, Fdamp, 
        Ks = 0, Kd = 0, l0 = 0, l;
    Vector3  Fsd, Fsd2, x;
    public Vector3 e;
    public Particle P1, P2;
    Vector3 []particlePos = new Vector3[2];

    void Start()
    {
        //gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
    }

    void Update()
    {
        //P1.ApplyGravity();
        //P2.ApplyGravity();
        //ComputeForces();
        //P1.UpdateParticle();
        //P2.UpdateParticle();
        Debug.DrawLine(P1.r, P2.r, Color.red);
        //gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);

    }

    public void ComputeForces()
    {
        //Get the current unit length
        Vector3 eNorm = (P2.r - P1.r);
        l = eNorm.magnitude;
        e = eNorm / l;

        //Make 1D directions
        float d1D1 = Vector3.Dot(e, P1.v);
        float d1D2 = Vector3.Dot(e, P2.v);

        //Calculate Spring Force
        Fspring = -Ks * (l0 - l);

        //Calculate Damping Force
        Fdamp = -Kd * (d1D1 - d1D2);

        //Calculate Spring Damper
        Fsd = (Fspring + Fdamp) * e;

        //Add forces
        P1.AddForce(Fsd);
        P2.AddForce(-Fsd);
    }

}
