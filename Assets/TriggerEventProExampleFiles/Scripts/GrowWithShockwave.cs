using UnityEngine;
using System.Collections;

namespace PathologicalGames
{

[RequireComponent(typeof(EventTrigger))]
public class GrowWithShockwave : MonoBehaviour
{
    public EventTrigger eventTrigger;
    protected Transform xform;

    protected void Awake()
    {
        this.xform = this.transform;
    }

    void Update()
    {
        Vector3 scl = this.eventTrigger.range * 2.1f; // Let geo move ahead of real range
        scl.y *= 0.2f; // More cenematic hieght.
        this.xform.localScale = scl;

        // Blend the alpha channel of the color off as the range expands.
        Color col = this.renderer.material.GetColor("_TintColor");
        col.a = Mathf.Lerp(0.7f, 0, this.eventTrigger.range.x / this.eventTrigger.endRange.x);
        this.renderer.material.SetColor("_TintColor", col);
    }
}
	
}