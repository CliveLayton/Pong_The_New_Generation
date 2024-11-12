using System;
using TMPro;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
   #region Variables

   [SerializeField] private TextMeshProUGUI player1Points;
   [SerializeField] private TextMeshProUGUI player2Points;

   private int pointsPlayer1;
   private int pointsPlayer2;
   private int resetPoint = 0;

   #endregion

   #region UnityMethods

   private void Start()
   {
      player1Points.text = "0";
      player2Points.text = "0";
      pointsPlayer1 = resetPoint;
      pointsPlayer2 = resetPoint;
   }

   #endregion

   #region Point Counter Methods

   public void CountPointsUp(int player)
   {
      if (player == 1)
      {
         pointsPlayer1 += 1;
         player1Points.text = pointsPlayer1.ToString();
      }
      else if (player == 2)
      {
         pointsPlayer2 += 1;
         player2Points.text = pointsPlayer2.ToString();
      }
   }

   #endregion
}
