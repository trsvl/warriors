using UnityEngine;

namespace _Project.Scripts.Gameplay.Troops
{
    public class TroopBase : PoolBase, ITroop, IDamageable
    {
        public Transform ClosestTarget
        {
            get
            {
                if (closestTarget && closestTarget.gameObject.activeSelf)
                {
                    return closestTarget;
                }

                return null;
            }
            set => closestTarget = value;
        }

        public new TroopBaseConfig Config
        {
            get => (TroopBaseConfig)base.Config;
            set => base.Config = value;
        }

        public Team Team { get; private set; }
        public string OpponentTag { get; private set; }
        public Rigidbody2D Rb { get; private set; }
        public IAnimationListener AnimationListener { get; set; }

        private Transform closestTarget;
        private float health;
        private StateMachine _stateMachine;


        protected void Create(Rigidbody2D rb, StateMachine stateMachine)
        {
            Rb = rb;
            _stateMachine = stateMachine;
        }

        public void Spawn(Team team)
        {
            Team = team;
            gameObject.tag = team.ToString();
            OpponentTag = (Team == Team.Player ? Team.Enemy : Team.Player).ToString();
            closestTarget = null;
            health = Config.MaxHealth;
            CheckRotation();
        }

        protected virtual void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Factory.Instance.ReturnToPool(this);
            }
        }

        public void CheckRotation()
        {
            if (ClosestTarget && transform.position.x > ClosestTarget.position.x)
            {
                gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else if (ClosestTarget && transform.position.x < ClosestTarget.position.x)
            {
                gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0f, Team == Team.Player ? 0f : 180f, 0f);
            }
        }
    }
}