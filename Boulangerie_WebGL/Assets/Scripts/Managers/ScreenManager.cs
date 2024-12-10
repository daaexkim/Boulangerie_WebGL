using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    public AdjustSize bgTrans;
    public AdjustSize ovenTrans;
    public AdjustSize groundTrans;
    public AdjustSize wallTrans_L, wallTrans_R;
    public Transform spawnPoint;
    public Transform lineTrans;

    public CameraBound camBound;
    protected override void Awake()
    {
        base.Awake();

        camBound.SetCameraBound();

        bgTrans.Adjusting(camBound);
        float height = ovenTrans.Adjusting(camBound);
        wallTrans_L.Adjusting_Wall(camBound);
        wallTrans_R.Adjusting_Wall(camBound);
        groundTrans.Adjusting_Ground(camBound);

        bgTrans.transform.position += new Vector3(0, 1.3f);
        ovenTrans.transform.position = new Vector3(0, camBound.Top - 0.69f);
        wallTrans_L.transform.position = new Vector3(camBound.Left - wallTrans_L.transform.localScale.x / 2f, 0);
        wallTrans_R.transform.position = new Vector3(camBound.Right + wallTrans_R.transform.localScale.x / 2f, 0);
        groundTrans.transform.position = new Vector3(0, camBound.Bottom - groundTrans.transform.localScale.y / 2f + 1.3f);
        spawnPoint.transform.position = new Vector3(0, camBound.Top - 0.5f);
        lineTrans.transform.localPosition = new Vector3(0, -height);
    }
}

[System.Serializable]
public class CameraBound
{
    public Camera camera;

    [HideInInspector] public float size_x, size_y;

    public void SetCameraBound()
    {
        camera = Camera.main;

        size_y = camera.orthographicSize;
        size_x = camera.orthographicSize * Screen.width / Screen.height;
    }

    public float Bottom
    {
        get
        {
            return size_y * -1 + camera.gameObject.transform.position.y;
        }
    }

    public float Top
    {
        get
        {
            return size_y + camera.gameObject.transform.position.y;
        }
    }

    public float Left
    {
        get
        {
            return size_x * -1 + camera.gameObject.transform.position.x;
        }
    }

    public float Right
    {
        get
        {
            return size_x + camera.gameObject.transform.position.x;
        }
    }

    public float Height
    {
        get
        {
            return size_y * 2;
        }
    }

    public float Width
    {
        get
        {
            return size_x * 2;
        }
    }
}