using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointCounter : MonoBehaviour
{
   #region Variables

   [SerializeField] private TextMeshProUGUI player1Points;
   [SerializeField] private TextMeshProUGUI player2Points;

   [SerializeField] private CanvasGroup player1Panel;
   [SerializeField] private CanvasGroup player2Panel;

   [SerializeField] private int pointsToWin = 10;
   
   private int pointsPlayer1;
   private int pointsPlayer2;
   private int resetPoint = 0;

   #endregion

   #region UnityMethods

   private void Start()
   {
      player1Panel.HideCanvasGroup();
      player2Panel.HideCanvasGroup();
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

      if (pointsPlayer1 >= pointsToWin)
      {
         player1Panel.ShowCanvasGroup();
         Time.timeScale = 0f;
      }

      if (pointsPlayer2 >= pointsToWin)
      {
         player2Panel.ShowCanvasGroup();
         Time.timeScale = 0f;
      }
   }

   public void RestartGame()
   {
      player1Panel.HideCanvasGroup();
      player2Panel.HideCanvasGroup();
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

   public void ToMainMenu()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
      Time.timeScale = 1f;
   }

   public void QuitGame()
   {
      Application.Quit();
   }

   #endregion
}
