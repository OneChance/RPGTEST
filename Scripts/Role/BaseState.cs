public class BaseState
{
	private int _baseValue; //基础值
	private int _buffValue;  //加成值
	private int _expToLevel; //升级技能需要的经验
	private float _levelModifier;//对升级技能需要的经验的倍增

#region Setters and Getters
	public BaseState ()
	{
		_baseValue = 0;
		_buffValue = 0;
		_expToLevel = 100;
		_levelModifier = 1.1f;
	}

	public int BaseValue {
		get{ return _baseValue;}
		set{ _baseValue = value;}
	}

	public int BufferValue {
		get{ return _buffValue;}
		set{ _buffValue = value;}
	}

	public int ExpToLevel {
		get{ return _expToLevel;}
		set{ _expToLevel = value;}
	}

	public float LevelModifier {
		get{ return _levelModifier;}
		set{ _levelModifier = value;}
	}
#endregion

	private int CalculateExpToLevel ()
	{
		return (int)(_expToLevel * _levelModifier);
	}

	public void LevelUp ()
	{
		_expToLevel = CalculateExpToLevel ();
		_baseValue++;
	}

	public int AdjustedValue ()
	{
		return _baseValue + _buffValue;
	}
}
