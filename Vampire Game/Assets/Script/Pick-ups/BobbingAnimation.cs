using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitude;
    public Vector3 direction;
    Vector3 initialPosition;
    Pickup pickup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pickup = GetComponent<Pickup>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       if(pickup && !pickup.hasBeenCollected)
        {
            transform.position = initialPosition + direction * (Mathf.Sin(Time.deltaTime * frequency) * magnitude);
        }
      
    }
}

