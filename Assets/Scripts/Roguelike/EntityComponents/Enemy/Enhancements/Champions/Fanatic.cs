/* Fanatic champions in D2 have 100% increased speed and -70% attack rating. We implement the speed increase,
 * but change the accuracy drop to a flat -30.*/

namespace AKSaigyouji.Roguelike
{
    public sealed class Fanatic : ChampionBuff
    {
        public override string DisplayName { get { return "Fanatic"; } }

        public override int EnhanceAccuracy(int accuracy)
        {
            return base.EnhanceAccuracy(accuracy) - 30;
        }

        public override float EnhanceAttackSpeed(float speed)
        {
            return base.EnhanceAttackSpeed(speed) / 2;
        }

        public override float EnhanceMoveSpeed(float speed)
        {
            return base.EnhanceMoveSpeed(speed) / 2;
        }
    } 
}