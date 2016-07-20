using UnityEngine;

public class PassThroughTest : MonoBehaviour
{

    public Transform start;
    public Transform end;

    public Transform squarePrefab;
    public Transform circlePrefab;

    // Use this for initialization
    void Start()
    {
        Vector3 disp = end.position - start.position;

        float zRot = Mathf.Atan2(disp.y, disp.x) * Mathf.Rad2Deg;

        Transform line = Instantiate(squarePrefab, start.position + disp / 2f, Quaternion.Euler(0, 0, zRot)) as Transform;
        Vector3 scale = line.localScale;
        scale.x = disp.magnitude + 1f;
        line.localScale = scale;

        SpriteRenderer sr = line.GetComponent<SpriteRenderer>();
        Color color = sr.color;
        color.a = 0.35f;
        sr.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
