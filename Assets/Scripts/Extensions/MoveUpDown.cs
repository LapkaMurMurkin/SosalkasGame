using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public class MoveUpDown : MonoBehaviour
    {
        public float speed ;          
        public float distance ;       

        private Vector3 startPos;
        private Vector3 targetPos;

        void Start()
        {
            startPos = transform.position;
            targetPos = startPos - new Vector3(0, distance, 0);
        }

        void Update()
        {
            // Двигаемся вниз
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            // Если дошли до нижней точки — моментально возвращаем в начало
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                transform.position = startPos;
            }
        }
    }
    
}