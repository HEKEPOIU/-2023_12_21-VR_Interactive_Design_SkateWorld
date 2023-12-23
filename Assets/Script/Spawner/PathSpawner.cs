using UnityEngine;

namespace Spawner
{
    public class PathSpawner : MonoBehaviour
    {
        private Transform[] _pathPoints;
        [SerializeField] private float _spawnDistance = 50f;
        [SerializeField] private GameObject _pathPrefab;
        // Start is called before the first frame update
        void Start()
        {
            _pathPoints = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _pathPoints[i] = transform.GetChild(i);
            }
            Spawn();
        }

        // Update is called once per frame
        void Spawn()
        {
            float pathLength = Vector3.Distance(_pathPoints[0].position, _pathPoints[1].position);
            Vector3 direction = (_pathPoints[1].position - _pathPoints[0].position).normalized;
            int pathCount = Mathf.CeilToInt(pathLength / _spawnDistance);
            for (int i = 0; i < pathCount; i++)
            {
                int range = Random.Range(0, 3);
                if (range == 0)
                {
                    GameObject coins = Instantiate(_pathPrefab, _pathPoints[0].position + direction * _spawnDistance * i, Quaternion.identity);
                    
                }
            }
        }
    }
}
