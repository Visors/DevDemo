using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Marble
{
    [RequireComponent(typeof(GameObjectPool))]
    public class MarbleController : MonoBehaviourSingleton<MarbleController>
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

        [SerializeField, Range(0f, 10f)] private float spawnInterval = 2f;

        [Header("Marble Mover")] [SerializeField, Range(0f, 10f)]
        private float marbleSpeed = 2f;

        private Vector3 RandomInitDirection()
        {
            var ret = new Vector3(Random.Range(0, 2) == 0 ? -1 : 1, Random.Range(-1f, 1f), 0);
            return ret;
        }

        private void Spawn()
        {
            if (!startSpawner) return;
            var marble = _pool.Get();
            Debug.Log("Spawn " + marble.GetInstanceID());
            _marbleToSpeed[marble] = RandomInitDirection() * marbleSpeed;
            marble.GetComponent<Rigidbody>().velocity = _marbleToSpeed[marble];
            marbles.Add(marble);
        }

        class MarbleMover : MonoBehaviourSingleton<MarbleMover>
        {
            private MarbleController _marbleController = MarbleController.Instance;
        }

        private void Awake()
        {
            _marbleToSpeed = new Dictionary<GameObject, Vector3>();
            _pool = GetComponent<GameObjectPool>();
            _pool.InitSize = initPoolSize;
            _pool.MaxSize = maxPoolSize;
            _pool.Prefab = marblePrefab;
            _pool.Warm();
        }

        private void Start()
        {
            InvokeRepeating(nameof(Spawn), 0, spawnInterval);
        }

        private void Update()
        {
        }
    }
}