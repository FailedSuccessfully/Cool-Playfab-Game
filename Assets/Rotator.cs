using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    Rigidbody2D rigid;
    Camera cam;
    public float camOffset = 1f;

    public float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (rigid.velocity.x < speed)
            rigid.AddTorque(-Mathf.Lerp(speed * rigid.mass, rigid.velocity.x, rigid.velocity.x / speed * rigid.mass));
        cam.transform.position = new Vector3{
            x = transform.position.x + camOffset * cam.orthographicSize,
            y = transform.position.y,
            z = cam.transform.position.z
        };
    }
}
