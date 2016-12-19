using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpringDamper : MonoBehaviour
{
    private Convert _c = new Convert();
    public float Fspring, Fdamp,
        Ks = 0, Kd = 0, L0 = 0, L;
    private Vector3 _fsd, _fsd2, _x;
    public Vector3 E;
    public MonoParticle P1, P2;
    private Vector3[] particlePos = new Vector3[2];
    public bool Broken = false;

    void Start()
    {
        gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
    }

    void Update()
    {
        if (!Broken)
        {
            //Debug.DrawLine(_c.Vec3ToVector3(P1.p.r), _c.Vec3ToVector3(P2.p.r), Color.red);
            particlePos[0] = _c.Vec3ToVector3(P1.P.R);
            particlePos[1] = _c.Vec3ToVector3(P2.P.R);
            gameObject.GetComponent<LineRenderer>().SetWidth(.5f, .5f);
            gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
        }
        else
            gameObject.GetComponent<LineRenderer>().enabled = false;
    }

    public void ComputeForces()
    {
        //Get the current unit length
        var eNorm = _c.Vec3ToVector3(P2.P.R - P1.P.R);
        L = eNorm.magnitude;
        E = eNorm / L;

        //Make 1D directions
        var d1D1 = Vector3.Dot(E, _c.Vec3ToVector3(P1.P.V));
        var d1D2 = Vector3.Dot(E, _c.Vec3ToVector3(P2.P.V));

        //Calculate Spring Force
        Fspring = -Ks * (L0 - L);

        //Calculate Damping Force
        Fdamp = -Kd * (d1D1 - d1D2);

        //Calculate Spring Damper
        _fsd = (Fspring + Fdamp) * E;

        //Add forces
        P1.P.AddForce(_c.Vector3ToVec3(_fsd));
        P2.P.AddForce(_c.Vector3ToVec3(-_fsd));
    }

}
