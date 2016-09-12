using UnityEngine;
using System.Collections;

public class counterForceObject{

    float counterForce;
    float wallNumber;

    public counterForceObject(float cf, float wn)
    {
        counterForce = cf;
        wallNumber = wn;
    }
    public float getCF()
    {
        return counterForce;
    }
    public float getWN()
    {
        return wallNumber;
    }
    public void setCF(float cf)
    {
        counterForce = cf;
    }
    public void setWN(float wn)
    {
        wallNumber = wn;
    }
	
}
