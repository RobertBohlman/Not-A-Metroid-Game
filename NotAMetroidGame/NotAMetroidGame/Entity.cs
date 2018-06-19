using System;

public class Entity {
    //The entity's current position
    private Vector2 position;

    //See note for draw function
    private Texture2D sprite;

    public Entity() {
	}

    /** Draw the entity on screen.
     * 
     *  The draw function will use the sprite field 
     *  and other necessary calls to draw this entity 
     *  on screen at its current position
     *  
     *  [NOTE]: While using a Texture2D allows us to draw a static image to the screen, 
     *  I'm not sure if there's a better way to draw frames of animation (there probably is)
     *  As a result, this will probably get revised slightly.
     **/
    public void Draw() {

    }
}
