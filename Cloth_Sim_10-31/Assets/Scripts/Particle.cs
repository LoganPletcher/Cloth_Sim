using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class Vec3
{
    public Vec3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }
    public Vec3(float a, float b, float c)
    {
        X = a;
        Y = b;
        Z = c;
    }
    public Vec3(Vec3 other)
    {
        this.X = other.X;
        this.Y = other.Y;
        this.Z = other.Z;
    }
    float X = 0, Y = 0, Z = 0;
    public float x { get { return X; } }
    public float y { get { return Y; } }
    public float z { get { return Z; } }

    public static Vec3 operator *(Vec3 v, float f)
    {
        Vec3 result = new Vec3();
        result.X = v.X * f;
        result.Y = v.Y * f;
        result.Z = v.Z * f;
        return result;
    }
    public static Vec3 operator *(float f, Vec3 v)
    {
        Vec3 result = new Vec3();
        result.X = v.X * f;
        result.Y = v.Y * f;
        result.Z = v.Z * f;
        return result;
    }
    public static Vec3 operator +(Vec3 v1, Vec3 v2)
    {
        Vec3 result = new Vec3();
        result.X = v1.X + v2.X;
        result.Y = v1.Y + v2.Y;
        result.Z = v1.Z + v2.Z;
        return result;
    }
    public static Vec3 operator -(Vec3 v1, Vec3 v2)
    {
        Vec3 result = new Vec3();
        result.X = v1.X - v2.X;
        result.Y = v1.Y - v2.Y;
        result.Z = v1.Z - v2.Z;
        return result;
    }
    public static Vec3 operator -(Vec3 v)
    {
        Vec3 result = new Vec3(-v.x, -v.y, -v.z);
        return result;
    }
}
public class Particle
{
    public Particle()
    {
        m = 1;
        anchor = false;
        broken = false;
        r = new Vec3(0, 0, 0);
        v = new Vec3(0, 0, 0);
        a = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        g = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }
    public Particle(Vec3 position)
    {
        r = new Vec3(position);
        m = 1;
        anchor = false;
        broken = false;
        v = new Vec3(0, 0, 0);
        a = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        g = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }
    public Particle(float mass)
    {
        m = mass;
        anchor = false;
        broken = false;
        r = new Vec3(0, 0, 0);
        v = new Vec3(0, 0, 0);
        a = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        g = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }
    public Particle(float mass, Vec3 position)
    {
        m = mass;
        r = new Vec3(position);
        anchor = false;
        broken = false;
        v = new Vec3(0, 0, 0);
        a = new Vec3(0, 0, 0);
        Force = new Vec3(0, 0, 0);
        g = new Vec3(0, -9.8f, 0);
        Fgravity = new Vec3(0, 0, 0);
    }


    public float m = 10;
    public bool anchor;
    public bool broken = false;
    public Vec3 r;
    public Vec3 v;
    public Vec3 a;
    public Vec3 Force;
    public Vec3 g = new Vec3(0, -9.8f, 0);
    public Vec3 Fgravity;

    public void Update(float deltaTime)
    {
        if (!anchor)
        {
            //Calculate acceleration
            a = (1 / m) * Force;

            //Calculate velocity
            v += (a * deltaTime);

            //Calculate position
            r += (v * deltaTime);

            //Reset Force
            Force = new Vec3(0, 0, 0);
        }
    }
    public void ApplyGravity(float i)
    {
        g = new Vec3(0, -9.8f, 0) * (m);
        Fgravity = g * m * i;
        AddForce(Fgravity);
    }

    public void AddForce(Vec3 force)
    {
        if (!anchor)
            Force += force;
    }
}