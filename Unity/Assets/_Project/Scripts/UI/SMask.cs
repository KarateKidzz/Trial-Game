using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMask : MonoBehaviour
{
    [Range(0.05f, 0.3f)]
    public float flickTime = 0.1f;

    [Range(0.01f, 0.05f)]
    public float addSize = 0.1f;

    public bool followPlayer = false;

    float timer = 0;

    bool bigger = true;

    Transform playerTransform;

    void Start()
    {
        playerTransform = PlayerReference.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > flickTime)
        {
            Vector3 localScale = transform.localScale;

            if (bigger)
                transform.localScale = new Vector3(localScale.x + addSize, localScale.y + addSize, localScale.z);
            else
                transform.localScale = new Vector3(localScale.x - addSize, localScale.y - addSize, localScale.z);

            bigger = !bigger;
            timer = 0;
        }
        if (followPlayer)
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 20 * Time.deltaTime);
    }
}
