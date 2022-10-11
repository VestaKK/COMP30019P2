
/**
Generic AttackInfo class to be used to encapsulate damage data for different entities
*/
public class AttackInfo 
{
    private float _damage;
    private float _range;
    private Vector3 _offset;
    public AttackInfo(float damage, float range, Vector3 offset) 
    {
        this._damage = damage;
        this._range = range;
        this._offset = offset;
    }

    public float Damage { get => _damage; }
    public float Range { get => _range; }
    public Vector3 Offset { get => _offset; }

}