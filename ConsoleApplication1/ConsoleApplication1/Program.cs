using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using BayesSharp;

namespace ConsoleApplication1
{
    class DGLop
    {
        public string name;

        public int after_count;
        public int before_count;
        public int acc_count;

        public double p;
        public double r;
        public double f;
        
        public DGLop()
        {
            name = "";
            after_count = 0;
            before_count = 0;
            acc_count = 0;
            p = 0.0;
            r = 0.0;
            f = 0.0;
        }
    };
    class Program
    {
        static void Main(string[] args)
        {
            TXL txl = new TXL("stopwords.txt");
            BayesSimpleTextClassifier classifier = new BayesSimpleTextClassifier();
            List<DGLop> DG = new List<DGLop>();
            List<string> label_before = new List<string>();
            List<string> label_after = new List<string>();
            List<string> testlist = new List<string>();

            DirectoryInfo mydir = new DirectoryInfo(@"class\");
            FileInfo[] f = mydir.GetFiles();
            
            foreach (FileInfo file in f)
            {
                StreamReader sr = new StreamReader(file.FullName);
                string line = "";
                DGLop tempDG = new DGLop();
                tempDG.name = Path.GetFileNameWithoutExtension(file.Name);
                DG.Add(tempDG);
                while ((line = sr.ReadLine()) != null)
                {
                    classifier.Train(tempDG.name, txl.tienXuLy(line));
                }
                sr.Close();
            }

            StreamReader testinput = new StreamReader(@"test\tests.txt");
            StreamWriter resultoutput = new StreamWriter(@"test\results.txt");
            string str;
            while ((str = testinput.ReadLine()) != null)
            {
                label_before.Add(str.Split(' ').Last());
                str = str.Replace(" " + label_before.Last(), string.Empty);

                testlist.Add(str);
                Dictionary<string, double> score = classifier.Classify(str);
                
                label_after.Add(score.First().Key);

                resultoutput.WriteLine(testlist.Last() + " " + label_after.Last());
            }

            for (int i = 0; i < DG.Count; ++i)
            {
                for (int j = 0; j < label_after.Count; ++j)
                {
                    if (label_before[j].Equals(DG[i].name))
                        ++DG[i].before_count;

                    if (label_after[j].Equals(DG[i].name))
                    {
                        ++DG[i].after_count;
                        if (label_after[j].Equals(label_before[j]))
                            ++DG[i].acc_count;
                    }
                }

                DG[i].p = (double)DG[i].acc_count / DG[i].after_count;
                DG[i].r = (double)DG[i].acc_count / DG[i].before_count;
                DG[i].f = 2 * DG[i].p * DG[i].r / (DG[i].p + DG[i].r);
            }

            int C_count = DG.Count;
            double p_macro = 0, r_macro = 0, f_macro = 0, f_micro = 0;
            for (int i = 0; i < C_count; ++i)
            {
                p_macro += DG[i].p;
                r_macro += DG[i].r;
                f_micro += DG[i].acc_count;
            }
            p_macro = p_macro / C_count;
            r_macro = r_macro / C_count;
            f_macro = 2 * p_macro * r_macro / (p_macro + r_macro);
            f_micro = f_micro / (double)testlist.Count;

            for (int i = 0; i < DG.Count; ++i)
            {
                resultoutput.WriteLine("P {0}: {1}", DG[i].name, DG[i].p * 100);
                resultoutput.WriteLine("R {0}: {1}", DG[i].name, DG[i].r * 100);
                resultoutput.WriteLine("F {0}: {1}", DG[i].name, DG[i].f * 100);

            }

            resultoutput.WriteLine("F_macro: {0}", f_macro * 100);
            resultoutput.WriteLine("F_micro: {0}", f_micro * 100);

            testinput.Close();
            resultoutput.Close();
        }
    }
}
