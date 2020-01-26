using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : MonoBehaviour
{
    public float charge = 1;
    private Rigidbody particle;

    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<Rigidbody>();        
    }
    
    public float GetCharge()
    {
        return charge;
    }

    public void SetCharge(float newCharge)
    {
        charge = newCharge;
        Debug.Log($"{this.name}'s charge is set to {newCharge}!");
    }
    
}
