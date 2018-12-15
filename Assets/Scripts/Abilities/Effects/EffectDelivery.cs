using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDelivery : MonoBehaviour {

    public List<EffectOriginPoint> effectOrigins = new List<EffectOriginPoint>();




    public Transform GetOriginPoint(Constants.EffectOrigin originType)
    {
        int count = effectOrigins.Count;
        for (int i = 0; i < count; i++)
        {
            if (effectOrigins[i].originType == originType)
                return effectOrigins[i].point;
        }

        return null;
    }

   

}
