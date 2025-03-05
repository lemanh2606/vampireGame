using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitude;
    public Vector3 direction;
    Vector3 initialPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPosition + direction *(Mathf.Sin(Time.deltaTime*frequency)*magnitude);
    }
}
