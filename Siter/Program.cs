using System;
using System.Globalization;
using System.IO;

namespace Siter
{
    class Program
    {
        public void main()
        {
            try
            {
                String s = "", fecha = "------", zeros = "AAAAAA", line;
                int errores = 0;
                String path = Directory.GetCurrentDirectory();
                using (StreamReader sr = File.OpenText(path + @"\PFDATOS.txt"))
                {
                    using (StreamWriter sw = new StreamWriter(path + @"\PFDATOStemp.txt", false))
                    {
                        while ((s = sr.ReadLine()) != null)
                        {
                            if (s.StartsWith("0401")) //Si la linea empieza con 0401
                            {
                                zeros = s.Substring(42, 6); //guardo en zeros, ver si fecha=

                                if (!(zeros.Equals("000000"))) // Si hay una fecha
                                {
                                    Console.WriteLine(s + "\n");
                                    fecha = zeros; //Guardo la fecha
                                    sw.WriteLine(s); //Escribo la linea
                                }
                                else //habia ceros
                                {
                                    Console.WriteLine(s);
                                    line = s.Remove(42, 6).Insert(42, fecha); //inserto fecha
                                    errores++;
                                    sw.WriteLine(line); //escribo linea
                                    Console.WriteLine(line + "\n");
                                }
                            }
                            else sw.WriteLine(s);
                        }
                    }
                }
                File.Delete(path + @"\PFDATOS.txt");
                File.Move(path + @"\PFDATOStemp.txt", path + @"\PFDATOS.txt");
                Console.WriteLine("PFDATOS.txt OK");

                using (StreamReader sr = File.OpenText(path + @"\CAB-CTA-OPER.txt"))
                {
                    using (StreamWriter sw = new StreamWriter(path + @"\CAB-CTA-OPERtemp.txt", false))
                    {
                        while ((s = sr.ReadLine()) != null)
                        {
                            line = s.Remove(254, 1).Insert(254, "0");
                            sw.WriteLine(line);
                        }
                    }
                }

                File.Delete(path + @"\CAB-CTA-OPER.txt");
                File.Move(path + @"\CAB-CTA-OPERtemp.txt", path + @"\CAB-CTA-OPER.txt");
                Console.WriteLine("CAB-CTA-OPER.txt OK");

                int month = DateTime.Now.Month;
                String monthName, year;
                monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                year = DateTime.Now.Year.ToString();
                //monthName = char.ToUpper(monthName[0]).ToString() + monthName.Substring(1);
                monthName = "Junio";
                String nameFile = "Siter" + monthName + year;

                string file1 = File.ReadAllText(path + @"\CAB-CTA-OPER.txt");
                string file2 = File.ReadAllText(path + @"\CTADATOS.txt");
                string file3 = File.ReadAllText(path + @"\PFDATOS.txt");
                File.WriteAllText((path + @"\" + nameFile + ".txt"), file1 + file2 + file3);
                Console.WriteLine(nameFile + ".txt" + " OK. " + errores + " encontrados.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            Console.WriteLine("Presione cualquier tecla para salir...");
            System.Console.ReadKey();
        }
    }
}