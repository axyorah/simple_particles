using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalMolecularInteraction : MonoBehaviour
{
    // physics-related params
    [Range(0f, 100f)]
    public float K = 40f;
    [Range(0f, 50f)]
    public float C = 4f;
    [Range(0f, 50f)]
    public float alpha = 2f;

    private Vector3 molecularInteractionForceVector;
    private Rigidbody thisParticle;
    private Rigidbody otherParticle;
    private Rigidbody[] particleArray;

    // controls-related params
    private int controlOffset;

    // Start is called before the first frame update
    void Start()
    {
        controlOffset = (int)Mathf.Round(Mathf.Min(Screen.width / 16, Screen.height / 10));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAllMolecularInteractionForceVectors();
    }

    private void UpdateAllMolecularInteractionForceVectors()
    {
        // we need to recalculate particleArray, as new particles can be added
        particleArray = GameObject.FindObjectsOfType<Rigidbody>();

        // iterate over each pair of particles to find their interaction
        foreach (Rigidbody thisParticle in particleArray)
        {
            // reset the attraction/repulsion force 
            molecularInteractionForceVector = Vector3.zero;

            foreach (Rigidbody otherParticle in particleArray)
            {
                if (thisParticle != otherParticle && thisParticle.name != "Room" && otherParticle.name != "Room")
                {
                    UpdateMolecularInteractionForceVector(thisParticle, otherParticle);
                }
            }

            thisParticle.AddForce(molecularInteractionForceVector);
        }
    }

    private void UpdateMolecularInteractionForceVector(Rigidbody thisParticle, Rigidbody otherParticle)
    {
        // get vector conneecting the centers of particles
        Vector3 delta = otherParticle.transform.position - thisParticle.transform.position;
        Vector3 direction = delta.normalized;
        float dist = delta.magnitude;

        // get particle charges
        float thisCharge = thisParticle.GetComponentInParent<Charge>().GetCharge();
        float otherCharge = otherParticle.GetComponentInParent<Charge>().GetCharge();

        /* simplified interaction is composed of:
         * - Coulomb interaction (opposite charges attract, and vv): 
         *     Coulomb Potential = K * charge1 * charge2 / dist [J]
         * - Pauli repulsion: 
         *     Pauli repulsion potential = C / dist * exp(-alpha *dist) [J]
         * Force is found as derivative of energy (potential) */
        molecularInteractionForceVector += (-K * thisCharge * otherCharge / Mathf.Pow(dist, 2)
                                           - C / Mathf.Pow(dist, 2) * Mathf.Exp(-alpha * dist)
                                           - C / dist * Mathf.Exp(-alpha * dist) * alpha)
                                           * direction;
    }

    private void OnGUI()
    {
        AddDescription();
        SetInteractionParamsViaSliders();        
    }

    private void AddDescription()
    {
        GUI.TextField(new Rect(controlOffset, controlOffset+120, 250, 135),
                                          "Simplified interaction:\n" +
                                          "  Coulomb Potential = K * q1 * q2 / r\n" +
                                          "  Pauli Repulsion = C / r * exp(-alpha * r)\n\n" +
                                          "where:\n" +
                                          "  q1,q1 - charge\n" +
                                          "  r - distance\n" +
                                          "  K,C,alpha - adjustable parameters:");
    }

    private void SetInteractionParamsViaSliders()
    {
        GUI.Box(new Rect(controlOffset+15, controlOffset+260, 50, 25), "K");
        K = GUI.HorizontalSlider(new Rect(controlOffset+80, controlOffset+270, 100, 10), K, 0f, 50f);
        GUI.Box(new Rect(controlOffset+195, controlOffset+260, 50, 25), $"{Mathf.Round(K)}");

        GUI.Box(new Rect(controlOffset+15, controlOffset+300, 50, 25), "C");
        C = GUI.HorizontalSlider(new Rect(controlOffset+80, controlOffset+310, 100, 10), C, 0f, 50f);
        GUI.Box(new Rect(controlOffset+195, controlOffset+300, 50, 25), $"{Mathf.Round(C)}");

        GUI.Box(new Rect(controlOffset+15, controlOffset+340, 50, 25), "alpha");
        alpha = GUI.HorizontalSlider(new Rect(controlOffset+80, controlOffset+350, 100, 10), alpha, 0f, 10f);
        GUI.Box(new Rect(controlOffset+195, controlOffset+340, 50, 25), $"{Mathf.Round(alpha * 10) / 10}");
    }
}
