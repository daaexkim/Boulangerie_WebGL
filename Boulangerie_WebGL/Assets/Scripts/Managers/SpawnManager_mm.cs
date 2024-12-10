using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using UnityEngine.UI;

public class SpawnManager_mm : Singleton <SpawnManager_mm>
{
    public Sprite[] painImages;
    public float spacing;       // ����
    [HideInInspector] public int painCol, painRow;

    private void Start()
    {
        // ������ �������� ��� �� ���
        painCol = Mathf.FloorToInt((Screen.height + spacing * 2) / spacing);
        painRow = Mathf.FloorToInt((Screen.width + spacing * 2) / spacing) + 1;

        Spawn_PainImages();
    }

    private void Spawn_PainImages()
    {
        // ���� ���� ��ġ ��� (���ο� ���� ���� ����)
        float startX = -(painRow - 2) * spacing / 2; // ���� �������� ���� ����
        float startY = -(painCol - 1) * spacing / 2; // ���� �������� ���� ����

        for (int col = 0; col < painCol; col++) // �� �� ���� �ݺ�
        {
            float ranSpeed = Random.Range(150f, 200f);

            for (int row = 0; row < painRow; row++) // �� �� ���� �ݺ�
            {
                // ���� ��ġ ��� (���ο� ���� ���� ����)
                float posX = startX + row * spacing; // �� �� ���� ��ġ
                float posY = startY + col * spacing; // �� �� ���� ��ġ

                // ������Ʈ ����
                Spawn_painImage_ran(posX, posY, ranSpeed, startX);
            }
        }
    }

    public void Spawn_painImage_ran(float posX, float posY, float speed, float startX)
    {
        PainImage image = PoolManager.Instance.GetFromPool<PainImage>("Pain_Img");
        image.SetImg(new Vector2(posX, posY), speed, startX);
    }
}
