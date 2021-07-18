using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UserStoredData{
    [SerializeField]
    public string name;
    [SerializeField]
    public float miles;
}
public class User : MonoBehaviour
{
    public UserStoredData data;
    private float speed = 3.0f; // in m/ph
    private float milesPerSec;
    public bool isWalking;
    public Rotator rider;

    private float SetMPH(float speed){
        return speed / 3600;
    }

    private void Awake() {
        isWalking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWalking){
            data.miles += /*milesPerSec*/ SetMPH(rider.GetComponent<Rigidbody2D>().velocity.x) * Time.fixedDeltaTime;
        }
    }

    public void StartWalking(){
        isWalking = true;
    }
}
