﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpringDamper : MonoBehaviour
{
    public float Fspring, Fdamp, 
        Ks = 0, Kd = 0, l0 = 0, l;
    Vector3  Fsd, Fsd2, x;
    public Vector3 e;
    public Particle P1, P2;
    Vector3 []particlePos = new Vector3[2];
    public bool broken = false;

    void Start()
    {
        gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
    }

    void Update()
    {
        //P1.ApplyGravity();
        //P2.ApplyGravity();
        //ComputeForces();
        //P1.UpdateParticle();
        //P2.UpdateParticle();
        if (!broken)
        {
            Debug.DrawLine(P1.r, P2.r, Color.red);
            particlePos[0] = P1.r;
            particlePos[1] = P2.r;
            gameObject.GetComponent<LineRenderer>().SetWidth(.5f, .5f);
            gameObject.GetComponent<LineRenderer>().SetPositions(particlePos);
        }
        else
            gameObject.GetComponent<LineRenderer>().enabled = false;
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
