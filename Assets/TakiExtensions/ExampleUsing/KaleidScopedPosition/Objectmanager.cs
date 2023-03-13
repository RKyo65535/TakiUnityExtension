using TakiExtension;
using UnityEngine;

public class Objectmanager : MonoBehaviour
{
    //最初に配置され、他のオブジェクトの元ともなるオブジェクトたち
    [SerializeField] CircleMove[] originalObjects;

    //コピー元のオブジェクト
    [SerializeField] GameObject copySourceObject;

    [SerializeField] GameObject lineObject;

    //いま世界にあるオブジェクトのTransform
    Transform[,][] objects;

    [SerializeField] float radius = 2;
    [SerializeField] int xRange = 10;
    [SerializeField] int yRange = 10;

    // Start is called before the first frame update
    void Start()
    {

        Transform thisTF = transform;

        objects = new Transform[xRange * 2 + 1, yRange * 2 + 1][];
        for (int x = -xRange; x <= xRange; x++)
        {
            for (int y = -yRange; y <= yRange; y++)
            {

                Debug.Log("x="+x+",y="+y+"における座標は" + Vector3.zero.GetKaleidScopedPosition(x, y, radius).ToString());
                var obj = Instantiate(lineObject, Vector3.zero.GetKaleidScopedPosition(x, y,radius), Quaternion.identity);
                if (Mathf.Abs(x + y) % 2 == 1)
                {
                    obj.transform.localScale = new Vector3(1, -1, 1); 
                }

                objects[x + xRange, y + yRange] = new Transform[originalObjects.Length];
                for (int i = 0; i < objects[x + xRange, y + yRange].Length; i++)
                {
                    objects[x + xRange, y + yRange][i] = Instantiate(copySourceObject, thisTF).transform;
                    objects[x + xRange, y + yRange][i].GetComponent<SpriteRenderer>().color =
                        originalObjects[i].gameObject.GetComponent<SpriteRenderer>().color;
                    objects[x + xRange, y + yRange][i].localScale = originalObjects[i].TF.localScale;

                    objects[x + xRange, y + yRange][i].position = originalObjects[i].TF.position.GetKaleidScopedPosition(x, y, radius);
                }
            }
        }



    }

    // Update is called once per frame
    void FixedUpdate()
    {

        foreach (CircleMove cm in originalObjects)
        {
            cm.Move();
        }
        for (int i = 0; i < objects[0, 0].Length; i++)
        {
            if (originalObjects[i].TF.position.IsInTriangle(radius))
            {
                originalObjects[i].gameObject.SetActive(true);
            }
            else
            {
                originalObjects[i].gameObject.SetActive(false);

            }

            for (int x = -xRange; x <= xRange; x++)
            {
                for (int y = -yRange; y <= yRange; y++)
                {
                    if (originalObjects[i].gameObject.activeSelf)
                    {
                        objects[x + xRange, y + yRange][i].gameObject.SetActive(true);
                        objects[x + xRange, y + yRange][i].position = originalObjects[i].TF.position.GetKaleidScopedPosition(x, y, radius);
                    }
                    else
                    {
                        objects[x + xRange, y + yRange][i].gameObject.SetActive(false);

                    }

                }
            }
        }
    }
}
