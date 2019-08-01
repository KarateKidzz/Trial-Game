using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerAttackBar : MonoBehaviour
{
    RectTransform RectTransform;
    RectTransform ParentRectTranform;

    Image Image;
    Outline Outline;

    public float MoveSpeed = 3;
    public bool Move = true;

    public float FlashSpeed = 0.5f;
    float elapsedTime;
    int dir = 1;

    public UnityEvent OnSubmit, OnReachEnd;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        ParentRectTranform = transform.parent.GetComponent<RectTransform>();
        Image = GetComponent<Image>();
        Outline = GetComponent<Outline>();
    }

    void OnEnable()
    {
        RectTransform.anchoredPosition = new Vector2(-(ParentRectTranform.rect.width / 2), 0);
        Move = true;
        elapsedTime = 0;
    }

    void Update()
    {
        if (Move)
        {
            Vector2 pos = RectTransform.anchoredPosition;
            pos.x += MoveSpeed * Time.deltaTime;
            RectTransform.anchoredPosition = pos;

            if (pos.x >= ParentRectTranform.rect.width / 2)
            {
                Move = false;
                OnReachEnd.Invoke();
            }
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= FlashSpeed)
            {
                elapsedTime = 0;
                dir *= -1;
                Image.color = dir == 1 ? Color.white : Color.black;
                Outline.effectColor = dir == 1 ? Color.black : Color.white;
            }

            if (Input.GetButtonDown("Submit"))
            {
                OnSubmit.Invoke();
            }
        }
    }

    public void GetDistances (out float parentWidth, out float currentX)
    {
        parentWidth = ParentRectTranform.rect.width;
        currentX = RectTransform.anchoredPosition.x;
    }
}
