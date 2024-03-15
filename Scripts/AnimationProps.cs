using Godot;
using System;

public partial class AnimationProps : AnimationPlayer
{
    [ExportCategory("Animation Clips Names")]
	[Export]
	private StringName _jumpAnimation;

	[Export]
	private StringName _fallAnimation;



	public void PlayJumpAnimation()
	{
		if (!CurrentAnimation.Equals(_jumpAnimation))
		{
			Play(_jumpAnimation);
		}
	}

	public void PlayFallAnimation()
	{
		if (!CurrentAnimation.Equals(_fallAnimation))
		{
			Play(_fallAnimation);
		}
	}
}
