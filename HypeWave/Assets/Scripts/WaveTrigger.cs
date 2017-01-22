using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : AkTriggerBase {

	public void Go()
    {
        if(triggerDelegate != null)
            triggerDelegate.Invoke(this.gameObject);
    }
}
