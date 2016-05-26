using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject dotPrefab;
    public int[] numDots;

    public GameObject ghostPrefab;
    public Color[] ghostColors;

    private Vector3 bottomLeft;
    private Vector3 topRight;
    private Vector3 widthHeight;

    // Use this for initialization
    void Start()
    {
        float distAway = Mathf.Abs(Camera.main.transform.position.y);

        bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distAway));
        topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, distAway));
        widthHeight = topRight - bottomLeft;

        transform.localScale = new Vector3(widthHeight.x, transform.localScale.y, widthHeight.z);

        createDots();

        for(int i = 0; i < ghostColors.Length; i++)
        {
            GameObject go = Instantiate(ghostPrefab, randomPos(), Quaternion.FromToRotation(Vector3.right, randomDir())) as GameObject;
            go.GetComponent<Renderer>().material.color = ghostColors[i];
        }
    }

    private void createDots()
    {
        float num = Random.Range(numDots[0], numDots[1] + 1);
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = randomPos();
            pos.y = dotPrefab.transform.position.y;
            Instantiate(dotPrefab, pos, Quaternion.identity);
        }
    }

    private Vector3 randomPos()
    {
        Vector3 pos = new Vector3();
        pos.x = Random.Range(bottomLeft.x, topRight.x);
        pos.z = Random.Range(bottomLeft.z, topRight.z);
        return pos;
    }

    public static Vector3 randomDir()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        return new Vector3(dir.x, 0, dir.y);
    }

    void OnTriggerStay(Collider other)
    {
        keepInBounds(other);
    }

    void OnTriggerExit(Collider other)
    {
        keepInBounds(other);
    }

    private void keepInBounds(Collider other)
    {
        Transform t = other.transform;

        if (t.position.x < bottomLeft.x)
        {
            t.position = new Vector3(t.position.x + widthHeight.x, t.position.y, t.position.z);
        }

        if (t.position.x > topRight.x)
        {
            t.position = new Vector3(t.position.x - widthHeight.x, t.position.y, t.position.z);
        }

        if (t.position.z < bottomLeft.z)
        {
            t.position = new Vector3(t.position.x, t.position.y, t.position.z + widthHeight.z);
        }

        if (t.position.z > topRight.z)
        {
            t.position = new Vector3(t.position.x, t.position.y, t.position.z - widthHeight.z);
        }
    }
}
