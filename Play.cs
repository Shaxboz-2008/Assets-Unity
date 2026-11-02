using UnityEngine;

public class PlayButton : MenuButton
{
    public MenuController menuController;

    public override void Activate()
    {
        if (menuController != null)
            menuController.StartGame();
    }
}