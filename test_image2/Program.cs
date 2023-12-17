using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace test_image2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Spécifie le chemin d'accès à votre image BMP
            string imagePath = "../../images/imagesReelles/2469s.bmp";

            // Transforme l'image en tableau 2D
            int[,] tabImage = TabFromFile(imagePath);

            int[,] carte = BruteForceSEDT(tabImage); // calcul de la SEDT

            Affiche_image(carte);
            SaveImage(carte, "../../images/imagesReelles/SEDT/2469s-SEDT.bmp");

            Console.ReadKey();

        }

        // Definition de la fonction TabFromFile : cree le tableau 2D stockant la valeur des pixels de l'image situe au bout du xhemin Xfile
        /* TabFromFile : fonc : int [,]
        parametre:
            Xfile : string : le chemin vers l'image bmp
        retour :
            tabImage : int [,] : le tableau stockant la valeur des pixels de l'image
        local :
            Bitmap : img : l'image 
        */
        public static int[,] TabFromFile(string Xfile)
        {
            Bitmap img = new Bitmap(Xfile);
            int[,] tabImage = ImageToInt(img);
            return tabImage;
        }

        // Definition de la fonction ImageToInt : cree le tableau 2D stockant la valeur des pixels de l'image Ximg
        /* ImageToInt : fonc : int [,]
        parametre:
            Ximg : Bitmap : l'image au format Bitmap
        retour :
            tab : int [,] : le tableau stockant la valeur des pixels de l'image Ximg
        local :
             
        */
        public static int[,] ImageToInt(Bitmap Ximg)
        {
            int largeur = Ximg.Width;
            int hauteur = Ximg.Height;
            int[,] tab = new int[hauteur, largeur];
            for(int lig=0;lig<hauteur; lig++)
            {
                for(int col=0; col<largeur; col++)
                {
                    Color c = Ximg.GetPixel(col, lig);

                    tab[lig, col] = (int)c.R;
                }
            }
            return tab;
        }

        // Definition de la fonction IntToImage : remplie Ximg à partir de Xtab
        /* IntToImage : proc : void
        parametre:
            Ximg : Bitmap : l'image au format Bitmap
            Xtab : int [,] : le tableau stockant la valeur des pixels de l'image Ximg
        retour :
            
        local :
             
        */
        public static void IntToImage(int[,] Xtab, Bitmap Ximg)
        {
            
            int largeur = Xtab.GetLength(1);
            int hauteur = Xtab.GetLength(0);
            for (int lig = 0; lig < hauteur; lig++)
            {
                for (int col = 0; col < largeur; col++)
                {
                    Color c = Color.FromArgb(255,Xtab[lig, col], Xtab[lig, col], Xtab[lig, col]);
                    Ximg.SetPixel(col, lig, c);
                }
            }
            
        }

        // Definition de la fonction saveImage : sauvegarde l'image dont la valeur des pixels est situee dans Xtab, au bout du chemin Xfile
        /* IntToImage : proc : void
        parametre:
            Xfile : string : chemin de l'image au format Bitmap
            Xtab : int [,] : le tableau stockant la valeur des pixels de l'image Ximg
        retour :
            
        local :
             
        */
        public static void SaveImage(int[,] Xtab, string Xfile)
        {
            Bitmap img = new Bitmap(Xtab.GetLength(1), Xtab.GetLength(0));

            IntToImage(Xtab, img);
            img.Save(Xfile);
            Console.WriteLine("Saugarde dans le fichier : " + Xfile);
        }

        // Definition de la fonction affiche_image : affiche l'image dont la valeur des pixels est situee dans Xtab
        /* affiche_image : proc : void
        parametre:
            Xtab : int [,] : le tableau stockant la valeur des pixels
        retour :
            
        local :
             
        */
        public static void Affiche_image(int[,] Xtab)
        {
            Bitmap img = new Bitmap(Xtab.GetLength(1), Xtab.GetLength(0));
            IntToImage(Xtab,img);
            Form f = new Form();
            f.BackgroundImage=img;
            f.Width = img.Width;
            f.Height = img.Height;
            f.Show();
        }

        /// <summary>
        /// Calcule la distance euclidienne carrée entre P et Q
        /// </summary>
        /// <param name="Xp">Coordonnées de P (tableau de taille 2)</param>
        /// <param name="Xq">Coordonnées de Q (tableau de taille 2)</param>
        /// <returns>Distance euclidienne carrée entre P et Q</returns>
        public static int DistEuclidienne(int[] Xp, int[] Xq)
        {
            return (int)(Math.Pow(Xp[0] - Xq[0], 2) + Math.Pow(Xp[1] - Xq[1], 2));
        }

        /// <summary>
        /// Affiche le tableau 2D
        /// </summary>
        /// <param name="Xtab"></param>
        public static void PrintTab(int[,] Xtab)
        {
            for (int i = 0; i < Xtab.GetLength(0); i++)
            {
                for (int j = 0; j < Xtab.GetLength(1); j++)
                {
                    Console.Write($"{Xtab[i, j]} ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Affiche la progression des calculs
        /// </summary>
        /// <param name="current">Progression actuelle</param>
        /// <param name="finish">Objectif à atteindre</param>
        public static void PrintProgress(int current, int finish)
        {
            double percent = Math.Round((double)current / (double)finish * 100);
            Console.Clear();
            Console.WriteLine($"Calcul en cours... ({current}/{finish} - {percent}%)");
        }

        /// <summary>
        /// Transforme la carte de distances en une image de la forme en niveux de gris
        /// </summary>
        /// <param name="Xtab">Tableau</param>
        /// <param name="ratio"></param>
        public static void Normalize(int[,] Xtab, double ratio)
        {
            for (int i = 0; i < Xtab.GetLength(0); i++)
            {
                for (int j = 0; j < Xtab.GetLength(1); j++)
                {
                    int px = Xtab[i, j]; 
                    if (px == -1)
                        Xtab[i, j] = 255;
                    else
                        Xtab[i, j] = (int)(px * ratio);
                }
            }
        }

        /// <summary>
        /// Retourne la valeur maximale du tableau
        /// </summary>
        /// <param name="Xtab">Tableau</param>
        /// <returns>Valeur maximale</returns>
        public static double Max(int[,] Xtab)
        {
            int max = int.MinValue;
            for (int i = 0; i < Xtab.GetLength(0); i++)
            {
                for (int j = 0; j < Xtab.GetLength(1); j++)
                {
                    if (Xtab[i, j] > max) max = Xtab[i, j];
                }
            }

            return max;
        }

        /// <summary>
        /// Retourne un tableau 2D de même taille
        /// </summary>
        /// <param name="Xtab">Tableau à cloner</param>
        /// <returns>Nouveau tableau</returns>
        public static int[,] CloneTabEmpty(int[,] Xtab)
        {
            return new int[Xtab.GetLength(0), Xtab.GetLength(1)];
        }

        /// <summary>
        /// Calcule la transformée en distance entre le pixel P et le fond
        /// </summary>
        /// <param name="Xp">Pixel P</param>
        /// <param name="Xbg">Fond</param>
        /// <returns>Transformée en distance</returns>
        public static int TransfoDist(int[] Xp, int[,] Xbg)
        {
            int minDist = int.MaxValue;

            for (int i = 0; i < Xbg.GetLength(0); i++)
            {
                for (int j = 0; j < Xbg.GetLength(1); j++)
                {
                    if (Xbg[i, j] == 0)
                    {
                        int dist = DistEuclidienne(Xp, new int[2] { i, j });
                        if (dist < minDist) minDist = dist;
                    }
                }
            }

            return minDist;
        }

        /// <summary>
        /// Retourne le tableau correspondant au fond de l'image (méthode bruteforce)
        /// </summary>
        /// <param name="Xtab">Tableau</param>
        /// <returns>Fond de l'image</returns>
        public static int[,] BackgroundImg(int[,] Xtab)
        {
            int[,] bg = CloneTabEmpty(Xtab);

            for (int i = 0; i < Xtab.GetLength(0); i++)
            {
                for (int j = 0; j < Xtab.GetLength(1); j++)
                {
                    if (Xtab[i, j] != 255) bg[i, j] = 255;
                    else bg[i, j] = 0;
                }
            }

            return bg;
        }

        /// <summary>
        /// Calcule la SEDT
        /// </summary>
        /// <param name="Xtab">Image</param>
        /// <returns>Carte des distances</returns>
        public static int[,] BruteForceSEDT(int[,] Xtab)
        {
            int height = Xtab.GetLength(0);
            int width = Xtab.GetLength(1);
            int progress = 0;

            int[,] bg = BackgroundImg(Xtab);
            int[,] carte = CloneTabEmpty(Xtab);
            
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bg[i, j] == 0) carte[i, j] = -1;
                    else
                        carte[i, j] = TransfoDist(new int[2] { i, j }, bg);
                    progress++;
                    PrintProgress(progress, height * width);
                }
            }

            double ratio = 255 / Max(carte);

            Normalize(carte, ratio);
            return carte;
        }
    }
}
