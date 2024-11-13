using System;
using TMPro;
using UnityEngine;

public class PointCounter : MonoBehaviour
{
   #region Variables

   [SerializeField] private TextMeshProUGUI player1Points;
   [SerializeField] private TextMeshProUGUI player2Points;

   [SerializeField] private CanvasGroup winPanel;
   
   private int pointsPlayer1;
   private int pointsPlayer2;
   private int resetPoint = 0;

   #endregion

   #region UnityMethods

   private void Start()
   {
      winPanel.HideCanvasGroup();
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

      if (pointsPlayer1 >= 10 || pointsPlayer2 >= 10)
      {
         winPanel.ShowCanvasGroup();
         Time.timeScale = 0f;
      }
   }

   public void RestartGame()
   {
      winPanel.HideCanvasGroup();
      ResetPoints();
      Time.timeScale = 1f;
   }

   private void ResetPoints()
   {
      pointsPlayer1 = resetPoint;
      pointsPlayer2 = resetPoint;
      player1Points.text = pointsPlayer1.ToString();
      player2Points.text = pointsPlayer2.ToString();
   }

   #endregion
}
