using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Delivers experience to the player upon death. 
    /// </summary>
    public sealed class OnDeathExperience : MonoBehaviour, IOnDeath
    {
        PlayerStats playerStats;

        public int Experience
        {
            get { return experience; }
            set
            {
                if (experience < 0) throw new ArgumentOutOfRangeException("experience");
                experience = value;
            }
        }
        [SerializeField] int experience;

        void Start()
        {
            Assert.IsNotNull(playerStats);
        }

        public void Initialize(PlayerStats playerStats)
        {
            this.playerStats = playerStats;
        }

        public void Invoke()
        {
            playerStats.AddExperience(experience);
        }

        void OnValidate()
        {
            experience = Mathf.Max(0, experience);
        }
    }
}