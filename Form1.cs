﻿using System;
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
    public partial class Form1 : Form
    {
        //déclaration des booléens pour le joueur
        bool goLeft, goRight, jumping, isGameOver;

        //déclaration variables joueur
        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;
        //déclaration variables plateformes
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        //déclaration variables des ennemies 
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        //musique
        SoundPlayer simpleSound;

        //initialise les composants du jeu et lance la musique 
        public Form1()
        {
            InitializeComponent();
            simpleSound = new SoundPlayer(Properties.Resources.YR);
            simpleSound.PlayLooping();
        }
        //Fonction dans laquelle il y a des vérifications grâce au Timer.
        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            //affiche le score
            txtScore.Text = "Score: " + score;
            //attribution de la jumpSpeed
            player.Top += jumpSpeed;
            //vérification des inputs 
            if (goLeft == true)
            {
                player.Left -= playerSpeed;
                player.Image=Properties.Resources.ghostPlayerLeft;
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
                            if (player.Top > x.Top - player.Height)
                            {
                                //réinitialise la force est fixe la position du joueur au dessus de la plateforme
                                force = 8;
                                player.Top = x.Location.Y - player.Height+1;
                                //empêche le joueur de rentrer a nouveau en collision avec la plateforme tant qu'il n'a pas sauté
                                if (!jumping)
                                {
                                    jumpSpeed = 0;
                                }
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
                //collisions joueur et pièces
                if ((string)x.Tag == "coin")
                {
                    //si le joueur passe sur une pièce visible la rend invisible et incrémente le score de un
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false;
                        score++;
                    }
                }
                // si le joueur passe par le portail avec les 26 pièces arrête le jeu et lance le niveau 2 
                if (player.Bounds.IntersectsWith(door.Bounds) && score == 26)
                {
                    gameTimer.Stop();
                    isGameOver = true;
                    txtScore.Text = "Score: " + score + Environment.NewLine + "Vers niveau 2";
                }
                else
                {
                    txtScore.Text = "Score: " + score + Environment.NewLine + "récupère toute ton âme";
                }

                //si le joueur rentre en collision avec un monstre le jeu s'arrête 
                if ((string)x.Tag == "enemy")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        isGameOver = true;
                    }
                }
            }





            //si le joueur sort de l'écran de plus de 50 pixels le jeu arrête 
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "tu es tombé";
            }
            //donne une vitesse a l'ennemi 1
            enemyOne.Left -= enemyOneSpeed;
            //vérifie si l'ennemi est au bout de sa plateforme 
            if (enemyOne.Left < pictureBox5.Left || enemyOne.Left + enemyOne.Width > pictureBox5.Left + pictureBox5.Width)
            {
                //si l'ennemi est au bout de sa plateforme change le sens de direction
                enemyOneSpeed = -enemyOneSpeed;
                //change le sprite en fonction de la direction
                if(enemyOneSpeed  < 0)
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
            if (e.KeyCode == Keys.Space && jumping == false )
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
            //remet le score à 0
            score = 0;

            txtScore.Text = "Score: " + score;
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

            horizontalPlatform.Left = 275;
            verticalPlatform.Top = 581;

            gameTimer.Start();


        }
        
    }
}