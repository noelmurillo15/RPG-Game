// Allan Murillo : Unity RPG Core Test Project

namespace RPG {

    public interface IDamageable {


        void AdjustHealth(float dmg);
        void StatChange(BuffType buff, float statAmt);
    }
}