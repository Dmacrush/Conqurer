using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class EnemyManager : MonoBehaviour
    {


        public List<EnemyTarget> enemyTargets = new List<EnemyTarget>();
        public List<BossTarget> BossTarget = new List<BossTarget>();


        public EnemyTarget GetEnemy(Vector3 from)
        {
            EnemyTarget r = null;
            float MinDistance = float.MaxValue;

            for (int i = 0; i < enemyTargets.Count; i++)
            {

                float tDis = Vector3.Distance(from, enemyTargets[i].GetTarget().transform.position  );
                if (tDis < MinDistance)
                {

                    MinDistance = tDis;
                r = enemyTargets[i];
                }

            }

            return r;
        }





        public static EnemyManager Singleton;
        



        void Awake()
        {
            Singleton = this;
        }




    }
}