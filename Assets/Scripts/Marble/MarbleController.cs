using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marble
{
    public class MarbleController : MonoBehaviourSingletonPersistent<MarbleController>
    {
        [Header("Marble")] [SerializeField] private GameObject marblePrefab;

        [SerializeField] private Rect marbleLifeArea;
        [SerializeField] private List<GameObject> marbles;

        private Dictionary<GameObject, Vector3> _marbleToSpeed;


        [Header("Marble Pool")] [SerializeField, Range(0, 50)]
        private int initPoolSize = 10;

        [SerializeField, Range(0, 50)] private int maxPoolSize = 20;
        private GameObjectPool _pool;

        [Header("Marble Spawner")] [SerializeField]
        private bool startSpawner;

        private MarbleSpawner _marbleSpawner;

        [SerializeField, Range(0f, 10f)] private float spawnInterval = 2f;

        [Header("Marble Mover")] [SerializeField, Range(0f, 10f)]
        private float marbleSpeed = 2f;

        class MarbleSpawner : MonoBehaviourSingleton<MarbleSpawner>
        {
            private MarbleController _marbleController = MarbleController.Instance;

            private Vector3 RandomInitDirection()
            {
                var direction2D = Random.insideUnitCircle;
                var ret = new Vector3(direction2D.x, direction2D.y, 0);
                return ret;
            }

            private void Spawn()
            {
                if (!_marbleController.startSpawner) return;
                var marble = _marbleController._pool.Get();
                _marbleController._marbleToSpeed[marble] = RandomInitDirection() * _marbleController.marbleSpeed;
                marble.GetComponent<Rigidbody>().velocity = _marbleController._marbleToSpeed[marble];
                _marbleController.marbles.Add(marble);
            }

            private void Awake()
            {
                Debug.Log("Marble Spawner awake.");
            }

            private void Start()
            {
                InvokeRepeating(nameof(Spawn), 0, 2f);
            }
        }

        class MarbleMover : MonoBehaviourSingleton<MarbleMover>
        {
            private MarbleController _marbleController = MarbleController.Instance;
        }

        private void Awake()
        {
            _pool = ScriptableObject.CreateInstance<GameObjectPool>();
            _pool.InitSize = initPoolSize;
            _pool.MaxSize = maxPoolSize;
            _pool.Prefab = marblePrefab;
            Debug.Log(_pool);
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (startSpawner && _marbleSpawner == null) _marbleSpawner = MarbleSpawner.Instance;
        }
    }
}