using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class CreateInput : MonoBehaviour
{
    private MainManager mainManager;
    public CustomInput input;



    // Start is called before the first frame update
    void Start()
    {
        mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
        //WriteInputFile();
    }

    public void WriteInputFile()
    {
        StreamWriter writer = new StreamWriter(mainManager.cfdCodePath + "/input.dat", false);

        writer.WriteLine("### Variables Etape, config cavité entrainée avec cylindre au centre");
        writer.WriteLine("");
        writer.WriteLine("# n_x n_y");
        writer.WriteLine(input.n.x.ToString(CultureInfo.InvariantCulture) + " " + input.n.y.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("#t_f");
        writer.WriteLine(input.tf.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("# l_x l_y");
        writer.WriteLine(input.lx.ToString(CultureInfo.InvariantCulture) + " " + input.ly.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("# frame");
        writer.WriteLine(input.frame.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("# density");
        writer.WriteLine(input.density.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("# cfl");
        writer.WriteLine(input.cfl.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("# Fo");
        writer.WriteLine(input.fo.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("# Reynolds number");
        writer.WriteLine(input.re.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("# Longueur caractéristique");
        writer.WriteLine(input.characteristicLength.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("# Vitesse caractéristique");
        writer.WriteLine(input.characteristicSpeed.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("# Chosen Scheme ('UR1' pour upwind régressif ou 'CD4' pour centré ordre 2 ou 'CD2' ordre 2)");
        writer.WriteLine(input.scheme);
        writer.WriteLine("");
        writer.WriteLine("## Conditions initiales");
        writer.WriteLine("");
        writer.WriteLine("# Cavite entrainee ('CE') ou conditions custom ('CC')");

        if (input.custom == true)
        {
            writer.WriteLine("CC");
        } else
        {
            writer.WriteLine("CE");
        }
        
        writer.WriteLine("");
        writer.WriteLine("# Big borders u:");
        writer.WriteLine(input.uSides[3].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine(input.uSides[0].ToString(CultureInfo.InvariantCulture) + " " + input.uSides[1].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine(input.uSides[2].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("# Big borders v:");
        writer.WriteLine(input.vSides[3].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine(input.vSides[0].ToString(CultureInfo.InvariantCulture) + " " + input.vSides[1].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine(input.vSides[2].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("# Global u, v");
        writer.WriteLine(input.uIni.ToString(CultureInfo.InvariantCulture) + " " + input.vIni.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("[si toutes les conditions géometriques sont inutiles, mettre 0 0 0 0 -1]");
        writer.WriteLine("# Obstacles polynomials coeff a, b, c, d, e (a*x^2 + b*x + c*y^2 + d*y <= e) [si certaines conditions inutiles, mettre 0 0 0 0 1]");
        writer.WriteLine(input.poly[0, 0].ToString(CultureInfo.InvariantCulture) + " " + input.poly[0, 1].ToString(CultureInfo.InvariantCulture) + " " + input.poly[0, 2].ToString(CultureInfo.InvariantCulture) + " " + input.poly[0, 3].ToString(CultureInfo.InvariantCulture) + " " + input.poly[0, 4].ToString(CultureInfo.InvariantCulture) + " ");
        writer.WriteLine(input.poly[1, 0].ToString(CultureInfo.InvariantCulture) + " " + input.poly[1, 1].ToString(CultureInfo.InvariantCulture) + " " + input.poly[1, 2].ToString(CultureInfo.InvariantCulture) + " " + input.poly[1, 3].ToString(CultureInfo.InvariantCulture) + " " + input.poly[1, 4].ToString(CultureInfo.InvariantCulture) + " ");
        writer.WriteLine("# Point de référence des polynomes (x, y)");
        writer.WriteLine(input.refPointPoly.x.ToString(CultureInfo.InvariantCulture) + " " + input.refPointPoly.y.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("# Obstacles rectangulaires : a*abs(x) + b*abs(y) <= c [si certaines conditions inutiles, mettre 0 0 1]");
        writer.WriteLine(input.square[0, 0].ToString(CultureInfo.InvariantCulture) + " " + input.square[0, 1].ToString(CultureInfo.InvariantCulture) + " " + input.square[0, 2].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine(input.square[1, 0].ToString(CultureInfo.InvariantCulture) + " " + input.square[1, 1].ToString(CultureInfo.InvariantCulture) + " " + input.square[1, 2].ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("# Point de référence des rectangles (x, y)");
        writer.WriteLine(input.refPointSquare.x.ToString(CultureInfo.InvariantCulture) + " " + input.refPointSquare.y.ToString(CultureInfo.InvariantCulture));
        writer.WriteLine("");
        writer.WriteLine("### Des conditions impossibles physiquement resulteront en un résultat absurde, il n'y a aucun avertissement de la part du programme.");

        writer.Close();
    }
}
