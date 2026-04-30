using UnityEngine;

namespace Pear.Demo
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField]
        private Vector3 rotationSpeed = new Vector3(0f, 30f, 0f);

        private void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}