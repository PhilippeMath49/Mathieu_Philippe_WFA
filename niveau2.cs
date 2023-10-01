using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA
{
    public partial class niveau2 : Form
    {
        //déclaration des booléens pour le joueur
        bool goLeft, goRight, jumping, isGameOver, hasKey;

        //déclaration variables joueur
        int jumpSpeed;
        int force;
        int playerSpeed = 7;
        //déclaration variables plateformes
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        //déclaration variables des ennemies 
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        int enemyThreeSpeed = 4;
        //musique
        SoundPlayer simpleSound;

        //initialise les composants du jeu et lance la musique 
        public niveau2()
        {
            InitializeComponent();
            simpleSound = new SoundPlayer(Properties.Resources.YR);
            simpleSound.PlayLooping();
            RestartGame();
        }
        //Fonction dans laquelle il y a des vérifications grâce au Timer.
        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            
            //attribution de la jumpSpeed
            player.Top += jumpSpeed;
            //vérification des inputs 
            if (goLeft == true)
            {
                player.Left -= playerSpeed;
                player.Image = Properties.Resources.ghostPlayerLeft;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
                player.Image = Properties.Resources.ghostPlayerRight;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }


            //gestion des collisions 
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {


                    // collision joueur/plateformes
                    if ((string)x.Tag == "platform")
                    {
                        // Vérifie si le joueur (player) entre en collision avec la plateforme 
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            // Vérifie si le joueur se trouve au-dessus de la plateforme
                            if (player.Top < x.Top)
                            {
                                // Ajustez la position du joueur uniquement lorsque le joueur est en train de descendre.
                                force = 8;
                                player.Top = x.Location.Y - player.Height + 1;
                                if (!jumping)
                                {
                                    jumpSpeed = 0;
                                    
                                }
                                //si le joueur entre en collision par le bas de la plateforme sa position est replacée sous la plateforme
                            }
                            else if (player.Top > x.Top)
                            {
                                player.Top = x.Location.Y + player.Height + 1;
                            }

                            //permet au joueur de bouger avec la plateforme  en lui attribuant la même vitesse que cette dernière
                            if ((string)x.Name == "horizontalPlatform" && goLeft == false || (string)x.Name == "horizontalPlatform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }


                        }
                        //affiche la plateforme au premier plan pour éviter que l'image du joueur ne passe devant
                        x.BringToFront();

                    }





                }
                //si le joueur touche les spikes il meurt 
                if((string)x.Tag == "spike")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        isGameOver = true;
                    }
                }

                //collisions joueur et pièces
                if ((string)x.Tag == "key")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Hide();
                        hasKey = true;
                        txtScore.Text =  "Ouvrez la porte ";
                    }
                }
                   

                //si le joueur rentre en collision avec un monstre le jeu s'arrête mais si le joueur est plus haut que le monstre il est tué
                if ((string)x.Tag == "enemy" && x.Visible == true)
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (player.Bottom < x.Top + 5)
                        {
                            x.Hide();
                        }
                        else
                        {
                            gameTimer.Stop();
                            isGameOver = true;
                        }

                    }
                }

                if((string)x.Tag == "door" && hasKey == true)
                {

                    
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        txtScore.Text = Environment.NewLine + "Tu as gagné";

                    }
                }
            }





            //si le joueur sort de l'écran de plus de 50 pixels le jeu arrête 
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = Environment.NewLine + "tu es tombé";
            }
            //donne une vitesse a l'ennemi 1
            enemyOne.Left -= enemyOneSpeed;
            //vérifie si l'ennemi est au bout de sa plateforme 
            if (enemyOne.Left < pictureBox5.Left || enemyOne.Left + enemyOne.Width > pictureBox5.Left + pictureBox5.Width)
            {
                //si l'ennemi est au bout de sa plateforme change le sens de direction
                enemyOneSpeed = -enemyOneSpeed;
                //change le sprite en fonction de la direction
                if (enemyOneSpeed < 0)
                {
                    enemyOne.Image = Properties.Resources.ghostRight;
                }
                else
                {
                    enemyOne.Image = Properties.Resources.ghostLeft;
                }
            }
            // comme pour l'ennemi 1
            enemyTwo.Left += enemyTwoSpeed;

            if (enemyTwo.Left < pictureBox2.Left || enemyTwo.Left + enemyTwo.Width > pictureBox2.Left + pictureBox2.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
                if (enemyTwoSpeed < 0)
                {
                    enemyTwo.Image = Properties.Resources.ghostLeft;
                }
                else
                {
                    enemyTwo.Image = Properties.Resources.ghostRight;
                }
            }

            enemythree.Left += -enemyThreeSpeed;
            if (enemythree.Left < pictureBox3.Left || enemythree.Left + enemythree.Width > pictureBox3.Left + pictureBox3.Width)
            {
                //si l'ennemi est au bout de sa plateforme change le sens de direction
                enemyThreeSpeed = - enemyThreeSpeed;
                //change le sprite en fonction de la direction
                if (enemyThreeSpeed < 0)
                {
                    enemythree.Image = Properties.Resources.ghostRight;
                }
                else
                {
                    enemythree.Image = Properties.Resources.ghostLeft;
                }
            }

            horizontalPlatform.Left -= horizontalSpeed;

            if (horizontalPlatform.Left < 0 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlatform.Top += verticalSpeed;

            if (verticalPlatform.Top < 195 || verticalPlatform.Top > 581)
            {
                verticalSpeed = -verticalSpeed;

            }
        }


        //vérification des inputs 
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        

        //ferme l'application quand on clic sur la croix 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        //vérification des inputs 
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }


        }
        //fonction pour relancer le jeu
        private void RestartGame()
        {
            //réinitialise les booléens 
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            hasKey = false;
            
           

       
            //réapparition des pièces 
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }


            // reset the position of player, platform and enemies

            player.Left = 72;
            player.Top = 656;

            enemyOne.Left = 471;
            enemyTwo.Left = 360;
            enemythree.Left = 730;

            horizontalPlatform.Left = 275;
            verticalPlatform.Top = 581;

            //affiche le score
            txtScore.Text = "Attrape la clé ";

            gameTimer.Start();


        }

    }
}