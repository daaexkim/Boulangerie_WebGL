using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : Singleton<TouchManager>
{
    [HideInInspector] public bool isTouching; // ��Ŭ�� üũ

    public void TouchDown()
    {
        if (isTouching || GameManager.Instance.isGameover || BtnManager.Instance.isIteming)
            return;

        isTouching = true;
    }

    public void TouchUp()
    {
        if (!isTouching || GameManager.Instance.isGameover || BtnManager.Instance.isIteming)
            return;

        isTouching = false;

        StartCoroutine(DropRoutine());
    }

    private IEnumerator DropRoutine()
    {
        SpawnManager sm = SpawnManager.Instance;

        if (sm.newPain == null)
            yield break;

        // SoundManager.Instance.SFXPlay(SFXType.Drop);
        sm.painList.Add(sm.newPain);

        sm.newPain.rigid.simulated = true;
        sm.newPain.SetFace(PainState.Fall);
        sm.newPain = null;

        yield return new WaitForSeconds(sm.maxSpawnCool);

        sm.newPain = sm.Spawn_Pain_Ran();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !isTouching && !EventSystem.current.IsPointerOverGameObject() && !GameManager.Instance.isGameover && !BtnManager.Instance.isIteming)
        {
            TouchDown();
        }
        else if (Input.GetMouseButtonUp(0) && isTouching && !GameManager.Instance.isGameover && !BtnManager.Instance.isIteming)
        {
            TouchUp();
        }

#elif UNITY_WEBGL || UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (!IsTouchValid(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) && !isTouching && !GameManager.Instance.isGameover && !BtnManager.Instance.isIteming)
            {
                TouchDown();
            }
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && isTouching && !GameManager.Instance.isGameover && !BtnManager.Instance.isIteming)
            {
                TouchUp();
            }
        }
#endif
    }

    bool IsTouchValid(int fingerId)
    {
        // ���� ��ġ �̿��� ��ġ�� �߻� ������ Ȯ��
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).fingerId != fingerId)
            {
                // �ٸ� ��ġ�� �߻� ���̸� ���� ��ġ�� ����
                return false;
            }
        }
        // ���� ��ġ�� �߻� ���̸� ��ȿ
        return true;
    }
}