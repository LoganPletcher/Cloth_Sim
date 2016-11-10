using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpringDamper : MonoBehaviour
{
    Convert c = new Convert();
    public float Fspring, Fdamp,
        Ks = 0, Kd = 0, l0 = 0, l;
    Vector3 Fsd, Fsd2, x;
    public Vector3 e;
    public MonoParticle P1, P2;
    Vector3[] particlePos = new Vector3[2];
    public bool broken = false;

    void Start()
    {
        gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
    }

    void Update()
    {
        if (!broken)
        {
            Debug.DrawLine(c.Vec3toVector3(P1.p.r), c.Vec3toVector3(P2.p.r), Color.red);
            particlePos[0] = c.Vec3toVector3(P1.p.r);
            particlePos[1] = c.Vec3toVector3(P2.p.r);
            gameObject.GetComponent<LineRenderer>().SetWidth(.5f, .5f);
            gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
        }
        else
            gameObject.GetComponent<LineRenderer>().enabled = false;
    }

    public void ComputeForces()
    {
        //Get the current unit length
        Vector3 eNorm = c.Vec3toVector3(P2.p.r - P1.p.r);
        l = eNorm.magnitude;
        e = eNorm / l;

        //Make 1D directions
        float d1D1 = Vector3.Dot(e, c.Vec3toVector3(P1.p.v));
        float d1D2 = Vector3.Dot(e, c.Vec3toVector3(P2.p.v));

        //Calculate Spring Force
        Fspring = -Ks * (l0 - l);

        //Calculate Damping Force
        Fdamp = -Kd * (d1D1 - d1D2);

        //Calculate Spring Damper
        Fsd = (Fspring + Fdamp) * e;

        //Add forces
        P1.p.AddForce(c.Vector3toVec3(Fsd));
        P2.p.AddForce(c.Vector3toVec3(-Fsd));
    }

}
