using Kronos.Worlds;
using Kronos.Minions;

namespace Kronos.Gods
{
    public abstract class God
    {
        public static int Smites { get; set; }
        public abstract Minion Minion { get; }
        public abstract string Name { get; }
        public abstract World World { get; set; }

        public abstract void Contemplate(int casualties, int defiles);
        public abstract void Play();
        public abstract void Smite();

        public BattleShip.Interface.Shot Smote()
        {
            Smites++;

            return new BattleShip.Interface.Shot(Minion.Target.Latitude, Minion.Target.Longitude);
        }
    }
}

