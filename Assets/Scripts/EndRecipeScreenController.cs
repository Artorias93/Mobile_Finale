using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EndRecipeScreenController : EndGameScreenController
{
    private void Awake()
    {
        Assert.AreEqual<int>(GO_stars.Length, 3, "Stars missing");

        for (int i = 0; i < GO_stars.Length; i++)
            hideStar(i);
    }

}
