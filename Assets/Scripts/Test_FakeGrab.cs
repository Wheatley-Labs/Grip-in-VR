using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FakeGrab : MonoBehaviour {

    SteamVR_TrackedController steamCtrl;
    public float selectRadius = 1;
	void Start () {
        this.steamCtrl = GetComponent<SteamVR_TrackedController>();
	}


    private void OnEnable()
    {
        this.steamCtrl = GetComponent<SteamVR_TrackedController>();
        steamCtrl.PadClicked += HandlePadClicked;
        steamCtrl.TriggerClicked += HandleTriggerClicked;
    }

    private void OnDisable()
    {
        steamCtrl.TriggerClicked -= HandleTriggerClicked;
        steamCtrl.PadClicked -= HandlePadClicked;
    }

    // Update is called once per frame
    void Update () {
        
     
	}


 
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, selectRadius);
        }


    #region Primitive Selection


    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, selectRadius, transform.forward, out hit, 100)){
            if (hit.transform.CompareTag("Grab"))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }

    private void HandlePadClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log(e.padY);

        if (e.padY < 0)
        {
            //Debug.Log(e.padY);
        }
        else
        {

        }
            //SelectNextPrimitive();
    }
    #endregion
}
