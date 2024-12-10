using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSize : MonoBehaviour
{
    private Vector2 baseScale = new Vector2(0.79f, 0.79f);

    public float Adjusting(CameraBound camBound)
    {
        transform.localScale = new Vector3(camBound.Width / 4.620853f * baseScale.x, camBound.Height / 10f * baseScale.y, 1f);
        return camBound.Height / 10f * baseScale.y;
    }
    public void Adjusting_Wall(CameraBound camBound)
    {
        transform.localScale = new Vector3(2f, camBound.Height * 1.5f, 1f);
    }
    public void Adjusting_Ground(CameraBound camBound)
    {
        transform.localScale = new Vector3(camBound.Width * 1.5f, 2f, 1f);
    }
}
