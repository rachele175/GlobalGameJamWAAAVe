using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour {
    [SerializeField]
    internal Transform target;

    Color32 PurpleRam = new Color32(126, 19, 255, 79);
    Color32 OrangeEagle = new Color32(255, 222, 0, 79);
    Color32 PinkMonk = new Color32(255, 0, 255, 79);
    Color32 GreenIg = new Color32(105, 255, 0, 79);
    public SpriteRenderer beam;
    private void OnEnable()
    {
        if (target != null)
        {
            transform.position = target.position + Vector3.back * 2 + Vector3.up * 2 + Vector3.left;

            

            
        }
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + Vector3.back * 1 + Vector3.up * 1 + Vector3.left * 0.25f,  Time.deltaTime / 0.1f);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), Time.deltaTime / 0.3f);
    }

    public void assignColor()
    {
        switch (target.GetComponent<playerController>().controllerNumber)
        {
            case 4:
                GetComponent<SpriteRenderer>().color = PinkMonk;
               beam.color = PinkMonk;
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = GreenIg;
                beam.color = GreenIg;
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = OrangeEagle;
                beam.color = OrangeEagle;
                break;
            case 3:
                GetComponent<SpriteRenderer>().color = PurpleRam;
               beam.color = PurpleRam;
                break;

        }
    }
}
