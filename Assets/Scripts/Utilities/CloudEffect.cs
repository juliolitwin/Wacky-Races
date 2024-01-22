using UnityEngine;

public class CloudEffect : MonoBehaviour
{
    public float _speed = 2;
    public GameObject[] _clouds;

    private void Start()
    {
        for (var i = 0; i < _clouds.Length; i++)
        {
            var halfHeight = Camera.main.orthographicSize;
            var yPosition = Random.Range(halfHeight / 2, halfHeight);
            _clouds[i].transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - i * 10, yPosition, 0);
        }
    }

    private void Update()
    {
        for (var i = 0; i < _clouds.Length; i++)
        {
            var cloud = _clouds[i];
            cloud.transform.Translate(_speed * Time.deltaTime, 0, 0);

            if (cloud.transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x)
            {
                var halfHeight = Camera.main.orthographicSize;
                var yPosition = Random.Range(halfHeight / 2, halfHeight);
                cloud.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - i * 10, yPosition, 0);
            }
        }
    }
}