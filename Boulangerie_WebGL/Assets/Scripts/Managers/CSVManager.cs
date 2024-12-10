using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSVManager : Singleton<CSVManager>
{
    public const int NULL = -99999;

    public TextAsset[] textAssets;
    public CSVList csvList = new CSVList();

    protected override void Awake()
    {
        base.Awake();

        CSV_Word();
    }

    public void CSV_Word()
    {
        int order = 0; int size = 4;
        string[] data = textAssets[order].text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int tableSize = data.Length / size - 1;
        csvList.wordDatas = new WordData[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int k = i + 1;
            csvList.wordDatas[i] = new WordData
            {
                word = data[size * k],
                gender = (Gender)Enum.Parse(typeof(Gender), data[size * k + 1]),
                meaning = data[size * k + 2],
                meaning_EN = data[size * k + 3]
            };
        }
    }

    public WordData Export_RanWord()
    {
        int length = csvList.wordDatas.Length;
        int ranNum = UnityEngine.Random.Range(0, length);
        return csvList.wordDatas[ranNum];
    }
}

[Serializable]
public class CSVList
{
    public WordData[] wordDatas;
}

public enum Gender { Masculin, Feminin, Neutral }

[Serializable]
public struct WordData
{
    public string word;
    public Gender gender;
    public string meaning;
    public string meaning_EN;
}