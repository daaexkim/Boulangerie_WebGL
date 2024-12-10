using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : Singleton<TouchManager>
{
    [HideInInspector] public bool isTouching; // 롱클릭 체크

    public void TouchDown()
    {
        if (isTouching || GameManager.Instance.isGameover)
            return;

        isTouching = true;
    }

    public void TouchUp()
    {
        if (!isTouching || GameManager.Instance.isGameover)
            return;

        isTouching = false;

        SpawnManager sm = SpawnManager.Instance;

        if (sm.newPain == null)
            return;

        // SoundManager.Instance.SFXPlay(SFXType.Drop);
        sm.painList.Add(sm.newPain);

        sm.newPain.rigid.simulated = true;
        sm.newPain.SetFace(PainState.Fall);
        sm.newPain = null;
        sm.curSpawnCool = 0f;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !isTouching && !EventSystem.current.IsPointerOverGameObject() && !GameManager.Instance.isGameover)
        {
            TouchDown();
        }
        else if (Input.GetMouseButtonUp(0) && isTouching && !GameManager.Instance.isGameover)
        {
            TouchUp();
        }

#elif UNITY_WEBGL || UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (!IsTouchValid(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !isTouching && !GameManager.Instance.isGameover)
            {
                TouchDown();
            }
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isTouching && !GameManager.Instance.isGameover)
            {
                TouchUp();
            }
        }
#endif
    }

    bool IsTouchValid(int fingerId)
    {
        // 현재 터치 이외의 터치가 발생 중인지 확인
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId != fingerId)
            {
                // 다른 터치가 발생 중이면 현재 터치를 무시
                return false;
            }
        }
        // 현재 터치만 발생 중이면 유효
        return true;
    }
}