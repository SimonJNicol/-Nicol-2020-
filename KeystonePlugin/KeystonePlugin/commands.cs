using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Windows.Forms;

namespace KeystonePlugin
{
    public class commands
    {
       public static void Main(string[] args)
        {
            Console.WriteLine("We are in the main for some reason");
        }
        /*        [CommandMethod("isflat")]
                public static void isflat()
                {

                } */


                // TO DO: add shear and moment calculations


        [CommandMethod("ap")] //Calculate the Area of a polyline
        public static void ap()
        {
            Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("Area of polyline: " + calcs.areaPoly() + " ft^2.");
        }
        [CommandMethod("plf")]
        public static void plf()
        {
            Document apdoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions strOpts = new PromptStringOptions("\nPlease enter the width of the area which the beam supports in feet: ");
            strOpts.AllowSpaces = false;
            PromptResult strRes = apdoc.Editor.GetString(strOpts);

            double[] plf = calcs.plf(calcs.inputLoads(), calcs.tribWidth(double.Parse(strRes.StringResult)));
            Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nLive load: " + plf[0] + " plf." + "\nDead Load: " + plf[1] + " plf.");

        }

        [CommandMethod("ult")]
        public static void uniformLoadTest()
        {
            Document apdoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions strOpts = new PromptStringOptions("\nPlease enter the uniform live load in pounds per linear foot: ");
            strOpts.AllowSpaces = false;
            PromptResult strRes = apdoc.Editor.GetString(strOpts);

            double[] plf = { 0, 0 };
            plf[0] = double.Parse(strRes.StringResult)/12; //loads stored in lbs per linear inch

            strOpts.Message = "\nPlease enter the uniform dead load in pounds per linear foot: ";
            strRes = apdoc.Editor.GetString(strOpts);
            plf[1] = double.Parse(strRes.StringResult)/12; //loads stored in lbs per linear inch

            strOpts.Message = "\nPlease enter the length of the beam in feet: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double beamLength = double.Parse(strRes.StringResult)*12; //Beam length stored in inches

            strOpts.Message = "\nPlease enter the modulus of elasticity in pounds force per square inch: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double elasticity = double.Parse(strRes.StringResult);

            strOpts.Message = "\nPlease enter the planar moment of inertia in quartic inches: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double mInertia = double.Parse(strRes.StringResult);

            calcs.uniformLoadTest(plf, beamLength, elasticity, mInertia);

        }
        [CommandMethod("plt")]
        public static void pointLoadTest()
        {
            Document apdoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions strOpts = new PromptStringOptions("\nPlease enter the point live load in pounds: ");
            strOpts.AllowSpaces = false;
            PromptResult strRes = apdoc.Editor.GetString(strOpts);

            double[] lbs = { 0, 0 };
            lbs[0] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs

            strOpts.Message = "\nPlease enter the point dead load in pounds: ";
            strRes = apdoc.Editor.GetString(strOpts);
            lbs[1] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs

            strOpts.Message = "\nPlease enter the length of the beam in feet: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double beamLength = double.Parse(strRes.StringResult) * 12; //Beam length stored in inches

            strOpts.Message = "\nPlease enter the modulus of elasticity in pounds force per square inch: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double elasticity = double.Parse(strRes.StringResult);

            strOpts.Message = "\nPlease enter the planar moment of inertia in quartic inches: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double mInertia = double.Parse(strRes.StringResult);

            calcs.pointLoadTest(lbs, beamLength, elasticity, mInertia);

        }
        [CommandMethod("clt")]
        public static void combinedLoadTest() //Only works for a point load located in the center of the beam
        {
            Document apdoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions strOpts = new PromptStringOptions("\nPlease enter the uniform live load in pounds per linear foot: ");
            strOpts.AllowSpaces = false;
            PromptResult strRes = apdoc.Editor.GetString(strOpts);

            double[] plf = { 0, 0 };
            double[] lbs = { 0, 0 };
            plf[0] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs per linear inch

            strOpts.Message = "\nPlease enter the uniform dead load in pounds per linear foot: ";
            strRes = apdoc.Editor.GetString(strOpts);
            plf[1] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs per linear inch

            strOpts.Message = "\nPlease enter the point live load in pounds: ";
            strRes = apdoc.Editor.GetString(strOpts);
            lbs[0] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs

            strOpts.Message = "\nPlease enter the point dead load in pounds: ";
            strRes = apdoc.Editor.GetString(strOpts);
            lbs[1] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs

            strOpts.Message = "\nPlease enter the length of the beam in feet: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double beamLength = double.Parse(strRes.StringResult) * 12; //Beam length stored in inches

            strOpts.Message = "\nPlease enter the modulus of elasticity in pounds force per square inch: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double elasticity = double.Parse(strRes.StringResult);

            strOpts.Message = "\nPlease enter the planar moment of inertia in quartic inches: ";
            strRes = apdoc.Editor.GetString(strOpts);
            double mInertia = double.Parse(strRes.StringResult);

            calcs.combinedLoadTest(lbs, plf, beamLength, elasticity, mInertia);

        }
        [CommandMethod("ultf")]
        public static void uniformLoadTestWithForm()
        {
            Program.Main(); //create the form
            string fileName = @"C:\Users\PC\source\repos\WindowsFormsKeystonePlug\WindowsFormsKeystonePlug\bin\Debug\output.txt";
            string fileName2 = @"C:\Users\PC\source\repos\KeystonePlugin\KeystonePlugin\input\ModulusesofElasticity.3.5.2020.csv";
            string fileName3 = @"C:\Users\PC\source\repos\KeystonePlugin\KeystonePlugin\input\MomentsofInertia.3.5.2020.csv";
            string input = File.ReadAllText(fileName);

            string tempElasticityDB = File.ReadAllText(fileName2);
            string tempMInertiaDB = File.ReadAllText(fileName3);
            string[,] elasticityDB = new string[3, 313];
            string[,] mInertiaDB = new string[3, 165];

            string[] temp = input.Split(',');
            string[] temp2 = tempElasticityDB.Split(',');
            string[] temp3 = tempMInertiaDB.Split(',');

            string woodQuality = temp[0];
            string woodSpecies = temp[1];
            string cutType = temp[2];
            string minResults = temp[3];
            double beamLength = double.Parse(temp[4]);
            double[] plf = { double.Parse(temp[5]), double.Parse(temp[6]) };

            int k = 0;
            int i = 0;
            int c = 0;

            foreach (string line in temp2)
            {
                elasticityDB[i, c] = temp2[k];
                if (i < 2) i++;
                else
                {
                    i = 0;
                    c++;
                }
                k++;
            }

            k = 0;
            i = 0;
            c = 0;

            foreach (string line in temp3)
            {
                mInertiaDB[i, c] = temp3[k];
                if (i < 2) i++;
                else
                {
                    i = 0;
                    c++;
                }
                k++;
            }
            //Deal with data parsing...

        }
        /*        [CommandMethod("ar")] //Calculate Area of a rectangle
                public static void  ar()
                {

                }
                */
    }

    class calcs
    {
/*        public static void Main()
        {
            Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("ERROR! In Calcs.Main routine.");
        } */
        public static double areaPoly()
        {
            Document apdoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions strOpts = new PromptStringOptions("\nPlease enter the number of sides of the polyline: ");
            strOpts.AllowSpaces = false;
            PromptResult strRes = apdoc.Editor.GetString(strOpts);
            int maxCount = Int16.Parse(strRes.StringResult) + 1;

            PromptPointResult pRes;
            Point2dCollection pCol = new Point2dCollection();
            PromptPointOptions pOpts = new PromptPointOptions("");

            pOpts.Message = "\nEnter the first point of the polyline.";
            pRes = apdoc.Editor.GetPoint(pOpts);
            pCol.Add(new Point2d(pRes.Value.X, pRes.Value.Y));

            if (pRes.Status == PromptStatus.Cancel)
            {
                return -1;
            }
            int i = 0;

            while (i < maxCount - 1)
            {
                pOpts.Message = "\nEnter point number " + (i + 2) + ": ";
                pOpts.UseBasePoint = true;
                pOpts.BasePoint = pRes.Value;

                pRes = apdoc.Editor.GetPoint(pOpts);
                pCol.Add(new Point2d(pRes.Value.X, pRes.Value.Y));
                if (pRes.Status == PromptStatus.Cancel)
                {
                    return -1;
                }
                i++;
            }
            i = 0;
            using (Polyline poly = new Polyline())
            {
                while (i < maxCount)
                {
                    poly.AddVertexAt(i, pCol[i], 0, 0, 0);
                    i++;
                }
                poly.Closed = true;
                return double.Parse(poly.Area.ToString()) / 144; //dividing by 144 to convert sq in to sq ft
            }
        }

        public static double tribWidth(double width)
        {
            return width / 2;
        }
        public static int[] inputLoads()
        {
            int[] loads = { 0, 0 };
            Document apdoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            PromptStringOptions strOpts = new PromptStringOptions("\nPlease enter the live load in psf (lbs/ft^2): ");
            strOpts.AllowSpaces = false;
            PromptResult strRes = apdoc.Editor.GetString(strOpts);
            loads[0] = Int16.Parse(strRes.StringResult); //Live load
            strOpts.Message = "Please enter the dead load in psf (lbs/ft^2): ";
            strRes = apdoc.Editor.GetString(strOpts);
            loads[1] = Int16.Parse(strRes.StringResult); //Dead load
            return loads;
        }
        public static double[] plf(int[] loads, double tribWidth)
        {
            double[] plf = { loads[0] * tribWidth, loads[1] * tribWidth };
            return plf;
        }
        public static double ftToIn(double dim)
        {
            return dim * 12;

        }
        public static void uniformLoadTest(double[] plf, double beamLength, double elasticity, double mInertia)
        {
            double totPlf = plf[0] + plf[1];
            double totMaxDeflection = (5 * totPlf * beamLength * beamLength * beamLength * beamLength) / (384 * elasticity * mInertia);
            double liveMaxDeflection = (5 * plf[0] * beamLength * beamLength * beamLength * beamLength) / (384 * elasticity * mInertia);
            if (totMaxDeflection <= beamLength / 240 && liveMaxDeflection <= beamLength / 360)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam passes total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam passes live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else if (liveMaxDeflection > beamLength / 360 && totMaxDeflection <= beamLength / 240)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam passes total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam failed live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else if (liveMaxDeflection > beamLength / 360 && totMaxDeflection > beamLength / 240)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam failed total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam failed live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nERROR! PROCESS ABORTED");
            }
        }
        public static void pointLoadTest(double[] lbs, double beamLength, double elasticity, double mInertia)
        {
            double totLbs = lbs[0] + lbs[1];
            double totMaxDeflection = (totLbs * beamLength * beamLength * beamLength) / (48 * elasticity * mInertia);
            double liveMaxDeflection = (lbs[0] * beamLength * beamLength * beamLength) / (48 * elasticity * mInertia);
            if (totMaxDeflection <= beamLength / 240 && liveMaxDeflection <= beamLength / 360)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam passes total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam passes live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else if (liveMaxDeflection > beamLength / 360 && totMaxDeflection <= beamLength / 240)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam passes total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam failed live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else if (liveMaxDeflection > beamLength / 360 && totMaxDeflection > beamLength / 240)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam failed total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam failed live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nERROR! PROCESS ABORTED");
            }
        }
        public static void combinedLoadTest(double[] lbs, double[] plf, double beamLength, double elasticity, double mInertia)
        {
            double totLbs = lbs[0] + lbs[1];
            double totPlf = plf[0] + plf[1];
            double totMaxDeflection = ((totLbs * beamLength * beamLength * beamLength) / (48 * elasticity * mInertia)) + ((5 * (totPlf / 12) * beamLength * beamLength * beamLength * beamLength) / (384 * elasticity * mInertia));
            double liveMaxDeflection = ((lbs[0] * beamLength * beamLength * beamLength) / (48 * elasticity * mInertia)) + ((5 * (plf[0] / 12) * beamLength * beamLength * beamLength * beamLength) / (384 * elasticity * mInertia));

            if (totMaxDeflection <= beamLength / 240 && liveMaxDeflection <= beamLength / 360)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam passes total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam passes live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else if (liveMaxDeflection > beamLength / 360 && totMaxDeflection <= beamLength / 240)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam passes total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam failed live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else if (liveMaxDeflection > beamLength / 360 && totMaxDeflection > beamLength / 240)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nBeam failed total load deflection by " + ((((beamLength / 240) / totMaxDeflection) * 100) - 100) + "%."
                + "\nBeam failed live load deflection by " + (((beamLength / 360) / liveMaxDeflection * 100) - 100) + "%.");
            }
            else
            {
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog("\nERROR! PROCESS ABORTED");
            }
        }
    }
    /*
    class forms
    {
        public static void uniformLoadTest()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new Form());

            double[] plf = { 0, 0 };
            plf[0] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs per linear inch
            plf[1] = double.Parse(strRes.StringResult) / 12; //loads stored in lbs per linear inch
            double beamLength = double.Parse(strRes.StringResult) * 12; //Beam length stored in inches
            double elasticity = double.Parse(strRes.StringResult);
            double mInertia = double.Parse(strRes.StringResult);

            calcs.uniformLoadTest(plf, beamLength, elasticity, mInertia);
    
        }
    }*/
}
