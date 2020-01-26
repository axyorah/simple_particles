using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Populate : MonoBehaviour
{
    // physics-related params
    public Rigidbody positiveParticle;
    public Rigidbody negativeParticle;

    public float positiveCharge = 1f;
    public float negativeCharge = -1f;
    
    private Rigidbody[] particleArray;

    // controls-related params
    public GUIStyle buttonStyle;
    private int controlOffset;

    private void Start()
    {
        // get top and left offset for description and controls
        controlOffset = (int) Mathf.Round(Mathf.Min(Screen.width/16, Screen.height/10));

        // center the text on the custom button (only Quit has a custom button)
        buttonStyle.padding.left = 25;
        buttonStyle.padding.top = 5;
    }

    // Update is called once per frame
    void Update()
    {
        // recalculate particleArray on each tick
        particleArray = GameObject.FindObjectsOfType<Rigidbody>();
    }

    private void OnGUI()
    {
        GUI.TextField(new Rect(controlOffset, controlOffset, 250, 25), "Generate particles:");

        // set charge for positive and negative particles
        SetChargeViaSliders();

        // add buttons to generate particles
        AddParticlesViaButtons();
        
        // Destroy all particles
        DestroyAllParticles();

        // quit app
        QuitApp();
    }

    private void SetChargeViaSliders()
    {
        // add sliders to vary charge of particles
        positiveCharge = GUI.HorizontalSlider(new Rect(controlOffset+140, controlOffset+35, 75, 20), 
                                              Mathf.Round(positiveCharge), 1f, 5f);
        negativeCharge = GUI.HorizontalSlider(new Rect(controlOffset+140, controlOffset+65, 75, 20), 
                                              Mathf.Round(negativeCharge), -1f, -5f);

        // specify currently set charge value
        GUI.Box(new Rect(controlOffset+215, controlOffset+30, 35, 20), $"{Mathf.Round(positiveCharge)}");
        GUI.Box(new Rect(controlOffset+215, controlOffset+60, 35, 20), $"{Mathf.Round(negativeCharge)}");
    }

    private void AddParticlesViaButtons()
    {
        if (GUI.Button(new Rect(controlOffset+15, controlOffset+30, 75, 25), "Positive"))
        {
            Rigidbody newPositive = Instantiate(positiveParticle) as Rigidbody;
            newPositive.GetComponentInParent<Charge>().SetCharge(positiveCharge);
            newPositive.transform.position = new Vector3(Random.value, Random.value, Random.value) * 10
                                           - Vector3.one * 5;
            newPositive.transform.localScale = Vector3.one * (0.5f + positiveCharge / 2f);            
        }

        if (GUI.Button(new Rect(controlOffset+15, controlOffset+60, 75, 25), "Negative"))
        {
            Rigidbody newNegative = Instantiate(negativeParticle) as Rigidbody;
            newNegative.GetComponentInParent<Charge>().SetCharge(negativeCharge);
            newNegative.transform.position = new Vector3(Random.value, Random.value, Random.value) * 10
                                           - Vector3.one * 5;
            newNegative.transform.localScale = Vector3.one * (0.5f - negativeCharge / 2f);            
        }
    }

    private void DestroyAllParticles()
    {
        if (GUI.Button(new Rect(Screen.width-controlOffset-75, controlOffset, 75, 25), "Clear"))
        {
            // pop all Rigidbodies from the list (except for Room)
            for (int i = particleArray.Length - 1; i >= 0; i--)
            {
                if (particleArray[i].name != "Room")
                {
                    Destroy(particleArray[i].GetComponentInParent<Charge>());
                    Destroy(particleArray[i].GetComponentInParent<SphereCollider>());
                    Destroy(particleArray[i].GetComponentInParent<MeshRenderer>());
                    Destroy(particleArray[i]);
                }
            }
        }
    }

    private void QuitApp()
    {
        int leftOffset = (int) Screen.width - controlOffset - 75;
        int topOffset = (int) Screen.height - controlOffset - 25;

        if (GUI.Button(new Rect(leftOffset, topOffset, 75, 25), "Quit", buttonStyle))
        {
            Application.Quit();
        }        
    }
}
