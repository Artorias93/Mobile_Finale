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
                 
                scoreText.text = gm.ErrorCount + "/" + gm.GameParameters.i_maxError;

                if (gm.GameFinished)
                {
                    endGameScreenController.showEndGamePanel(gm.Score);
                }
            }
        }
    }
}

