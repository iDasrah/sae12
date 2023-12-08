// Auteur : Aurélie Leborgne
// Calcule la SEDT et les boules maximales d'une forme
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

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

            //Ajoutez ici d'autres traitements ou analyse du tableau
            
            tabImage = CarteDistEucBF(tabImage);

            Affiche_image(tabImage);

            SaveImage(tabImage, "../../images/imagesReelles/SEDT/2469s-SEDT.bmp");


            // Attendez que l'utilisateur appuie sur une touche avant de fermer la console
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

        public static int MinTab(int[,] Xtab)
        {
            int min = Xtab[0,0];

            for (int i = 0; i < Xtab.GetLength(0); i++)
            {
                for (int j = 0; j < Xtab.GetLength(1); j++)
                {
                    if (Xtab[i,j] < min)
                        min = Xtab[i,j];
                }
            }

            return min;
        }

        public static int DistEuc(int xP, int yP, int xQ, int yQ)
        {
            return (int)Math.Pow(xQ - xP, 2) + (int)Math.Pow(yQ - yP, 2);
        }

        public static int[,] BackgroundImg(int[,] Xtab)
        {
            int[,] bg = new int[Xtab.GetLength(0), Xtab.GetLength(1)];

            for (int i = 0;  i < Xtab.GetLength(0); i++)
            {
                for (int j = 0;  j < Xtab.GetLength(1);  j++)
                {
                    if (Xtab[i, j] == 255)
                    {
                        bg[i, j] = 1;
                    } else
                    {
                        bg[i, j] = 0;
                    }
                }
            }

            return bg;
        }

        public static int TransformeDist(int xP, int yP, int[,] bg)
        {
            int[,] trans = new int[bg.GetLength(0), bg.GetLength(1)];
            for (int i = 0; i <  bg.GetLength(0); i++)
            {
                for (int j = 0;j < bg.GetLength(1); j++)
                {
                    trans[i, j] = DistEuc(xP, yP, i, j);
                }
            }
            return MinTab(trans);
        }

        public static int[,] CarteDistEucBF(int[,] Xtab)
        {
            int[,] card = new int[Xtab.GetLength(0), Xtab.GetLength(1)];
            int[,] bgImg = BackgroundImg(Xtab);

            for (int i = 0; i < Xtab.GetLength(0); i++)
            {
                for (int j = 0; j < Xtab.GetLength(1); j++)
                {
                    if (bgImg[i, j] == 1)
                    {
                        card[i, j] = 255;
                        Console.WriteLine("255");
                    } else
                    {
                        int trans = TransformeDist(i, j, bgImg);
                        card[i, j] = trans;
                        Console.WriteLine(trans);
                    }
                }
            }

            return card;
        }
    }
}
