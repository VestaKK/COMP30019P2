public class MeleeAttackInfo : AttackInfo
{
    private float _duration;
    public AttackInfo(float damage, float duration, float range, Vector3 offset)
         : base(damage, range, offset)
    {
        this._duration = duration;
    }

    public float Duration { get => _duration; }

}