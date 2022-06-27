namespace FruitNinjaMinigame
{
    public class UIManager : AUIManager
    {
        protected override void LateUpdate()
        {
            GameManager gm = GameManager.Instance;
            if (gm != null)
            {
                float normalizedTime = (float) gm.RemainingTime / gm.GameParameters.i_playTime;
                UpdateSlider(normalizedTime);

                scoreText.text = gm.OutCount + "/" + gm.GameParameters.i_maxError;

                if (gm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}