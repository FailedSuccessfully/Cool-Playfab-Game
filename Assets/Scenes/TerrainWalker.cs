using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainWalker : MonoBehaviour
{
    private SpriteShapeController shape;
    private EdgeCollider2D col;
    private Oscilator osc;
    private Rotator rider;
    public int maxPoints = 10;

    public int step = 1;
    public float speed = 1f;
    private bool isRunning = false;

    Coroutine Stepper;

    // Start is called before the first frame update
    void Start()
    {
        shape = GetComponentInChildren<SpriteShapeController>();
        col = shape.edgeCollider;
        osc = GetComponentInChildren<Oscilator>();
        osc.transform.localPosition = shape.spline.GetPosition(shape.spline.GetPointCount() - 1);
        rider = GetComponentInChildren<Rotator>();
        Stepper = StartCoroutine(Timer(step));

    }

    // Update is called once per frame
    void Update()
    {
        if (Stepper == null && shape.spline.GetPointCount() < maxPoints)
            Stepper = StartCoroutine(Timer(step));
        if (isRunning){
            Move();
            osc.Oscilate();
        }
        else{
            RemovePoints();
        }
    }

    void Move(){
        osc.transform.position = new Vector3{
            x = osc.transform.position.x + speed * Time.deltaTime,
            y = osc.transform.position.y,
            z = osc.transform.position.z
        };
    }

    IEnumerator Timer(int seconds){
        isRunning = true;
        while (step == seconds && shape.spline.GetPointCount() < maxPoints){
            //Debug.Log(Time.time);
            yield return new WaitForSeconds(seconds);
            //PlaceAffector();
            AddPoint();
        }
        Stepper = null;
        isRunning = false;
        yield return null;
    }

    void AddPoint(){
        shape.spline.InsertPointAt(shape.spline.GetPointCount(), osc.transform.position);
    }

    void RemovePoints(){
        while (shape.spline.GetPosition(maxPoints/3).x < rider.transform.position.x &&
                shape.spline.GetPointCount() >= maxPoints &&
                shape.spline.GetPointCount() > 2)
            shape.spline.RemovePointAt(0);
    }
}
