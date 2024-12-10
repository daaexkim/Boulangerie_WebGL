using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using UnityEngine.UI;

public class SpawnManager_mm : Singleton <SpawnManager_mm>
{
    public Sprite[] painImages;
    public float spacing;       // 간격
    [HideInInspector] public int painCol, painRow;

    private void Start()
    {
        // 간격을 기준으로 행과 열 계산
        painCol = Mathf.FloorToInt((Screen.height + spacing * 2) / spacing);
        painRow = Mathf.FloorToInt((Screen.width + spacing * 2) / spacing) + 1;

        Spawn_PainImages();
    }

    private void Spawn_PainImages()
    {
        // 스폰 시작 위치 계산 (가로와 세로 기준 변경)
        float startX = -(painRow - 2) * spacing / 2; // 행을 기준으로 가로 설정
        float startY = -(painCol - 1) * spacing / 2; // 열을 기준으로 세로 설정

        for (int col = 0; col < painCol; col++) // 열 → 세로 반복
        {
            float ranSpeed = Random.Range(150f, 200f);

            for (int row = 0; row < painRow; row++) // 행 → 가로 반복
            {
                // 스폰 위치 계산 (가로와 세로 기준 변경)
                float posX = startX + row * spacing; // 행 → 가로 위치
                float posY = startY + col * spacing; // 열 → 세로 위치

                // 오브젝트 스폰
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
