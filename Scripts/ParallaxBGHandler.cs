using Godot;
using Godot.Collections;
using System;

namespace Background;
public partial class ParallaxBGHandler : ParallaxBackground
{
	[Export]
	private GamePlay _gameplayScript;

	[Export]
	Array<ParallaxLayer> _parallaxLayers;

	private float _viewportWidth;

    public override void _Ready()
    {
        base._Ready();

		
		GD.Print(_gameplayScript.ViewportSize);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }



    /// <summary>
    /// Get the sprit2d node in the <c>parallaxLayer</c> and get the scale according to sprite width / viewport width
    /// </summary>
    /// <param name="parallaxLayer">parent parallax layer</param>
    /// <param name="spriteTextureSize">return the texture size to set the mirroring offset</param>
    /// <returns>The correct scale</returns>
    private float GetParallaxSpritScale(ParallaxLayer parallaxLayer, Vector2 viewportSize, out Vector2 spriteTextureSize)
	{
		Sprite2D parallaxSprite = parallaxLayer.FindChild("Sprite2D") as Sprite2D;
		spriteTextureSize = parallaxSprite.Texture.GetSize();

		float correctScale =  viewportSize.X / spriteTextureSize.X;
		parallaxSprite.Scale = new Vector2(correctScale, correctScale);
		
		return correctScale;
	}


	/// <summary>
	/// set the motion mirror vector of the <c>parallaxLayer</c>
	/// </summary>
	/// <param name="parallaxLayer"></param>
	private void SetupParallaxLayer(ParallaxLayer parallaxLayer, Vector2 viewportSize)
	{
		float correctScale = GetParallaxSpritScale(parallaxLayer, viewportSize, out Vector2 spriteSize);
		parallaxLayer.MotionMirroring = new Vector2(0.0f, spriteSize.Y * correctScale);
	}

	/// <summary>
	/// Set up the parallax Background
	/// </summary>
	/// <param name="viewportSize">The view port size</param>
	public void SetupParallaxBG(Vector2 viewportSize)
	{
		foreach (var layer in _parallaxLayers)
		{
			SetupParallaxLayer(layer, viewportSize);
		}
	}
}
