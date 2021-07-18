using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeThing : MonoBehaviour
{
    private SpriteShapeController shape;
    private EdgeCollider2D col;
    public Oscilator osc;
    private PointEffector2D affector;
    public Rotator rider;
    public int maxPoints = 10;

    public int step = 1;
    public float speed = 1f;

    Coroutine Stepper;

    // Start is called before the first frame update
    void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        //affector = GetComponentInChildren<PointEffector2D>();
        col = shape.edgeCollider;
        osc.transform.localPosition = new Vector3(5,5,0); //shape.spline.GetPosition(shape.spline.GetPointCount() - 1);
        Stepper = StartCoroutine(Timer(step));

    }

    // Update is called once per frame
    void Update()
    {
        if (Stepper == null)
            Stepper = StartCoroutine(Timer(step));
        Move();
    }

    void Move(){
        osc.transform.position = new Vector3{
            x = osc.transform.position.x + speed * Time.deltaTime,
            y = osc.transform.position.y,
            z = osc.transform.position.z
        };
    }

    IEnumerator Timer(int seconds){
        while (step == seconds){
            //Debug.Log(Time.time);
            yield return new WaitForSeconds(seconds);
            //PlaceAffector();
            AddPoint();
            if (shape.spline.GetPointCount() > maxPoints){
                RemovePoints();
            }
        }
        Stepper = null;
        yield return null;
    }

    void AddPoint(){
        shape.spline.InsertPointAt(shape.spline.GetPointCount(), osc.transform.position);
    }

    void RemovePoints(){
        while (shape.spline.GetPointCount() > maxPoints && shape.spline.GetPointCount() > 2)
            shape.spline.RemovePointAt(0);
    }

    void PlaceAffector(){
        // place affector on a lerp between rider and oscilator
        Vector3 pos = new Vector3();
        Vector3 oscPos = osc.transform.position;
        Vector3 ridePos = rider.transform.position;
        pos.x = Mathf.Lerp(ridePos.x, oscPos.x , ridePos.x / (ridePos.x + oscPos.x));
        RaycastHit2D a = Physics2D.Raycast(new Vector2(pos.x, osc.amplitude), Vector2.down);
        pos.y = a.point.y + rider.transform.localScale.y;
        affector.transform.position = pos; // shape.spline.GetPosition(shape.spline.GetPointCount() - 1);
}
}
