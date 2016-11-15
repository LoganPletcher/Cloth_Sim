using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ClothTriangle : MonoBehaviour
{
    Convert c = new Convert();
    public MonoParticle P1, P2, P3;
    public float p, Cd;
    public Vector3 Vair, Vsurface;
    public Vector3 P1v, P2v, P3v, v;
    public bool broken = false;

    void Start()
    {
    }

    public void CalcAeroForce()
    {
        //Calculate Average Velocity
        Vsurface = (c.Vec3toVector3(P1.p.v + P2.p.v + P3.p.v)) / 3;
        v = Vsurface - Vair;
        Vector3 n = Vector3.Cross(c.Vec3toVector3(P2.p.r - P1.p.r), c.Vec3toVector3(P3.p.r - P1.p.r)) /
            (Vector3.Cross(c.Vec3toVector3(P2.p.r - P1.p.r), c.Vec3toVector3(P3.p.r - P1.p.r))).magnitude;
        float A = .5f * Vector3.Cross(c.Vec3toVector3(P2.p.r - P1.p.r), c.Vec3toVector3(P3.p.r - P1.p.r)).magnitude;
        if (v.magnitude != 0)
        {
            float a = A * (Vector3.Dot(v, n) / v.magnitude);
            Vector3 Faero = (-.5f * (p * (v.magnitude * v.magnitude) * Cd * a * n)) / 3;
            P1.p.AddForce(c.Vector3toVec3(Faero));
            P2.p.AddForce(c.Vector3toVec3(Faero));
            P3.p.AddForce(c.Vector3toVec3(Faero));
        }
    }
}