using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using DG.Tweening;
using TMPro;

public class Pain : MonoBehaviour, IPoolObject
{
    SpawnManager sm;
    TranslateManager tm;
    CameraBound camBound;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public SpriteRenderer sr;
    PolygonCollider2D col;
    TextMeshPro tmpro;
    [HideInInspector] public SpriteRenderer faceSr;
    GameManager gm;

    Coroutine curFaceRoutine, lineTriggerRoutine;
    Sprite def_FaceSpirte;
    public float deadTime;
    public bool isBited;
    public Sprite defSprite, biteSprite, burnedSprite, bitedBurnedSprite;
    public WordData wordData;
    public int level;
    public float defScale;
    [HideInInspector] public float LBorder, RBorder;
    public bool isMerge, isDropped;

    public void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");
        sm = SpawnManager.Instance;
        tm = TranslateManager.Instance;
        gm = GameManager.Instance;
        camBound = ScreenManager.Instance.camBound;
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        tmpro = GetComponentInChildren<TextMeshPro>();
        LBorder = camBound.Left + defScale / 2f;
        RBorder = camBound.Right - defScale / 2f;
        faceSr = transform.Find("Face").GetComponent<SpriteRenderer>();

        if (gm.gameMode == GameMode.Bebe)
            tmpro.gameObject.SetActive(false);

        transform.localScale = new Vector2(0.01f, 0.01f);
    }

    public void OnGettingFromPool()
    {
        curFaceRoutine = null;
        isMerge = false;
        transform.rotation = Quaternion.identity;
        def_FaceSpirte = sm.painFace.Export_defSprite_Ran();
        faceSr.sprite = def_FaceSpirte;
        transform.localScale = new Vector2(0.01f, 0.01f);

        Sequence seq = DOTween.Sequence().SetUpdate(true);

        seq.Append(transform.DOScale(new Vector3(defScale, defScale), 0.1f))
            .OnComplete(() =>
            {
                transform.localScale = new Vector3(defScale, defScale);
            });
    }
    private IEnumerator DropRoutine()
    {
        yield return new WaitForSeconds(2f);
        isDropped = true;
    }

    public void SetPain(int _level, WordData data)
    {
        sr.sprite = defSprite;
        level = _level;
        wordData = data;
        tmpro.text = wordData.word;
        if (gm.gameMode == GameMode.Jeune)
            tmpro.color = sm.genderColors[(int)wordData.gender];
        isDropped = false;
        if(gameObject.activeSelf)
            StartCoroutine(DropRoutine());

        Bited(false);
    }

    public void SetFace(PainState state)
    {
        if (curFaceRoutine == null && gameObject.activeSelf)
            curFaceRoutine = StartCoroutine(FaceRoutine(state));
    }
    private IEnumerator FaceRoutine(PainState state)
    {
        switch (state)
        {
            case PainState.Basic:
                faceSr.sprite = def_FaceSpirte;
                break;
            case PainState.Hit:
                if (isDropped)
                    break;
                faceSr.sprite = sm.painFace.hitSprite;
                yield return new WaitForSeconds(0.7f);
                break;
            case PainState.Fall:
                faceSr.sprite = sm.painFace.fallSprite;
                yield return new WaitForSeconds(1.8f);
                break;
            case PainState.Merge:
                faceSr.sprite = sm.painFace.mergeSprite;
                yield return new WaitForSeconds(0.7f);
                break;
        }
        faceSr.sprite = def_FaceSpirte;
        curFaceRoutine = null;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Pain"))
        {
            if (GameManager.Instance.isGameover)
                return;

            Pain other =  collision.gameObject.GetComponent<Pain>();

            if(other.level == level && !isMerge && !other.isMerge)
            {
                if(gm.gameMode == GameMode.Bebe)
                {

                }

                else if (wordData.gender != Gender.Neutral && other.wordData.gender != Gender.Neutral && wordData.gender != other.wordData.gender)
                {
                    return;
                }


                other.isMerge = true;
                isMerge = true;
                other.rigid.simulated = false;
                rigid.simulated = false;

                Vector2 thisTrans = transform.position;
                Vector2 otherTrans = other.transform.position;
                Vector2 middlePos = new Vector2((thisTrans.x + otherTrans.x) / 2f, (thisTrans.y + otherTrans.y) / 2f);

                Sequence mergeSeq = DOTween.Sequence().SetUpdate(true);
                mergeSeq.Append(transform.DOMove(middlePos, 0.1f));
                mergeSeq.Join(other.transform.DOMove(middlePos, 0.1f));
                mergeSeq.OnComplete(() =>
                {
                    // Done Merge
                    sm.Destroy_Pain(level, this);
                    sm.Destroy_Pain(level, other);

                    sm.Merge_Pain(++level, middlePos);

                    if(gm.gameMode == GameMode.Bebe)
                        sm.Spawn_Effect(defScale + 0.2f, middlePos, Color.white);
                    else if (wordData.gender == Gender.Neutral || other.wordData.gender == Gender.Neutral)
                        sm.Spawn_Effect(defScale + 0.2f, middlePos, sm.genderColors[(int)Gender.Neutral]);
                    else
                        sm.Spawn_Effect(defScale + 0.2f, middlePos, sm.genderColors[(int)wordData.gender]);
                });
            }
            else
            {
                SetFace(PainState.Hit);
                other.SetFace(PainState.Hit);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))
        {
            if(lineTriggerRoutine != null)
            {
                StopCoroutine(lineTriggerRoutine);
            }
            lineTriggerRoutine = StartCoroutine(TriggerRoutine());
        }
    }

    private IEnumerator TriggerRoutine()
    {
        while (true)
        {
            deadTime += Time.deltaTime;

            if (deadTime > 1.8f)
            {
                GameManager.Instance.GameOver(this);
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))
        {
            deadTime = 0;
            StopCoroutine(lineTriggerRoutine);
        }
    }

    public void Bited(bool isB)
    {
        if(isB)
        {
            sm.Spawn_Effect(defScale + 0.2f, transform.position, sm.genderColors[(int)wordData.gender]);
            isBited = true;
            sr.sprite = biteSprite;
            ResetPolygonColliderToSprite();
        }
        else
        {
            isBited = false;
            sr.sprite = defSprite;
            ResetPolygonColliderToSprite();
        }
    }
    public void Burned()
    {
        sr.sprite = isBited ? bitedBurnedSprite : burnedSprite;
        faceSr.sprite = sm.painFace.fallSprite;
    }
    private void ResetPolygonColliderToSprite()
    {
        if (sr.sprite == null)
        {
            return;
        }

        // ���� Sprite�� Physics Shape �����͸� ������ �ݶ��̴��� �ʱ�ȭ
        col.pathCount = sr.sprite.GetPhysicsShapeCount();
        for (int i = 0; i < col.pathCount; i++)
        {
            var shape = new List<Vector2>();
            sr.sprite.GetPhysicsShape(i, shape);
            col.SetPath(i, shape.ToArray());
        }
    }

    public void ResetTranslation()
    {
        tmpro.font = tm.font_fr;
        tmpro.text = wordData.word;
    }
    public void Translation()
    {
        switch (gm.curCountry)
        {
            case Country.ko:
                tmpro.font = tm.font_ko;
                tmpro.text = wordData.meaning;
                break;
            case Country.en:
                tmpro.font = tm.font_en;
                tmpro.text = wordData.meaning_EN;
                break;
        }
    }
}

public enum PainState
{
    Basic, Hit, Fall, Merge 
}