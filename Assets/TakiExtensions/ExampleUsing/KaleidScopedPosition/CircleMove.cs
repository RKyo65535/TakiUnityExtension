using UnityEngine;

namespace TakiExtensions.ExampleUsing.KaleidScopedPosition
{
    public class CircleMove : MonoBehaviour
    {

        public Transform TF;
        float rotate;
        int angularVelocity;

        // Use this for initialization
        void Awake()
        {
            TF = transform;
            rotate = Random.Range(0, 360f);
            angularVelocity = Random.Range(2, 9);
        }

        // Update is called once per frame
        public void Move()
        {
            rotate = (rotate + angularVelocity) % 360;

            float angle = rotate * Mathf.Deg2Rad;

            TF.position += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 0.2f;

        }
    }
}
