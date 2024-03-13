using Godot;
using InputActionNames;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	private float _speed;
	private Vector2 _movmentDirection;


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		MovePlayer();
    }


	/// <summary>
	/// Take the input and set the direction of the movment
	/// and steadly slow down the player to a halt when thers's no input
	/// </summary>
	private void MovePlayer()
	{
		float easeToHalt = 10.0f;
		float direction = Input.GetAxis(ActionName.MOVE_LEFT, ActionName.MOVE_RIGHT);

		if (direction != 0)
		{
			_movmentDirection = new Vector2(direction * _speed, Velocity.Y);
			Velocity = _movmentDirection;
		}
		else
		{
			_movmentDirection = new Vector2(0.0f, Velocity.Y);
			Velocity = Velocity.MoveToward(_movmentDirection, _speed / easeToHalt);
		}

		GD.Print(direction);
		GD.Print(_movmentDirection);

		MoveAndSlide();
	}
}
