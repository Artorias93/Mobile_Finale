namespace PlaceTheCandles
{
    public class UIManager : AUIManager
    {
        protected override void LateUpdate()
        {
            PlaceCandlesGameManager gm = PlaceCandlesGameManager.Instance;
            if (gm != null)
            {
                float normalizedTime = (float) gm.RemainingTime / gm.GameParameters.i_playTime;
                UpdateSlider(normalizedTime);
                 
                scoreText.text = gm.RunesCount + "/" + gm.GameParameters.i_runeToComplete;

                if (gm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}

