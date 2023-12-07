using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Door door;

    private void OnTriggerStay(Collider other)
    {
        push = true;
    }

    private void OnTriggerExit(Collider other)
    {
        push = false;
    }

    void Interact(bool open)
    {
        door.Interact(open);

        if (open != lastImpulse)
        {
            SpawnImpulse();
            lastImpulse = open;
        }
    }

    bool push = false;
    Vector3 basePos;
    Vector3 pushPos;
    public Vector3 pushOffset = new Vector3(0, -1f, 0);
    float currentT = 0;
    public float pushSpeed = 1;

    private void Start()
    {
        basePos = transform.position;
        pushPos = basePos + pushOffset;
    }

    private void FixedUpdate()
    {
        transform.position = LerpVector3(basePos, pushPos, currentT);

        currentT = Mathf.Clamp(push ? currentT + (Time.fixedDeltaTime * pushSpeed) : currentT - (Time.fixedDeltaTime * pushSpeed), 0, 1);

        Interact(currentT >= 1);
    }

    public static Vector3 LerpVector3(Vector3 a, Vector3 b, float p)
    {
        float x = Mathf.Lerp(a.x, b.x, p);
        float y = Mathf.Lerp(a.y, b.y, p);
        float z = Mathf.Lerp(a.z, b.z, p);
        return new Vector3(x, y, z);
    }

    public Object soundwaveObject;
    public float impulseStrength = 4f;
    public float returnT = 5f;
    public bool lastImpulse = false;

    private void SpawnImpulse()
    {
        GameObject GO = Instantiate(soundwaveObject, transform.position, Quaternion.identity) as GameObject;

        Soundwave soundwaveScript = GO.GetComponent<Soundwave>();
        soundwaveScript.Impulse(impulseStrength, returnT);
    }
}